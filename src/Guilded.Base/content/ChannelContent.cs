using System;
using Guilded.Base.Users;

namespace Guilded.Base.Content;

/// <summary>
/// Represents the base for channel content.
/// </summary>
/// <remarks>
/// <para>This does not include deleted content.</para>
/// </remarks>
/// <typeparam name="TId">The type of the content identifier (property <see cref="Id"/>)</typeparam>
/// <typeparam name="TServer">The type of the server identifier (property <see cref="ServerId"/>)</typeparam>
public abstract class ChannelContent<TId, TServer> : ClientObject, ICreatableContent where TId : notnull
{
    #region JSON properties
    /// <summary>
    /// Gets the identifier of the content.
    /// </summary>
    /// <value>Content ID</value>
    public TId Id { get; }
    /// <summary>
    /// Gets the identifier of the channel where the content is.
    /// </summary>
    /// <value>Channel ID</value>
    public Guid ChannelId { get; }
    /// <summary>
    /// Gets the identifier of the server where the content is.
    /// </summary>
    /// <value>Server ID</value>
    public TServer ServerId { get; }

    #region Who, when
    /// <summary>
    /// Gets the identifier of <see cref="User">user</see> that created the content.
    /// </summary>
    /// <remarks>
    /// <para>If webhook or bot created this reaction, the value of this property will be <c>Ann6LewA</c>.</para>
    /// </remarks>
    /// <value>User ID</value>
    public HashId CreatedBy { get; }
    /// <summary>
    /// Gets the date when the content was created.
    /// </summary>
    /// <value>Date</value>
    public DateTime CreatedAt { get; }
    #endregion

    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="ChannelContent{T, S}" /> from the specified JSON properties.
    /// </summary>
    /// <param name="id">The identifier of the content</param>
    /// <param name="channelId">The identifier of the channel where the content is</param>
    /// <param name="serverId">The identifier of the server where the content is</param>
    /// <param name="createdBy">The identifier of <see cref="User">user</see> creator of the content</param>
    /// <param name="createdAt">the date when the content was created</param>
    protected ChannelContent(TId id, Guid channelId, TServer serverId, HashId createdBy, DateTime createdAt) =>
        (Id, ChannelId, ServerId, CreatedBy, CreatedAt) = (id, channelId, serverId, createdBy, createdAt);
    #endregion

    #region Overrides
    /// <summary>
    /// Returns whether this instance and the <paramref name="other">specified instance</paramref> are equal to each other.
    /// </summary>
    /// <param name="other">Another instance to compare</param>
    /// <returns>Instances are equal</returns>
    public override bool Equals(object? other) =>
        other is ChannelContent<TId, TServer> content && content.ChannelId == ChannelId && content.Id.Equals(Id);
    /// <summary>
    /// Returns a hashcode of this instance.
    /// </summary>
    /// <returns>HashCode</returns>
    public override int GetHashCode() =>
        HashCode.Combine(ChannelId, Id);
    /// <summary>
    /// Returns string equivalent to this instance.
    /// </summary>
    /// <returns>Instance as a string</returns>
    public override string ToString() =>
        $"Content {Id}";
    #endregion
}