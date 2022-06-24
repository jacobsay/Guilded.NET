using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using Guilded.Base;
using Guilded.Base.Events;
using Guilded.Base.Users;
using Newtonsoft.Json.Linq;

using RestSharp;
using Websocket.Client;

namespace Guilded;

/// <summary>
/// Represents the base for all Guilded clients.
/// </summary>
/// <remarks>
/// <para>There is not much to be used here. It is recommended to use <see cref="GuildedBotClient" />.</para>
/// </remarks>
/// <seealso cref="GuildedBotClient" />
/// <seealso cref="BaseGuildedClient" />
public abstract partial class AbstractGuildedClient : BaseGuildedClient
{
    #region Fields
    /// <summary>
    /// An observable event that occurs once Guilded client has connected and added finishing touches.
    /// </summary>
    protected Subject<Me> PreparedSubject = new();
    #endregion

    #region Properties
    /// <inheritdoc cref="PreparedSubject" />
    public IObservable<Me> Prepared => PreparedSubject.AsObservable();

    /// <inheritdoc cref="WelcomeEvent.User" />
    public Me? Me { get; protected set; }

    /// <summary>
    /// Whether the client is <see cref="Prepared">prepared</see>.
    /// </summary>
    /// <value>Client is prepared</value>
    public bool IsPrepared { get; protected set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new base instance of <see cref="AbstractGuildedClient" /> children types.
    /// </summary>
    /// <seealso cref="GuildedBotClient()" />
    /// <seealso cref="GuildedBotClient(string)" />
    protected AbstractGuildedClient()
    {
        #region Event list
        // Dictionary of supported events, so we wouldn't need to manually do it.
        // The only manual work to be done is in AbstractGuildedClient.Messages.cs file,
        // which only allows us to subscribe to events and it is literally +1 member
        // to be added and copy pasting for the most part.
        GuildedEvents = new Dictionary<object, IEventInfo<object>>
        {
            // Event messages
            { SocketOpcode.Welcome,            new EventInfo<WelcomeEvent>() },
            { SocketOpcode.Resume,             new EventInfo<ResumeEvent>() },

            // Team events
            { "TeamMemberJoined",              new EventInfo<MemberJoinedEvent>() },
            { "TeamMemberUpdated",             new EventInfo<MemberUpdatedEvent>() },
            { "teamRolesUpdated",              new EventInfo<RolesUpdatedEvent>() },
            { "TeamXpAdded",                   new EventInfo<XpAddedEvent>() },
            { "TeamMemberRemoved",             new EventInfo<MemberRemovedEvent>() },
            { "TeamMemberBanned",              new EventInfo<MemberBanEvent>() },
            { "TeamMemberUnbanned",            new EventInfo<MemberBanEvent>() },
            { "TeamChannelCreated",            new EventInfo<ChannelEvent>() },
            { "TeamChannelUpdated",            new EventInfo<ChannelEvent>() },
            { "TeamChannelDeleted",            new EventInfo<ChannelEvent>() },
            { "TeamWebhookCreated",            new EventInfo<WebhookEvent>() },
            { "TeamWebhookUpdated",            new EventInfo<WebhookEvent>() },

            // Chat messages
            { "ChatMessageCreated",            new EventInfo<MessageEvent>() },
            { "ChatMessageUpdated",            new EventInfo<MessageEvent>() },
            { "ChatMessageDeleted",            new EventInfo<MessageDeletedEvent>() },
            { "ChannelMessageReactionCreated", new EventInfo<MessageReactionEvent>() },
            { "ChannelMessageReactionDeleted", new EventInfo<MessageReactionEvent>() },

            // List items
            { "ListItemCreated",               new EventInfo<ListItemEvent>() },
            { "ListItemUpdated",               new EventInfo<ListItemEvent>() },
            { "ListItemDeleted",               new EventInfo<ListItemEvent>() },
            { "ListItemCompleted",             new EventInfo<ListItemEvent>() },
            { "ListItemUncompleted",           new EventInfo<ListItemEvent>() },

            // Docs
            { "DocCreated",                    new EventInfo<DocEvent>() },
            { "DocUpdated",                    new EventInfo<DocEvent>() },
            { "DocDeleted",                    new EventInfo<DocEvent>() },

            // Calendar events
            { "CalendarEventCreated",          new EventInfo<CalendarEventEvent>() },
            { "CalendarEventUpdated",          new EventInfo<CalendarEventEvent>() },
            { "CalendarEventDeleted",          new EventInfo<CalendarEventEvent>() },
        };
        #endregion

        WebsocketMessage.Subscribe(
            OnSocketMessage,
            // Relay the error onto welcome observable
            e => GuildedEvents[(byte)1].OnError(e)
        );

        // Prepare state
        Welcome.Subscribe(welcome =>
        {
            Me = welcome.User;

            if (!IsPrepared)
            {
                PreparedSubject.OnNext(Me);
                IsPrepared = true;
            }
        });
        Disconnected.Subscribe(info =>
        {
            if (info.Type != DisconnectionType.NoMessageReceived)
                IsPrepared = false;
        });
    }
    #endregion

    #region Methods
    /// <summary>
    /// Connects <see cref="AbstractGuildedClient">this client</see> to Guilded.
    /// </summary>
    /// <seealso cref="DisconnectAsync" />
    /// <seealso cref="GuildedBotClient.ConnectAsync()" />
    /// <seealso cref="GuildedBotClient.ConnectAsync(string)" />
    public override async Task ConnectAsync()
    {
        try
        {
            await Websocket.StartOrFail().ConfigureAwait(false);
            ConnectedSubject.OnNext(this);
        }
        catch (Exception e)
        {
            ConnectedSubject.OnError(e);
        }
    }

    /// <summary>
    /// Disconnects <see cref="AbstractGuildedClient">this client</see> from Guilded.
    /// </summary>
    /// <seealso cref="ConnectAsync" />
    /// <seealso cref="GuildedBotClient.ConnectAsync()" />
    /// <seealso cref="GuildedBotClient.ConnectAsync(string)" />
    public override async Task DisconnectAsync()
    {
        if (Websocket.IsRunning)
            await Websocket.StopOrFail(WebSocketCloseStatus.NormalClosure, "DisconnectAsync invoked").ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        await DisconnectAsync().ConfigureAwait(false);

        // They aren't disposed by DisconnectAsync, only shut down
        Websocket.Dispose();
    }

    private void EnforceLimit(string name, string value, short limit)
    {
        if (value.Length > limit)
            throw new ArgumentOutOfRangeException(name, value, $"{name} exceeds the {limit} character limit");
    }

    private void EnforceLimitOnNullable(string name, string? value, short limit)
    {
        if (value is not null) EnforceLimit(name, value, limit);
    }

    private async Task<T> GetResponseProperty<T>(RestRequest request, object key) =>
        (await ExecuteRequestAsync<JContainer>(request).ConfigureAwait(false)).Data![key]!.ToObject<T>(GuildedSerializer)!;
    #endregion
}