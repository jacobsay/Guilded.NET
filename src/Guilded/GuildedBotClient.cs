using System;
using System.Threading.Tasks;
using Guilded.Abstract;
using Guilded.Base.Client;
using RestSharp;

namespace Guilded;

/// <summary>
/// Represents the client type for Guilded bots.
/// </summary>
/// <remarks>
/// <para>Use this to initiate and log into Guilded bot.</para>
/// <para>If you want to connect, set <see cref="AuthToken" /> and then use <see cref="ConnectAsync()" />.</para>
/// <para>You can also use <see cref="ConnectAsync(string)" />, which doesn't require <see cref="AuthToken" /> set.</para>
/// </remarks>
/// <example>
/// <para>This showcases a Guilded bot client that connects to Guilded, listens for prepared event and only listens to messages:</para>
/// <code language="csharp">
/// using GuildedBotClient client = new GuildedBotClient("...auth...");
/// client.Prepared += _ => Console.WriteLine("I am prepared!");
/// client.MessageCreated.Subscribe(msg => Console.WriteLine("Received message with content:\n{0}", msg.Content));
/// await client.ConnectAsync();
/// </code>
/// <para>An example of a Guilded bot client with <c>!ping</c> command</para>
/// <code language="csharp">
/// await using var client = new GuildedBotClient("...auth...");
///
/// client.Prepared
///       .Subscribe(me =>
///           Console.WriteLine("I am prepared! Logged in as '{0}'", me.Name)
///       );
/// client.MessageCreated
///     .Where(msg => msg.Content == "!ping")
///     .Subscribe(msg => await msg.RespondAsync("Pong!"));
///
/// await client.ConnectAsync();
/// </code>
/// </example>
/// <seealso cref="AbstractGuildedClient" />
/// <seealso cref="BaseGuildedClient" />
/// <seealso cref="AbstractGuildedClient.Prepared" />
/// <seealso cref="BaseGuildedClient.Connected" />
/// <seealso cref="ConnectAsync()" />
/// <seealso cref="ConnectAsync(string)" />
/// <seealso cref="AbstractGuildedClient.MessageCreated" />
/// <seealso cref="AbstractGuildedClient.MessageUpdated" />
public class GuildedBotClient : AbstractGuildedClient
{
    #region Properties
    /// <summary>
    /// An authentication token used to log into a bot in Guilded.
    /// </summary>
    /// <remarks>
    /// <para>An authentication token that will be used to connect to Guilded using <see cref="ConnectAsync()" /> method.</para>
    /// <para>This token can be set through <see cref="GuildedBotClient(string)" /> constructor. This is optional and <see cref="ConnectAsync(string)" /> can be used instead with authentication token as an argument.</para>
    /// <para><see cref="AuthToken" /> will be passed as a header <c>Authorization</c> with bearer prefix.</para>
    /// </remarks>
    /// <value>Authentication token</value>
    /// <seealso cref="ConnectAsync()" />
    /// <seealso cref="ConnectAsync(string)" />
    /// <seealso cref="GuildedBotClient(string)" />
    protected string? AuthToken { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new <see cref="GuildedBotClient" /> instance without authentication token.
    /// </summary>
    /// <remarks>
    /// <para>This creates a new client and only initiates it. It does not connect to Guilded.</para>
    /// <para>If you want to connect to Guilded, use <see cref="ConnectAsync(string)" /> with bot's authentication token.</para>
    /// </remarks>
    /// <returns>New empty <see cref="GuildedBotClient" /> instance</returns>
    /// <seealso cref="GuildedBotClient" />
    /// <seealso cref="GuildedBotClient(string)" />
    /// <seealso cref="AuthToken" />
    public GuildedBotClient() { }

    /// <summary>
    /// Creates a new <see cref="GuildedBotClient" /> instance with given <paramref name="auth" />.
    /// </summary>
    /// <remarks>
    /// <para>This creates a new client and only initiates it. It does not connect to Guilded.</para>
    /// <para>If you want to connect, set <see cref="AuthToken" /> and then use <see cref="ConnectAsync()" />.</para>
    /// <para>You can also use <see cref="ConnectAsync(string)" />, which doesn't require <see cref="AuthToken" /> set.</para>
    /// </remarks>
    /// <param name="auth">Authentication token used to log into the bot in Guilded</param>
    /// <exception cref="ArgumentNullException">When passed argument <paramref name="auth" /> is <see langword="null" />, empty or whitespace</exception>
    /// <returns>New authorizable <see cref="GuildedBotClient" /> instance</returns>
    /// <seealso cref="GuildedBotClient" />
    /// <seealso cref="GuildedBotClient()" />
    /// <seealso cref="AuthToken" />
    public GuildedBotClient(string auth)
    {
        // Make sure correct token is passed
        if (string.IsNullOrWhiteSpace(auth))
            throw new ArgumentNullException(nameof(auth));

        AuthToken = auth;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Connects to Guilded bot using parameter as an auth.
    /// </summary>
    /// <remarks>
    /// <para>Creates a new connection to Guilded using argument <paramref name="auth" />. This does not use <see cref="AuthToken" />.</para>
    /// <para>To disconnect from Guilded, use <see cref="BaseGuildedClient.DisconnectAsync" /></para>
    /// </remarks>
    /// <param name="auth">The token to be used for authorization</param>
    /// <exception cref="ArgumentNullException">When passed argument <paramref name="auth" /> is <see langword="null" />, empty or whitespace</exception>
    /// <seealso cref="ConnectAsync()" />
    /// <seealso cref="GuildedBotClient()" />
    public async Task ConnectAsync(string auth)
    {
        if (string.IsNullOrWhiteSpace(auth))
            throw new ArgumentNullException(nameof(auth));
        // Give authentication token to Guilded
        AdditionalHeaders.Add("Authorization", $"Bearer {auth}");
        // Add all headers to the clients
        Rest.AddDefaultHeaders(AdditionalHeaders);

        await base.ConnectAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Connects to Guilded using defined auth.
    /// </summary>
    /// <remarks>
    /// <para>Creates a new connection to Guilded using set property <see cref="AuthToken" /> as an authentication token.</para>
    /// </remarks>
    /// <seealso cref="ConnectAsync(string)" />
    /// <seealso cref="GuildedBotClient(string)" />
    public override Task ConnectAsync() =>
        ConnectAsync(AuthToken ?? throw new NullReferenceException("Expected AuthToken to have a value"));
    #endregion
}