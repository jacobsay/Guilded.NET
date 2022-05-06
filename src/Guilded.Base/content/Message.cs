using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Guilded.Base.Embeds;
using Guilded.Base.Permissions;
using Guilded.Base.Servers;
using Guilded.Base.Users;
using Newtonsoft.Json;

namespace Guilded.Base.Content;

/// <summary>
/// Represents a message posted in a chat channel or alike.
/// </summary>
/// <remarks>
/// <para>Either an existing or a cached message. It can be found in chat, voice and stream channels, as well as threads with the same channel types as described.</para>
/// <para>This currently includes both messages of types <see cref="MessageType.Default"/> and <see cref="MessageType.System"/>, but it could be changed in the future.</para>
/// </remarks>
/// <seealso cref="Doc"/>
/// <seealso cref="ListItemBase{T}"/>
/// <seealso cref="ForumThread"/>
public class Message : ChannelContent<Guid, HashId?>, IUpdatableContent, IWebhookCreatable, IReactibleContent
{
    #region Static
    /// <summary>
    /// The count of how many characters there can be in <see cref="Content">message's content</see>.
    /// </summary>
    /// <value>Limit</value>
    public const short TextLimit = 4000;
    /// <summary>
    /// The count of how many <see cref="Embeds">embeds</see> there can be in <see cref="Message">a message</see>.
    /// </summary>
    /// <value>Limit</value>
    public const byte EmbedLimit = 1;
    /// <summary>
    /// The count of how many <see cref="Message">messages</see> can be replied to per <see cref="Message">message</see>.
    /// </summary>
    /// <value>Limit</value>
    public const byte ReplyLimit = 5;
    #endregion

    #region JSON properties

    #region Content
    /// <summary>
    /// Gets the text contents of the message.
    /// </summary>
    /// <remarks>
    /// <para>The contents are formatted in Markdown. This includes images and videos, which are in the format of <c>![](source_url)</c>.</para>
    /// </remarks>
    /// <value>Markdown string?</value>
    public string? Content { get; }
    /// <summary>
    /// Gets the list of <see cref="Message">messages</see> being replied to.
    /// </summary>
    /// <remarks>
    /// <para>The max reply limit is 5.</para>
    /// </remarks>
    /// <value>List of message IDs?</value>
    public IList<Guid>? ReplyMessageIds { get; }
    /// <summary>
    /// Gets the list of <see cref="Embed">custom embeds</see> that this message contains.
    /// </summary>
    /// <remarks>
    /// <para>The max embed limit as of now is 1.</para>
    /// </remarks>
    /// <value>List of embeds?</value>
    public IList<Embed>? Embeds { get; }
    /// <summary>
    /// Gets whether <see cref="IsReply">the reply</see> or mention is private.
    /// </summary>
    /// <remarks>
    /// <para>This can only be <see langword="true"/> if <see cref="ReplyMessageIds"/> has a value or there is an user or role mention in the <see cref="Content" />.</para>
    /// </remarks>
    /// <value>Message is private</value>
    public bool IsPrivate { get; }
    /// <summary>
    /// Gets whether the specified message is a reply
    /// </summary>
    /// <value>Message is a reply</value>
    public bool IsReply => ReplyMessageIds?.Count > 0;
    #endregion

    /// <summary>
    /// Gets the identifier of <see cref="Webhook">the webhook</see> that created the message.
    /// </summary>
    /// <value>Webhook ID?</value>
    public Guid? CreatedByWebhook { get; }
    /// <summary>
    /// Gets the date when the message was edited.
    /// </summary>
    /// <remarks>
    /// <para>Only returns the most recent update.</para>
    /// </remarks>
    /// <value>Updated at?</value>
    public DateTime? UpdatedAt { get; }
    /// <summary>
    /// Gets the type of the message.
    /// </summary>
    /// <remarks>
    /// <para>Distinguishes the messages by what content they contain.</para>
    /// </remarks>
    /// <value>Message type</value>
    public MessageType Type { get; }
    /// <summary>
    /// Gets whether the specified message is a system message.
    /// </summary>
    /// <remarks>
    /// <para>A system message is a message that is created automatically on specific events, such as renaming the channel. Usually, it's something like "User has renamed the channel from X to Y"</para>
    /// </remarks>
    /// <value>Message is a system message</value>
    public bool IsSystemMessage => Type == MessageType.System;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of <see cref="Message" /> from the specified JSON properties.
    /// </summary>
    /// <param name="id">The identifier of the message</param>
    /// <param name="channelId">The identifier of the channel where the message is</param>
    /// <param name="serverId">The identifier of the server where the message is</param>
    /// <param name="content">The text contents of the message</param>
    /// <param name="replyMessageIds">Gets the list of <see cref="Message">messages</see> being replied to</param>
    /// <param name="embeds">Gets the list of <see cref="Embed">custom embeds</see> that this message contains</param>
    /// <param name="isPrivate">Whether the reply is private</param>
    /// <param name="createdBy">The identifier of <see cref="User">user</see> that created the message</param>
    /// <param name="createdByWebhookId">The identifier of the webhook that created the message</param>
    /// <param name="createdAt">the date when the message was created</param>
    /// <param name="updatedAt">the date when the message was edited</param>
    /// <param name="type">The type of the message</param>
    [JsonConstructor]
    public Message(
        [JsonProperty(Required = Required.Always)]
        Guid id,

        [JsonProperty(Required = Required.Always)]
        Guid channelId,

        [JsonProperty(Required = Required.Always)]
        HashId createdBy,

        [JsonProperty(Required = Required.Always)]
        DateTime createdAt,

        [JsonProperty(Required = Required.Always)]
        MessageType type,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        HashId? serverId = null,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        IList<Guid>? replyMessageIds = null,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string? content = null,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        IList<Embed>? embeds = null,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        bool isPrivate = false,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        Guid? createdByWebhookId = null,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        DateTime? updatedAt = null
    ) : base(id, channelId, serverId, createdBy, createdAt) =>
        (Content, ReplyMessageIds, Embeds, IsPrivate, CreatedByWebhook, UpdatedAt, Type) = (content, replyMessageIds, embeds, isPrivate, createdByWebhookId, updatedAt, type);
    #endregion

    #region Additional
    /// <summary>
    /// Creates a message in the parent channel (from <see cref="ChannelContent{T, S}.ChannelId" />).
    /// </summary>
    /// <param name="message">The text contents of the message in Markdown plain text</param>
    /// <exception cref="GuildedException"/>
    /// <exception cref="GuildedPermissionException"/>
    /// <exception cref="GuildedResourceException"/>
    /// <exception cref="GuildedRequestException"/>
    /// <exception cref="GuildedAuthorizationException"/>
    /// <exception cref="ArgumentNullException">When the <see cref="MessageContent.Content">content</see> only consists of whitespace or is <see langword="null"/> and <see cref="MessageContent.Embeds">embeds</see> are also null or its array is empty</exception>
    /// <exception cref="ArgumentOutOfRangeException">When the <see cref="MessageContent.Content"/> is above the message limit of 4000 characters</exception>
    /// <permission cref="ChatPermissions.ReadMessages">Required for reading all channel and thread messages</permission>
    /// <permission cref="ChatPermissions.SendMessages">Required for sending a message in a channel</permission>
    /// <permission cref="ChatPermissions.SendThreadMessages">Required for sending a message in a thread</permission>
    /// <returns>Created message</returns>
    public async Task<Message> CreateMessageAsync(MessageContent message) =>
        await ParentClient.CreateMessageAsync(ChannelId, message);
    /// <inheritdoc cref="CreateMessageAsync(MessageContent)"/>
    /// <remarks>
    /// <para>The <paramref name="content">text contents</paramref> will be formatted in Markdown.</para>
    /// </remarks>
    /// <param name="content">The text contents of the message in Markdown plain text</param>
    public async Task<Message> CreateMessageAsync(string content) =>
        await ParentClient.CreateMessageAsync(ChannelId, content).ConfigureAwait(false);
    /// <inheritdoc cref="CreateMessageAsync(string)"/>
    /// <param name="content">The text contents of the message in Markdown plain text</param>
    /// <param name="isPrivate">Whether the mention is private</param>
    /// <param name="isSilent">Whether the mention is silent and does not ping</param>
    public async Task<Message> CreateMessageAsync(string content, bool isPrivate = false, bool isSilent = false) =>
        await ParentClient.CreateMessageAsync(ChannelId, content, isPrivate, isSilent).ConfigureAwait(false);
    /// <inheritdoc cref="CreateMessageAsync(string)"/>
    /// <param name="content">The text contents of the message in Markdown plain text</param>
    /// <param name="replyTo">The array of all messages it is replying to(5 max)</param>
    public async Task<Message> CreateMessageAsync(string content, params Guid[] replyTo) =>
        await ParentClient.CreateMessageAsync(ChannelId, content, replyTo).ConfigureAwait(false);
    /// <inheritdoc cref="CreateMessageAsync(string)"/>
    /// <param name="content">The text contents of the message in Markdown plain text</param>
    /// <param name="isPrivate">Whether the reply is private</param>
    /// <param name="isSilent">Whether the reply is silent and does not ping</param>
    /// <param name="replyTo">The array of all messages it is replying to(5 max)</param>
    public async Task<Message> CreateMessageAsync(string content, bool isPrivate = false, bool isSilent = false, params Guid[] replyTo) =>
        await ParentClient.CreateMessageAsync(ChannelId, content, isPrivate, isSilent, replyTo).ConfigureAwait(false);
    /// <inheritdoc cref="CreateMessageAsync(MessageContent)" />
    /// <remarks>
    /// <para>No text contents of the message won't be displayed besides embed.</para>
    /// </remarks>
    /// <param name="embeds">The array of custom embeds that will be visible in the message</param>
    public async Task<Message> CreateMessageAsync(params Embed[] embeds) =>
        await ParentClient.CreateMessageAsync(ChannelId, embeds).ConfigureAwait(false);
    /// <inheritdoc cref="CreateMessageAsync(Embed[])" />
    /// <param name="isPrivate">Whether the reply/mention is private</param>
    /// <param name="isSilent">Whether the reply/mention is silent and does not ping</param>
    /// <param name="replyTo">The array of all messages it is replying to(5 max)</param>
    /// <param name="embeds">The array of custom embeds that will be visible in the message</param>
    public async Task<Message> CreateMessageAsync(bool isPrivate = false, bool isSilent = false, Guid[]? replyTo = null, params Embed[] embeds) =>
        await ParentClient.CreateMessageAsync(ChannelId, isPrivate, isSilent, replyTo, embeds).ConfigureAwait(false);
    /// <summary>
    /// Creates a message in the parent channel (from <see cref="ChannelContent{T, S}.ChannelId" />).
    /// </summary>
    /// <remarks>
    /// <para>The <paramref name="content">text contents</paramref> will be formatted in Markdown.</para>
    /// <para>This does not automatically include the message in the reply list.</para>
    /// <para><paramref name="embeds">Embeds</paramref> will be displayed alongside <paramref name="content">text content</paramref>.</para>
    /// </remarks>
    /// <param name="content">The text contents of the message in Markdown plain text</param>
    /// <param name="embeds">The array of custom embeds that will be visible in the message</param>
    /// <exception cref="GuildedException"/>
    /// <exception cref="GuildedPermissionException"/>
    /// <exception cref="GuildedResourceException"/>
    /// <exception cref="GuildedRequestException"/>
    /// <exception cref="GuildedAuthorizationException"/>
    /// <exception cref="ArgumentNullException">When the <paramref name="content"/> only consists of whitespace or is <see langword="null"/></exception>
    /// <exception cref="ArgumentOutOfRangeException">When the <paramref name="content"/> is above the message limit of 4000 characters</exception>
    /// <permission cref="ChatPermissions.ReadMessages">Required for reading all channel and thread messages</permission>
    /// <permission cref="ChatPermissions.SendMessages">Required for sending a message in a channel</permission>
    /// <permission cref="ChatPermissions.SendThreadMessages">Required for sending a message in a thread</permission>
    /// <returns>Created message</returns>
    public async Task<Message> CreateMessageAsync(string content, params Embed[] embeds) =>
        await ParentClient.CreateMessageAsync(ChannelId, content, embeds).ConfigureAwait(false);
    /// <inheritdoc cref="CreateMessageAsync(string, Embed[])" />
    /// <param name="content">The text contents of the message in Markdown plain text</param>
    /// <param name="isPrivate">Whether the reply is private</param>
    /// <param name="isSilent">Whether the reply is silent and does not ping</param>
    /// <param name="replyTo">The array of all messages it is replying to(5 max)</param>
    /// <param name="embeds">The array of custom embeds that will be visible in the message</param>
    public async Task<Message> CreateMessageAsync(string content, bool isPrivate = false, bool isSilent = false, Guid[]? replyTo = null, params Embed[] embeds) =>
        await ParentClient.CreateMessageAsync(ChannelId, content, isPrivate, isSilent, replyTo, embeds).ConfigureAwait(false);
    /// <summary>
    /// Replies to the message in the parent channel (from <see cref="ChannelContent{T, T}.ChannelId" />).
    /// </summary>
    /// <remarks>
    /// <para>The given text <paramref name="content" /> will be formatted in Markdown.</para>
    /// <para>Includes this message (<see cref="ChannelContent{T, S}.Id" /> property) in the reply list.</para>
    /// </remarks>
    /// <param name="content">The text contents of the message in Markdown plain text</param>
    /// <exception cref="GuildedException"/>
    /// <exception cref="GuildedPermissionException"/>
    /// <exception cref="GuildedResourceException"/>
    /// <exception cref="GuildedRequestException"/>
    /// <exception cref="GuildedAuthorizationException"/>
    /// <exception cref="ArgumentNullException">When the <paramref name="content"/> only consists of whitespace or is <see langword="null"/></exception>
    /// <exception cref="ArgumentOutOfRangeException">When the <paramref name="content"/> is above the message limit of 4000 characters</exception>
    /// <permission cref="ChatPermissions.ReadMessages">Required for reading all channel and thread messages</permission>
    /// <permission cref="ChatPermissions.SendMessages">Required for sending a message in a channel</permission>
    /// <permission cref="ChatPermissions.SendThreadMessages">Required for sending a message in a thread</permission>
    /// <returns>Created message</returns>
    public async Task<Message> ReplyAsync(string content) =>
        await CreateMessageAsync(content, Id).ConfigureAwait(false);
    /// <inheritdoc cref="ReplyAsync(string)"/>
    /// <param name="content">The text contents of the message in Markdown plain text</param>
    /// <param name="isPrivate">Whether the reply is private</param>
    /// <param name="isSilent">Whether the reply is silent and does not mention</param>
    public async Task<Message> ReplyAsync(string content, bool isPrivate = false, bool isSilent = false) =>
        await CreateMessageAsync(content, isPrivate, isSilent, Id).ConfigureAwait(false);
    /// <inheritdoc cref="ReplyAsync(string)"/>
    /// <param name="embeds">The array of custom embeds that will be visible in the message</param>
    public async Task<Message> ReplyAsync(params Embed[] embeds) =>
        await CreateMessageAsync(replyTo: new Guid[] { Id }, embeds: embeds).ConfigureAwait(false);
    /// <inheritdoc cref="ReplyAsync(string)"/>
    /// <param name="isPrivate">Whether the mention/reply is private</param>
    /// <param name="isSilent">Whether the mention/reply is silent and does not ping anyone</param>
    /// <param name="embeds">The array of custom embeds that will be visible in the message</param>
    public async Task<Message> ReplyAsync(bool isPrivate = false, bool isSilent = false, params Embed[] embeds) =>
        await CreateMessageAsync(isPrivate, isSilent, replyTo: new Guid[] { Id }, embeds).ConfigureAwait(false);
    /// <inheritdoc cref="ReplyAsync(string)"/>
    /// <param name="content">The text contents of the message in Markdown plain text</param>
    /// <param name="isPrivate">Whether the mention/reply is private</param>
    /// <param name="isSilent">Whether the mention/reply is silent and does not ping anyone</param>
    /// <param name="embeds">The array of custom embeds that will be visible in the message</param>
    public async Task<Message> ReplyAsync(string content, bool isPrivate = false, bool isSilent = false, params Embed[] embeds) =>
        await CreateMessageAsync(content, isPrivate, isSilent, replyTo: new Guid[] { Id }, embeds).ConfigureAwait(false);
    /// <inheritdoc cref="BaseGuildedClient.UpdateMessageAsync(Guid, Guid, string)"/>
    /// <param name="content">The text contents of the message in Markdown plain text</param>
    public async Task<Message> UpdateAsync(string content) =>
        await ParentClient.UpdateMessageAsync(ChannelId, Id, content).ConfigureAwait(false);
    /// <inheritdoc cref="BaseGuildedClient.DeleteMessageAsync(Guid, Guid)"/>
    public async Task DeleteAsync() =>
        await ParentClient.DeleteMessageAsync(ChannelId, Id).ConfigureAwait(false);
    /// <inheritdoc cref="BaseGuildedClient.AddReactionAsync(Guid, Guid, uint)"/>
    /// <param name="emote">The identifier of the emote to add</param>
    public async Task<Reaction> AddReactionAsync(uint emote) =>
        await ParentClient.AddReactionAsync(ChannelId, Id, emote).ConfigureAwait(false);
    // /// <inheritdoc cref="BaseGuildedClient.RemoveReactionAsync(Guid, Guid, uint)"/>
    // /// <param name="emoteId">The identifier of the emote to remove</param>
    // public async Task RemoveReactionAsync(uint emoteId) =>
    //     await ParentClient.RemoveReactionAsync(ChannelId, Id, emoteId).ConfigureAwait(false);
    #endregion
}
