using Newtonsoft.Json;

namespace Guilded.NET.Base.Events
{
    /// <summary>
    /// An event that is received once WebSocket is initiated.
    /// </summary>
    /// <remarks>
    /// <para>This event is received once WebSocket (re)connects to Guilded.</para>
    /// <para><see cref="WelcomeEvent"/> can be used to ensure that WebSocket has
    /// connected to Guilded or that the events from Guilded are being received.</para>
    /// <para>This event has no name in API but has an event opcode of <c>1</c>.</para>
    /// </remarks>
    /// <seealso cref="ResumeEvent"/>
    /// <seealso cref="GuildedWebsocketException"/>
    public class WelcomeEvent : BaseObject
    {
        #region JSON properties
        /// <summary>
        /// The duration of time between each heartbeat.
        /// </summary>
        /// <value>Milliseconds</value>
        [JsonProperty("heartbeatIntervalMs", Required = Required.Always)]
        public int HeartbeatInterval
        {
            get; set;
        }
        /// <summary>
        /// The identifier of the last event sent.
        /// </summary>
        /// <value>Event ID?</value>
        public string LastMessageId
        {
            get; set;
        }
        #endregion
    }
}