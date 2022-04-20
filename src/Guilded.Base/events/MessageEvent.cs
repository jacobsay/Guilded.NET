using System;
using System.Threading.Tasks;
using Guilded.Base.Content;
using Newtonsoft.Json;

namespace Guilded.Base.Events;

/// <summary>
/// Represents the base for message-related events.
/// </summary>
/// <seealso cref="Message"/>
/// <seealso cref="MessageDeletedEvent"/>
public abstract class MessageEvent<T> : BaseObject where T : BaseObject
{
    #region JSON properties
    /// <summary>
    /// The message received from the event.
    /// </summary>
    /// <value>Message</value>
    public T Message { get; }
    /// <inheritdoc />
    public HashId? ServerId { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="MessageEvent"/> from the specified JSON properties.
    /// </summary>
    /// <param name="serverId">The identifier of the server where the message event occurred</param>
    /// <param name="message">The message received from the event</param>
    protected MessageEvent(
        HashId? serverId,

        T message
    ) =>
        (ServerId, Message) = (serverId, message);
    #endregion
}
/// <summary>
/// Represents an event with the name <c>ChatMessageCreated</c> or <c>ChatMessageUpdated</c> and opcode <c>0</c> that occurs once someone creates/posts or updates/edits a <see cref="MessageEvent{T}.Message">message</see> in a channel.
/// </summary>
/// <seealso cref="MessageDeletedEvent"/>
/// <seealso cref="Message"/>
public class MessageEvent : MessageEvent<Message>
{
    #region Properties
    /// <inheritdoc cref="ChannelContent{T, S}.ChannelId"/>
    public Guid ChannelId => Message.ChannelId;
    /// <inheritdoc cref="Message.Content"/>
    public string Content => Message.Content;
    /// <inheritdoc cref="ChannelContent{T, S}.CreatedBy"/>
    public HashId CreatedBy => Message.CreatedBy;
    /// <inheritdoc cref="Message.CreatedByWebhook"/>
    public Guid? CreatedByWebhook => Message.CreatedByWebhook;
    /// <inheritdoc cref="ChannelContent{T, S}.CreatedAt"/>
    public DateTime CreatedAt => Message.CreatedAt;
    /// <inheritdoc cref="Message.UpdatedAt"/>
    public DateTime? UpdatedAt => Message.UpdatedAt;
    /// <inheritdoc cref="Message.Type"/>
    public MessageType Type => Message.Type;
    /// <inheritdoc cref="Message.IsReply"/>
    public bool IsReply => Message.IsReply;
    /// <inheritdoc cref="Message.IsSystemMessage"/>
    public bool IsSystemMessage => Message.IsSystemMessage;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="MessageEvent"/> from the specified JSON properties.
    /// </summary>
    /// <param name="serverId">The identifier of the server where the message event occurred</param>
    /// <param name="message">The message received from the event</param>
    [JsonConstructor]
    public MessageEvent(
        [JsonProperty]
        HashId? serverId,

        [JsonProperty(Required = Required.Always)]
        Message message
    ) : base(serverId, message) { }
    #endregion

    #region Additional
    /// <inheritdoc cref="Message.CreateMessageAsync(string)"/>
    public async Task<Message> CreateMessageAsync(string content) =>
        await Message.CreateMessageAsync(content).ConfigureAwait(false);
    /// <inheritdoc cref="Message.CreateMessageAsync(string, Guid[])"/>
    public async Task<Message> CreateMessageAsync(string content, params Guid[] replyMessageIds) =>
        await Message.CreateMessageAsync(content, replyMessageIds).ConfigureAwait(false);
    /// <inheritdoc cref="Message.CreateMessageAsync(string, bool, Guid[])"/>
    public async Task<Message> CreateMessageAsync(string content, bool isPrivate, params Guid[] replyMessageIds) =>
        await Message.CreateMessageAsync(content, isPrivate, replyMessageIds).ConfigureAwait(false);
    /// <inheritdoc cref="Message.ReplyAsync(string)"/>
    public async Task<Message> ReplyAsync(string content) =>
        await Message.ReplyAsync(content).ConfigureAwait(false);
    /// <inheritdoc cref="Message.ReplyAsync(string, bool)"/>
    public async Task<Message> ReplyAsync(string content, bool isPrivate) =>
        await Message.ReplyAsync(content, isPrivate).ConfigureAwait(false);
    /// <inheritdoc cref="Message.UpdateAsync(string)"/>
    public async Task<Message> UpdateAsync(string content) =>
        await Message.UpdateAsync(content).ConfigureAwait(false);
    /// <inheritdoc cref="Message.DeleteAsync"/>
    public async Task DeleteAsync() =>
        await Message.DeleteAsync().ConfigureAwait(false);
    /// <inheritdoc cref="Message.AddReactionAsync(uint)"/>
    public async Task<Reaction> AddReactionAsync(uint emoteId) =>
        await Message.AddReactionAsync(emoteId).ConfigureAwait(false);
    // /// <inheritdoc cref="Message.RemoveReactionAsync(uint)"/>
    // public async Task RemoveReactionAsync(uint emoteId) =>
    //     await Message.RemoveReactionAsync(emoteId).ConfigureAwait(false);
    #endregion
}