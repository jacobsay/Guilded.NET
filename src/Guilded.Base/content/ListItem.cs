using System;
using System.Threading.Tasks;
using Guilded.Base.Users;
using Newtonsoft.Json;

namespace Guilded.Base.Content;

/// <summary>
/// Represents an item in a list channel.
/// </summary>
/// <remarks>
/// <para>Either an existing or a cached list item. It can only be found in list items and may sometimes be found in list threads (officially unsupported).</para>
/// </remarks>
/// <typeparam name="T">The type of the list item's note</typeparam>
/// <seealso cref="ListItemNote"/>
/// <seealso cref="ListItemNoteSummary"/>
/// <seealso cref="Content.Message"/>
/// <seealso cref="ForumThread"/>
/// <seealso cref="Doc"/>
public class ListItem<T> : ChannelContent<Guid, HashId>, IUpdatableContent, IWebhookCreatable where T : ListItemNoteSummary
{
    #region JSON properties
    /// <summary>
    /// Gets the text contents of the message in the item.
    /// </summary>
    /// <remarks>
    /// <para>The contents of the list item are formatted in Markdown. The contents must only be in a single line.</para>
    /// <para>Videos, images, code blocks and other block formatting is not supported.</para>
    /// </remarks>
    /// <value>Single-line markdown string</value>
    public string Message { get; }
    /// <summary>
    /// Gets the note of the list item.
    /// </summary>
    /// <value>List item note?</value>
    public T? Note { get; }
    /// <summary>
    /// Gets the identifier of the webhook that created the list item.
    /// </summary>
    /// <remarks>
    /// <note type="note">Currently, only chat messages can be created by Webhooks.</note>
    /// </remarks>
    /// <value>Webhook ID?</value>
    public Guid? CreatedByWebhook { get; }
    /// <summary>
    /// Gets the date of when the list item was edited.
    /// </summary>
    /// <value>Date?</value>
    public DateTime? UpdatedAt { get; }
    /// <summary>
    /// Gets the identifier of <see cref="User">user</see> who updated this list item.
    /// </summary>
    /// <value>User ID?</value>
    public HashId? UpdatedBy { get; }
    /// <summary>
    /// Gets the date of when the list item was completed.
    /// </summary>
    /// <value>Date?</value>
    public DateTime? CompletedAt { get; }
    /// <summary>
    /// Gets the identifier of <see cref="User">user</see> who ticked off this list item.
    /// </summary>
    /// <value>User ID?</value>
    public HashId? CompletedBy { get; }
    /// <summary>
    /// Gets the identifier of the parent list item of this list item.
    /// </summary>
    /// <value>User ID?</value>
    public Guid? ParentListItemId { get; }
    #endregion

    #region Properties
    /// <summary>
    /// Gets whether the list item was ticked off
    /// </summary>
    /// <returns>List item is completed</returns>
    public bool IsCompleted => CompletedAt is not null;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="ListItem{T}"/> with provided details.
    /// </summary>
    /// <param name="id">The identifier of the list item</param>
    /// <param name="channelId">The identifier of the channel where the list item is</param>
    /// <param name="serverId">The identifier of the server where the list item is</param>
    /// <param name="message">The text contents of the message in list item</param>
    /// <param name="note">The note of the list item</param>
    /// <param name="createdBy">The identifier of <see cref="User">user</see> creator of the list item</param>
    /// <param name="createdByWebhookId">The identifier of the webhook creator of the list item</param>
    /// <param name="createdAt">The date of when the list item was created</param>
    /// <param name="updatedAt">The date of when the list item was edited</param>
    /// <param name="updatedBy">The identifier of <see cref="User">user</see> updater of the list item</param>
    /// <param name="completedAt">The date of when the list item was completed</param>
    /// <param name="completedBy">The identifier of <see cref="User">user</see> completer of the list item</param>
    /// <param name="parentListItemId">The identifier of the parent list item of this list item</param>
    [JsonConstructor]
    public ListItem(
        [JsonProperty(Required = Required.Always)]
        Guid id,

        [JsonProperty(Required = Required.Always)]
        Guid channelId,

        [JsonProperty(Required = Required.Always)]
        HashId serverId,

        [JsonProperty(Required = Required.Always)]
        string message,

        [JsonProperty(Required = Required.Always)]
        HashId createdBy,

        [JsonProperty(Required = Required.Always)]
        DateTime createdAt,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        Guid? createdByWebhookId = null,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        T? note = null,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        DateTime? updatedAt = null,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        HashId? updatedBy = null,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        DateTime? completedAt = null,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        HashId? completedBy = null,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        Guid? parentListItemId = null
    ) : base(id, channelId, serverId, createdBy, createdAt) =>
        (Message, Note, CreatedByWebhook, UpdatedAt, UpdatedBy, CompletedAt, CompletedBy, ParentListItemId) = (message, note, createdByWebhookId, updatedAt, updatedBy, completedAt, completedBy, parentListItemId);
    #endregion

    #region Additional
    /// <inheritdoc cref="BaseGuildedClient.UpdateMessageAsync(Guid, Guid, string)"/>
    /// <param name="message">The new contents of the list item's message in Markdown plain text</param>
    /// <param name="note">The new contents of the list item's note in Markdown plain text</param>
    public async Task<ListItem<ListItemNote>> UpdateAsync(string message, string? note = null) =>
        await ParentClient.UpdateListItemAsync(ChannelId, Id, message, note).ConfigureAwait(false);
    /// <inheritdoc cref="BaseGuildedClient.DeleteListItemAsync(Guid, Guid)"/>
    public async Task DeleteAsync() =>
        await ParentClient.DeleteListItemAsync(ChannelId, Id).ConfigureAwait(false);
    #endregion
}
/// <summary>
/// The summary of the list item's note.
/// </summary>
/// <remarks>
/// <para>The minimal information about the list item's note.</para>
/// </remarks>
public class ListItemNoteSummary : BaseObject, ICreatableContent, IUpdatableContent
{
    #region Who, when
    /// <summary>
    /// The identifier of <see cref="User">user</see> that created the note.
    /// </summary>
    /// <value>User ID</value>
    public HashId CreatedBy { get; }
    /// <summary>
    /// The date of when the note was created.
    /// </summary>
    /// <value>Date</value>
    public DateTime CreatedAt { get; }
    /// <summary>
    /// The identifier of <see cref="User">user</see> that updated the note.
    /// </summary>
    /// <remarks>
    /// <para>The identifier of <see cref="User">user</see> that most recently updated this note.</para>
    /// </remarks>
    /// <value>User ID</value>
    public HashId? UpdatedBy { get; }
    /// <summary>
    /// The date of when the note was edited.
    /// </summary>
    /// <remarks>
    /// <para>The date of when the note was most recently updated.</para>
    /// </remarks>
    /// <value>Date</value>
    public DateTime? UpdatedAt { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="ListItemNoteSummary"/> with provided details.
    /// </summary>
    /// <param name="createdBy">The identifier of <see cref="User">user</see> that created the note</param>
    /// <param name="createdAt">The date of when the note was created</param>
    /// <param name="updatedBy">The identifier of <see cref="User">user</see> that updated the note</param>
    /// <param name="updatedAt">The date of when the note was edited</param>
    [JsonConstructor]
    public ListItemNoteSummary(
        [JsonProperty(Required = Required.Always)]
        HashId createdBy,

        [JsonProperty(Required = Required.Always)]
        DateTime createdAt,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        HashId? updatedBy = null,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        DateTime? updatedAt = null
    ) =>
        (CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) = (createdAt, createdBy, updatedAt, updatedBy);
    #endregion
}
/// <summary>
/// The information about the list item's note.
/// </summary>
/// <remarks>
/// <para>The full information about the list item's note.</para>
/// </remarks>
public class ListItemNote : ListItemNoteSummary
{
    #region JSON properties
    /// <summary>
    /// The contents of the note in the item.
    /// </summary>
    /// <remarks>
    /// <para>The contents of the list item's note formatted in Markdown.</para>
    /// </remarks>
    /// <value>Markdown string</value>
    public string Content { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="ListItemNote"/> with provided details.
    /// </summary>
    /// <param name="content">The contents of the note</param>
    /// <param name="createdBy">The identifier of <see cref="User">user</see> creator of the list item's note</param>
    /// <param name="createdAt">The date of when the list item's note was created</param>
    /// <param name="updatedBy">The identifier of <see cref="User">user</see> that updated the note</param>
    /// <param name="updatedAt">The date of when the note was edited</param>
    [JsonConstructor]
    public ListItemNote(
        [JsonProperty(Required = Required.Always)]
        string content,

        [JsonProperty(Required = Required.Always)]
        HashId createdBy,

        [JsonProperty(Required = Required.Always)]
        DateTime createdAt,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        HashId? updatedBy = null,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        DateTime? updatedAt = null
    ) : base(createdBy, createdAt, updatedBy, updatedAt) =>
        Content = content;
    #endregion
}