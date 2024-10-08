using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guilded.Base;
using Guilded.Base.Client;
using Guilded.Base.Content;
using Guilded.Base.Embeds;
using Guilded.Base.Servers;
using Guilded.Base.Users;
using RestSharp;

namespace Guilded.Webhook;

/// <summary>
/// Represents the <see cref="GuildedWebhookClient">client</see> for <see cref="Base.Servers.Webhook">webhook</see> execution.
/// </summary>
/// <remarks>
/// <para>This does not require to be connected. You can use it on a go.</para>
/// </remarks>
/// <example>
/// <para>Here's an example of a <see cref="GuildedWebhookClient">webhook client</see> in action:</para>
/// <code language="csharp">
/// await using var webhookClient = new GuildedWebhookClient("...url here...", "... another webhook's url here...", ...);
///
/// await webhookClient.CreateMessageAsync("Everyone, we have an announcement to make: Stop bullying!");
/// </code>
/// </example>
/// <seealso cref="T:Guilded.GuildedBotClient" />
/// <seealso cref="T:Guilded.AbstractGuildedClient" />
/// <seealso cref="BaseGuildedClient" />
/// <seealso cref="BaseGuildedService" />
public class GuildedWebhookClient : BaseGuildedService
{
    /// <summary>
    /// Gets the list of all <see cref="Base.Servers.Webhook">webhooks</see> this <see cref="GuildedWebhookClient">client</see> will execute.
    /// </summary>
    /// <value>List of <see cref="Base.Servers.Webhook">webhooks</see></value>
    public IList<IWebhook> Webhooks { get; }

    #region Constructors
    /// <summary>
    /// Creates a new <see cref="GuildedWebhookClient" /> instance from given <paramref name="webhooks" />.
    /// </summary>
    /// <remarks>
    /// <para>To execute the webhooks, you can use <see cref="CreateMessageAsync(MessageContent)" /> method.</para>
    /// </remarks>
    /// <param name="webhooks">The list of <see cref="Base.Servers.Webhook">webhooks</see> that will be executed</param>
    /// <returns>New <see cref="GuildedWebhookClient" /> instance</returns>
    /// <seealso cref="GuildedWebhookClient" />
    /// <seealso cref="GuildedWebhookClient(IList{IWebhook})" />
    /// <seealso cref="GuildedWebhookClient(IWebhook[])" />
    /// <seealso cref="GuildedWebhookClient(Uri[])" />
    /// <seealso cref="GuildedWebhookClient(string[])" />
    /// <seealso cref="GuildedWebhookClient(Guid, string)" />
    public GuildedWebhookClient(IList<IWebhook> webhooks) : base(new Uri(GuildedUrl.Media, "webhooks")) =>
        Webhooks = webhooks;

    /// <inheritdoc cref="GuildedWebhookClient(IList{IWebhook})" />
    public GuildedWebhookClient(params IWebhook[] webhooks) : this((IList<IWebhook>)webhooks) { }

    /// <inheritdoc cref="GuildedWebhookClient(IList{IWebhook})" />
    public GuildedWebhookClient(params Uri[] webhookUrls) : this(webhookUrls.Select(x => new WebhookSkeleton(x)).ToList<IWebhook>()) { }

    /// <inheritdoc cref="GuildedWebhookClient(IList{IWebhook})" />
    public GuildedWebhookClient(params string[] webhookUrls) : this(webhookUrls.Select(x => new WebhookSkeleton(x)).ToList<IWebhook>()) { }

    /// <inheritdoc cref="GuildedWebhookClient(IList{IWebhook})" />
    public GuildedWebhookClient(Guid webhook, string token) : this(new WebhookSkeleton(webhook, token)) { }
    #endregion

    #region Methods
    /// <summary>
    /// Creates a <see cref="Message">message</see> for the specified <see cref="Base.Servers.Webhook">webhook</see>.
    /// </summary>
    /// <param name="webhook">The <see cref="Base.Servers.Webhook.Url">url</see> of the <see cref="Base.Servers.Webhook">webhook</see> to execute</param>
    /// <param name="message">The <see cref="Message">message</see> to send</param>
    /// <exception cref="GuildedException" />
    /// <exception cref="GuildedRequestException" />
    /// <exception cref="GuildedResourceException" />
    public Task CreateMessageAsync(Uri webhook, MessageContent message) =>
        ExecuteRequestAsync(new RestRequest(webhook, Method.Post).AddBody(message));

    /// <inheritdoc cref="CreateMessageAsync(Uri, MessageContent)" />
    /// <param name="webhook">The <see cref="Base.Servers.Webhook.Url">url</see> of the <see cref="Base.Servers.Webhook">webhook</see> to execute</param>
    /// <param name="content">The <see cref="Message.Content">text contents</see> of the <see cref="Message">message</see> in Markdown</param>
    /// <param name="embeds">The list of all <see cref="Embed">custom embeds</see> in the <see cref="Message">message</see> (max — <c>1</c>)</param>
    /// <param name="username">The displayed <see cref="UserSummary.Name">name</see> of the webhook</param>
    /// <param name="avatar">The displayed <see cref="UserSummary.Avatar">profile picture</see> of the webhook</param>
    public Task CreateMessageAsync(Uri webhook, string? content = null, IList<Embed>? embeds = null, string? username = null, Uri? avatar = null) =>
        CreateMessageAsync(webhook, new MessageContent(content, embeds, username: username, avatar: avatar));

    /// <inheritdoc cref="CreateMessageAsync(Uri, MessageContent)" />
    /// <param name="webhook">The <see cref="Base.Servers.Webhook.Url">url</see> of the <see cref="Base.Servers.Webhook">webhook</see> to execute</param>
    /// <param name="content">The <see cref="Message.Content">text contents</see> of the <see cref="Message">message</see> in Markdown</param>
    /// <param name="username">The displayed <see cref="UserSummary.Name">name</see> of the webhook</param>
    /// <param name="avatar">The displayed <see cref="UserSummary.Avatar">profile picture</see> of the webhook</param>
    /// <param name="embeds">The array of all <see cref="Embed">custom embeds</see> in the <see cref="Message">message</see> (max — <c>1</c>)</param>
    public Task CreateMessageAsync(Uri webhook, string? content = null, string? username = null, Uri? avatar = null, params Embed[] embeds) =>
        CreateMessageAsync(webhook, new MessageContent(content, embeds, username: username, avatar: avatar));

    /// <inheritdoc cref="CreateMessageAsync(Uri, MessageContent)" />
    /// <param name="webhook">The <see cref="Base.Servers.Webhook.Url">url</see> of the <see cref="Base.Servers.Webhook">webhook</see> to execute</param>
    /// <param name="content">The <see cref="Message.Content">text contents</see> of the <see cref="Message">message</see> in Markdown</param>
    /// <param name="embeds">The array of all <see cref="Embed">custom embeds</see> in the <see cref="Message">message</see> (max — <c>1</c>)</param>
    public Task CreateMessageAsync(Uri webhook, string content, params Embed[] embeds) =>
        CreateMessageAsync(webhook, new MessageContent(content, embeds));

    /// <inheritdoc cref="CreateMessageAsync(Uri, MessageContent)" />
    /// <param name="webhook">The <see cref="Base.Servers.Webhook.Url">url</see> of the <see cref="Base.Servers.Webhook">webhook</see> to execute</param>
    /// <param name="embeds">The array of all <see cref="Embed">custom embeds</see> in the <see cref="Message">message</see> (max — <c>1</c>)</param>
    public Task CreateMessageAsync(Uri webhook, params Embed[] embeds) =>
        CreateMessageAsync(webhook, new MessageContent(embeds));

    /// <summary>
    /// Creates a <see cref="Message">message</see> for every <see cref="Webhooks">webhook</see> in the <see cref="GuildedWebhookClient">client</see>.
    /// </summary>
    /// <param name="message">The <see cref="Message">message</see> to send</param>
    /// <exception cref="GuildedException" />
    /// <exception cref="GuildedRequestException" />
    /// <exception cref="GuildedResourceException" />
    public async Task CreateMessageAsync(MessageContent message)
    {
        foreach (IWebhook webhook in Webhooks)
            await ExecuteRequestAsync(new RestRequest(webhook.Url, Method.Post).AddBody(message)).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a <see cref="Message">message</see> for every <see cref="Webhooks">webhook</see> in the <see cref="GuildedWebhookClient">client</see>.
    /// </summary>
    /// <remarks>
    /// <para>The text <paramref name="content" /> will be formatted in Markdown.</para>
    /// </remarks>
    /// <param name="content">The <see cref="Message.Content">text contents</see> of the <see cref="Message">message</see> in Markdown</param>
    /// <param name="embeds">The list of all <see cref="Embed">custom embeds</see> in the <see cref="Message">message</see> (max — <c>1</c>)</param>
    /// <param name="username">The displayed <see cref="UserSummary.Name">name</see> of the webhook</param>
    /// <param name="avatar">The displayed <see cref="UserSummary.Avatar">profile picture</see> of the webhook</param>
    /// <exception cref="GuildedException" />
    /// <exception cref="GuildedRequestException" />
    /// <exception cref="GuildedResourceException" />
    public Task CreateMessageAsync(string? content = null, IList<Embed>? embeds = null, string? username = null, Uri? avatar = null) =>
        CreateMessageAsync(new MessageContent(content, embeds, null, username: username, avatar: avatar));

    /// <inheritdoc cref="CreateMessageAsync(string, IList{Embed}, string, Uri)" />
    /// <param name="content">The <see cref="Message.Content">text contents</see> of the <see cref="Message">message</see> in Markdown</param>
    /// <param name="username">The displayed <see cref="UserSummary.Name">name</see> of the webhook</param>
    /// <param name="avatar">The displayed <see cref="UserSummary.Avatar">profile picture</see> of the webhook</param>
    /// <param name="embeds">The array of all <see cref="Embed">custom embeds</see> in the <see cref="Message">message</see> (max — <c>1</c>)</param>
    public Task CreateMessageAsync(string? content = null, string? username = null, Uri? avatar = null, params Embed[] embeds) =>
        CreateMessageAsync(new MessageContent(content, embeds, null, username: username, avatar: avatar));

    /// <inheritdoc cref="CreateMessageAsync(string, IList{Embed}, string, Uri)" />
    /// <param name="content">The <see cref="Message.Content">text contents</see> of the <see cref="Message">message</see> in Markdown</param>
    /// <param name="embeds">The array of all <see cref="Embed">custom embeds</see> in the <see cref="Message">message</see> (max — <c>1</c>)</param>
    public Task CreateMessageAsync(string content, params Embed[] embeds) =>
        CreateMessageAsync(new MessageContent(content, embeds));

    /// <inheritdoc cref="CreateMessageAsync(string, IList{Embed}, string, Uri)" />
    /// <param name="embeds">The array of all <see cref="Embed">custom embeds</see> in the <see cref="Message">message</see> (max — <c>1</c>)</param>
    public Task CreateMessageAsync(params Embed[] embeds) =>
        CreateMessageAsync(new MessageContent(embeds));
    #endregion
}