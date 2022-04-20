using Guilded.Base.Users;
using Newtonsoft.Json;

namespace Guilded.Base.Events;

/// <summary>
/// Represents an event with the opcode <c>1</c> that is received once WebSocket connects or reconnects.
/// </summary>
/// <remarks>
/// <para><see cref="WelcomeEvent"/> can be used to ensure that WebSocket has connected to Guilded or that the events from Guilded are being received.</para>
/// </remarks>
/// <seealso cref="ResumeEvent"/>
/// <seealso cref="GuildedWebsocketException"/>
public class WelcomeEvent : BaseObject
{
    #region JSON properties
    /// <summary>
    /// The duration between heartbeats.
    /// </summary>
    /// <remarks>
    /// <para>The duration between each heartbeat in milliseconds. The value is usually <c>22500</c>.</para>
    /// </remarks>
    /// <value>Milliseconds</value>
    public int HeartbeatInterval { get; }
    /// <summary>
    /// The identifier of the last event sent.
    /// </summary>
    /// <remarks>
    /// <para>The identifier of the last message that was received before this event.</para>
    /// </remarks>
    /// <value>Event ID?</value>
    public string? LastMessageId { get; }
    /// <summary>
    /// The current logged in user.
    /// </summary>
    /// <value>Me</value>
    public Me User { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="WelcomeEvent"/> from the specified JSON properties.
    /// </summary>
    /// <param name="heartbeatIntervalMs">The duration between heartbeats</param>
    /// <param name="user">The current logged in user</param>
    /// <param name="lastMessageId">The identifier of the last event sent</param>
    [JsonConstructor]
    public WelcomeEvent(
        [JsonProperty(Required = Required.Always)]
        int heartbeatIntervalMs,

        [JsonProperty(Required = Required.Always)]
        Me user,

        string? lastMessageId
    ) =>
        (HeartbeatInterval, User, LastMessageId) = (heartbeatIntervalMs, user, lastMessageId);
    #endregion
}