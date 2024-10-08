using System;

using Newtonsoft.Json;

namespace Guilded.Base.Embeds;

/// <summary>
/// Represents an author of the content represented in an <see cref="Embed">embed</see>.
/// </summary>
/// <example>
/// <para>An example of using <see cref="EmbedAuthor" /> to display content owner:</para>
/// <code language="csharp">
/// // ... Getting information about a new post...
/// EmbedAuthor author = new EmbedAuthor(post.Author.Username, post.Author.Avatar, post.Url);
/// Embed embed = new Embed
/// {
///     Author = author,
///     Description = post.Text.Length > 4000
///                 ? post.Text.Substring(0, 3997) + "..."
///                 : post.Text
/// };
/// await client.CreateMessageAsync(channelId, embed);
/// </code>
/// </example>
/// <seealso cref="Embed" />
/// <seealso cref="EmbedFooter" />
/// <seealso cref="EmbedField" />
/// <seealso cref="EmbedMedia" />
public class EmbedAuthor
{
    #region Properties
    /// <summary>
    /// Gets the name of an embed author.
    /// </summary>
    /// <remarks>
    /// <para>The provided Markdown is ignored.</para>
    /// </remarks>
    /// <value>Name</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets the URL that author links.
    /// </summary>
    /// <remarks>
    /// <para>Can be used to open up author's profile or link to the content of the embed.</para>
    /// </remarks>
    /// <value>URL?</value>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Uri? Url { get; set; }

    /// <summary>
    /// Gets the URL to author's icon.
    /// </summary>
    /// <remarks>
    /// <para>Used to display the icon of the content's author.</para>
    /// </remarks>
    /// <value>URL?</value>
    [JsonProperty("icon_url", NullValueHandling = NullValueHandling.Ignore)]
    public Uri? IconUrl { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="EmbedAuthor" /> without an icon and without a URL.
    /// </summary>
    /// <param name="name">The name of the embed author</param>
    public EmbedAuthor(string name) =>
        Name = name;

    /// <inheritdoc cref="EmbedAuthor(string)" />
    public EmbedAuthor(object? name) : this(name?.ToString() ?? string.Empty) { }

    /// <summary>
    /// Initializes a new instance of <see cref="EmbedAuthor" /> with an optional <paramref name="url" />.
    /// </summary>
    /// <param name="name">The name of the embed author</param>
    /// <param name="url">The URL that author links</param>
    /// <param name="icon">The URL to author's icon</param>
    /// <returns>New <see cref="EmbedAuthor" /> instance</returns>
    /// <seealso cref="EmbedAuthor" />
    /// <seealso cref="EmbedAuthor(string, string, string)" />
    [JsonConstructor]
    public EmbedAuthor(
        [JsonProperty(Required = Required.Always)]
        string name,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        Uri? url = null,

        [JsonProperty("icon_url", NullValueHandling = NullValueHandling.Ignore)]
        Uri? icon = null
    ) : this(name) =>
        (IconUrl, Url) = (icon, url);

    /// <inheritdoc cref="EmbedAuthor(string, Uri?, Uri?)" />
    /// <exception cref="UriFormatException"><paramref name="url" /> or <paramref name="icon" /> have bad <see cref="Uri" /> formatting</exception>
    /// <seealso cref="EmbedAuthor" />
    /// <seealso cref="EmbedAuthor(string, Uri, Uri)" />
    public EmbedAuthor(string name, string? url = null, string? icon = null) : this(name, url is null ? null : new Uri(url), icon is null ? null : new Uri(icon)) { }

    /// <inheritdoc cref="EmbedAuthor(string, Uri?, Uri?)" />
    /// <seealso cref="EmbedAuthor" />
    /// <seealso cref="EmbedAuthor(string, Uri, Uri)" />
    public EmbedAuthor(object? name, Uri? url = null, Uri? icon = null) : this(name?.ToString() ?? string.Empty, url, icon) { }

    /// <inheritdoc cref="EmbedAuthor(string, Uri?, Uri?)" />
    /// <exception cref="UriFormatException"><paramref name="url" /> or <paramref name="icon" /> have bad <see cref="Uri" /> formatting</exception>
    /// <seealso cref="EmbedAuthor" />
    /// <seealso cref="EmbedAuthor(string, Uri, Uri)" />
    public EmbedAuthor(object? name, string? url = null, string? icon = null) : this(name?.ToString() ?? string.Empty, url, icon) { }
    #endregion
}