using System;
using System.Collections.Generic;
using System.Linq;
using Guilded.Base.Content;
using Guilded.Base.Json;
using Newtonsoft.Json;

using SystemColor = System.Drawing.Color;

namespace Guilded.Base.Embeds;

/// <summary>
/// Represents a custom content embed that includes any kind of information.
/// </summary>
/// <remarks>
/// <para>This is usually used in Webhooks to provide an information about a new post or any event that occurred. It can also be used for displaying results from a bot or used as a content instead of plain Markdown. Embeds may look something like quote blocks, but with more content like fields, footers, etc.</para>
/// </remarks>
/// <example>
/// <para>Embeds can be created using constructors, object initializers and fluent interface methods.</para>
/// <para>This example showcases mixing object initializer along with constructor parameters:</para>
/// <code language="csharp">
/// Embed embed = new Embed(thumbnail: imageUrl)
/// {
///     Title = "Title here",
///     Description = "Description here"
/// };
/// </code>
/// <para>The example below showcases using fluent interface methods:</para>
/// <code language="csharp">
/// Embed embed = new Embed()
///     .SetTitle("Title here")
///     .SetDescription("Description here")
///     .SetFooter("Footer text here");
/// </code>
/// </example>
/// <seealso cref="Message" />
/// <seealso cref="Topic" />
/// <seealso cref="ListItem" />
/// <seealso cref="Doc" />
[JsonObject(MissingMemberHandling = MissingMemberHandling.Ignore, ItemNullValueHandling = NullValueHandling.Ignore)]
public class Embed
{
    #region Constants
    /// <summary>
    /// The count of how many fields there can be in <see cref="Embed" />.
    /// </summary>
    /// <value>Limit</value>
    /// <seealso cref="Embed" />
    /// <seealso cref="EmbedField" />
    /// <seealso cref="Fields" />
    public const int FieldLimit = 25;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the title of the <see cref="Embed">embed</see>.
    /// </summary>
    /// <remarks>
    /// <para>The contents are formatted in Markdown.</para>
    /// </remarks>
    /// <value>Markdown string?</value>
    /// <seealso cref="Embed" />
    /// <seealso cref="Url" />
    /// <seealso cref="Description" />
    public string? Title { get; set; }

    /// <summary>
    /// Gets the URL of the content that <see cref="Embed">embed</see> displays.
    /// </summary>
    /// <value>URL?</value>
    /// <seealso cref="Embed" />
    /// <seealso cref="Title" />
    /// <seealso cref="Description" />
    public Uri? Url { get; set; }

    /// <summary>
    /// The description text of the <see cref="Embed">embed</see>.
    /// </summary>
    /// <remarks>
    /// <para>The contents are formatted in Markdown.</para>
    /// </remarks>
    /// <value>Markdown string?</value>
    /// <seealso cref="Embed" />
    /// <seealso cref="Title" />
    /// <seealso cref="Url" />
    public string? Description { get; set; }

    /// <summary>
    /// Gets the author of the content that <see cref="Embed">embed</see> displays.
    /// </summary>
    /// <value>Embed Author?</value>
    /// <seealso cref="Embed" />
    /// <seealso cref="Footer" />
    /// <seealso cref="Timestamp" />
    public EmbedAuthor? Author { get; set; }

    /// <summary>
    /// Gets the colour of the <see cref="Embed">embed</see>.
    /// </summary>
    /// <remarks>
    /// <para>This is displayed as left-side border in the official Guilded client, but may be displayed differently in other clients.</para>
    /// </remarks>
    /// <value>Colour?</value>
    /// <seealso cref="Embed" />
    /// <seealso cref="Timestamp" />
    /// <seealso cref="Description" />
    [JsonConverter(typeof(DecimalColorConverter))]
    public SystemColor? Color { get; set; }

    /// <summary>
    /// Gets the thumbnail image of the <see cref="Embed">embed</see>.
    /// </summary>
    /// <remarks>
    /// <para>This is displayed as image at the right of <see cref="Embed">an embed</see> and as square in the official Guilded app.</para>
    /// </remarks>
    /// <value>Embed media?</value>
    /// <seealso cref="Embed" />
    /// <seealso cref="Image" />
    /// <seealso cref="Fields" />
    public EmbedMedia? Thumbnail { get; set; }

    /// <summary>
    /// Gets the image of the content that <see cref="Embed">embed</see> displays.
    /// </summary>
    /// <remarks>
    /// <para>This appears at the bottom of <see cref="Embed">an embed</see> and above a footer in the official Guilded app.</para>
    /// </remarks>
    /// <value>Embed media?</value>
    /// <seealso cref="Embed" />
    /// <seealso cref="Thumbnail" />
    /// <seealso cref="Fields" />
    public EmbedMedia? Image { get; set; }

    /// <summary>
    /// Gets the list of fields in the <see cref="Embed">embed</see>.
    /// </summary>
    /// <remarks>
    /// <para>Fields can be both inline and blocks.</para>
    /// </remarks>
    /// <value>List of embed fields?</value>
    /// <seealso cref="Embed" />
    /// <seealso cref="Image" />
    /// <seealso cref="Thumbnail" />
    /// <seealso cref="FieldLimit" />
    public IList<EmbedField>? Fields { get; set; }

    /// <summary>
    /// The footer of <see cref="Embed">an embed</see>.
    /// </summary>
    /// <remarks>
    /// <para>The bottom area of an embed that provides further information about anything.</para>
    /// <para>Footers can also have timestamps, but that can be used by setting <see cref="Timestamp" /> property. Timestamps are not officially part of footers, but that's the most common way they are displayed by the clients and official Guilded app.</para>
    /// </remarks>
    /// <value>Embed Footer?</value>
    /// <seealso cref="Embed" />
    /// <seealso cref="Timestamp" />
    /// <seealso cref="Author" />
    public EmbedFooter? Footer { get; set; }

    /// <summary>
    /// Gets the timestamp of <see cref="Embed">an embed</see>.
    /// </summary>
    /// <remarks>
    /// <para>Usually displayed in the footer.</para>
    /// </remarks>
    /// <value>Date?</value>
    /// <seealso cref="Embed" />
    /// <seealso cref="Footer" />
    /// <seealso cref="Author" />
    /// <seealso cref="Color" />
    public DateTime? Timestamp { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new empty instance of <see cref="Embed" />.
    /// </summary>
    /// <returns>Empty <see cref="Embed" /> instance</returns>
    public Embed() { }

    /// <summary>
    /// Initializes a new instance of <see cref="Embed" /> with the given imagess.
    /// </summary>
    /// <param name="image">The image of <see cref="Embed">an embed</see></param>
    /// <param name="thumbnail">The thumbnail image of <see cref="Embed">an embed</see></param>
    public Embed(Uri? image = null, Uri? thumbnail = null) =>
        (Image, Thumbnail) = (EmbedMedia.CreateOrNull(image), EmbedMedia.CreateOrNull(thumbnail));

    /// <summary>
    /// Initializes a new instance of <see cref="Embed" /> with a <paramref name="url" />.
    /// </summary>
    /// <param name="url">The URL of the content that embed displays</param>
    public Embed(Uri url) =>
        Url = url;

    /// <summary>
    /// Initializes a new instance of <see cref="Embed" /> with colour <paramref name="color" />.
    /// </summary>
    /// <param name="color">The colour of <see cref="Embed">an embed</see></param>
    public Embed(SystemColor color) =>
        Color = color;

    /// <summary>
    /// Initializes a new instance of <see cref="Embed" /> with colour <paramref name="argb" />.
    /// </summary>
    /// <param name="argb">The colour of <see cref="Embed">an embed</see> in RGB format</param>
    public Embed(int argb) : this(SystemColor.FromArgb(argb)) { }

    /// <summary>
    /// Initializes a new instance of <see cref="Embed" /> with channels
    /// <paramref name="red" />, <paramref name="green" /> and <paramref name="blue" /> of <see cref="Color" /> property.
    /// </summary>
    /// <param name="red">The red channel of the <see cref="Color">colour</see></param>
    /// <param name="green">The green channel of <see cref="Color">colour</see></param>
    /// <param name="blue">The blue channel of <see cref="Color">colour</see></param>
    public Embed(int red, int green, int blue) : this(SystemColor.FromArgb(red, green, blue)) { }

    /// <summary>
    /// Initializes a new instance of <see cref="Embed" /> with a <paramref name="description" />.
    /// </summary>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    public Embed(string description) =>
        Description = description;

    /// <summary>
    /// Initializes a new instance of <see cref="Embed" /> with title <paramref name="title" />.
    /// </summary>
    /// <param name="title">The title of <see cref="Embed">an embed</see></param>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    public Embed(string title, string description) : this(description) =>
        Title = title;

    /// <summary>
    /// Initializes a new instance of <see cref="Embed" /> with a <paramref name="title" /> and a <paramref name="url" />.
    /// </summary>
    /// <param name="title">The title of <see cref="Embed">an embed</see></param>
    /// <param name="url">The URL of <see cref="Embed">an embed</see></param>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    public Embed(string title, Uri url, string description) : this(title, description) =>
        Url = url;

    /// <summary>
    /// Initializes a new instance of <see cref="Embed" /> with an <paramref name="image" />.
    /// </summary>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="image">The image of <see cref="Embed">an embed</see></param>
    public Embed(string description, EmbedMedia image) : this(description) =>
        Image = image;

    /// <inheritdoc cref="Embed(string, EmbedMedia)" />
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="image">The image of <see cref="Embed">an embed</see></param>
    public Embed(string description, Uri image) : this(description, new EmbedMedia(image)) { }

    /// <summary>
    /// Initializes a new instance of <see cref="Embed" /> with an <paramref name="image" /> and a <paramref name="title" />.
    /// </summary>
    /// <param name="title">The title of <see cref="Embed">an embed</see></param>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="image">The image of <see cref="Embed">an embed</see></param>
    public Embed(string title, string description, EmbedMedia image) : this(title, description) =>
        Image = image;

    /// <inheritdoc cref="Embed(string, string, EmbedMedia)" />
    /// <param name="title">The title of <see cref="Embed">an embed</see></param>
    /// <param name="url">The URL of <see cref="Embed">an embed</see></param>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="image">The image of <see cref="Embed">an embed</see></param>
    public Embed(string title, Uri url, string description, EmbedMedia image) : this(title, description, image) =>
        Url = url;

    /// <inheritdoc cref="Embed(string, string, EmbedMedia)" />
    /// <param name="title">The title of <see cref="Embed">an embed</see></param>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="image">The image of <see cref="Embed">an embed</see></param>
    public Embed(string title, string description, Uri image) : this(title, description, new EmbedMedia(image)) { }

    /// <inheritdoc cref="Embed(string, string, EmbedMedia)" />
    /// <param name="title">The title of <see cref="Embed">an embed</see></param>
    /// <param name="url">The URL of <see cref="Embed">an embed</see></param>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="image">The image of <see cref="Embed">an embed</see></param>
    public Embed(string title, Uri url, string description, Uri image) : this(title, url, description, new EmbedMedia(image)) { }

    /// <summary>
    /// Initializes a new instance of <see cref="Embed" /> with its <paramref name="fields" />.
    /// </summary>
    /// <param name="fields">The list of fields in this embed</param>
    public Embed(IList<EmbedField> fields) =>
        Fields = fields;

    /// <inheritdoc cref="Embed(IList{EmbedField})" />
    /// <param name="fields">The array of fields in this embed</param>
    public Embed(params EmbedField[] fields) : this(fields.ToList()) { }

    /// <inheritdoc cref="Embed(IList{EmbedField})" />
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="fields">The list of fields in this embed</param>
    public Embed(string description, IList<EmbedField> fields) : this(description) =>
        Fields = fields;

    /// <inheritdoc cref="Embed(IList{EmbedField})" />
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="fields">The array of fields in this embed</param>
    public Embed(string description, params EmbedField[] fields) : this(description, fields.ToList()) { }

    /// <summary>
    /// Initializes a new instance of <see cref="Embed" /> with its <paramref name="fields" /> and a <paramref name="title" />.
    /// </summary>
    /// <param name="title">The title of <see cref="Embed">an embed</see></param>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="fields">The list of fields in this embed</param>
    public Embed(string title, string description, IList<EmbedField> fields) : this(title, description) =>
        Fields = fields;

    /// <inheritdoc cref="Embed(string, string, IList{EmbedField})" />
    /// <param name="title">The title of <see cref="Embed">an embed</see></param>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="fields">The array of fields in this embed</param>
    public Embed(string title, string description, params EmbedField[] fields) : this(title, description, fields.ToList()) { }

    /// <summary>
    /// Initializes a new instance of <see cref="Embed" /> with a <paramref name="footer" /> and a <paramref name="title" />.
    /// </summary>
    /// <param name="title">The title of <see cref="Embed">an embed</see></param>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="footer">The footer of <see cref="Embed">an embed</see></param>
    public Embed(string title, string description, EmbedFooter footer) : this(title, description) =>
        Footer = footer;

    /// <inheritdoc cref="Embed(string, string, EmbedFooter)" />
    /// <param name="title">The title of <see cref="Embed">an embed</see></param>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="footer">The text of <see cref="Embed">an embed</see> footer</param>
    public Embed(string title, string description, string footer) : this(title, description, new EmbedFooter(footer)) { }

    /// <inheritdoc cref="Embed(string, string, EmbedFooter)" />
    /// <param name="title">The title of <see cref="Embed">an embed</see></param>
    /// <param name="url">The URL of <see cref="Embed">an embed</see></param>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="footer">The footer of <see cref="Embed">an embed</see></param>
    public Embed(string title, Uri url, string description, EmbedFooter footer) : this(title, description, footer) =>
        Url = url;

    /// <inheritdoc cref="Embed(string, string, EmbedFooter)" />
    /// <param name="title">The title of <see cref="Embed">an embed</see></param>
    /// <param name="url">The URL of <see cref="Embed">an embed</see></param>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="footer">The text of <see cref="Embed">an embed</see> footer</param>
    public Embed(string title, Uri url, string description, string footer) : this(title, url, description, new EmbedFooter(footer)) { }

    /// <inheritdoc cref="Embed(string, string, EmbedFooter)" />
    /// <param name="title">The title of <see cref="Embed">an embed</see></param>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="footer">The footer of <see cref="Embed">an embed</see></param>
    /// <param name="timestamp">The timestamp of <see cref="Embed">an embed</see> footer</param>
    public Embed(string title, string description, EmbedFooter footer, DateTime timestamp) : this(title, description, footer) =>
        Timestamp = timestamp;

    /// <inheritdoc cref="Embed(string, string, EmbedFooter)" />
    /// <param name="title">The title of <see cref="Embed">an embed</see></param>
    /// <param name="url">The URL of <see cref="Embed">an embed</see></param>
    /// <param name="description">The description text of <see cref="Embed">an embed</see></param>
    /// <param name="footer">The footer of <see cref="Embed">an embed</see></param>
    /// <param name="timestamp">The timestamp of <see cref="Embed">an embed</see> footer</param>
    public Embed(string title, Uri url, string description, EmbedFooter footer, DateTime timestamp) : this(title, description, footer, timestamp) =>
        Url = url;
    #endregion

    #region Methods

    #region Methods Title
    /// <summary>
    /// Sets the <see cref="Title">title</see> as the given <paramref name="value" />.
    /// </summary>
    /// <param name="value">The value of the <see cref="Embed">embed's</see> title</param>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetTitle(string value)
    {
        Title = value;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Url">url</see> as the given <paramref name="value" />.
    /// </summary>
    /// <param name="value">The value of the <see cref="Embed">embed's</see> url</param>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetUrl(Uri value)
    {
        Url = value;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Url">url</see> as the given <paramref name="value" />.
    /// </summary>
    /// <remarks>
    /// <para>The given <paramref name="value" /> will be converted to <see cref="Uri" />.</para>
    /// </remarks>
    /// <param name="value">The value of the <see cref="Embed">embed's</see> url</param>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />, empty or whitespace</exception>
    /// <exception cref="UriFormatException"><paramref name="value" /> has bad <see cref="Uri" /> formatting</exception>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetUrl(string value) =>
        SetUrl(new Uri(value));
    #endregion

    #region Method SetDescription
    /// <summary>
    /// Sets the <see cref="Description">description</see> as the given <paramref name="value" />.
    /// </summary>
    /// <param name="value">The text contents of the <see cref="Embed">embed's</see> description</param>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />, empty or whitespace</exception>
    /// <exception cref="OverflowException"><paramref name="value" /> exceeds 4000 character limit</exception>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetDescription(string value)
    {
        if (value?.Length > 4000)
            throw new OverflowException($"Argument {nameof(value)} cannot be more than 4'000 characters.");
        Description = value;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Description">description</see> as the given <paramref name="value" />.
    /// </summary>
    /// <remarks>
    /// <para>The given <paramref name="value" /> will be converted to string.</para>
    /// </remarks>
    /// <param name="value">The contents of the <see cref="Embed">embed's</see> description</param>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetDescription(object? value) =>
        SetDescription(value?.ToString() ?? string.Empty);
    #endregion

    #region Method SetAuthor
    /// <summary>
    /// Sets the <see cref="Author">author</see> as the given <paramref name="value" />.
    /// </summary>
    /// <param name="value">The author of the content that <see cref="Embed">embed</see> represents</param>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetAuthor(EmbedAuthor value)
    {
        Author = value;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Author">author</see> as the given author with the given <paramref name="name" />.
    /// </summary>
    /// <remarks>
    /// <para>A given <paramref name="name" /> will be converted to <see cref="EmbedAuthor" />.</para>
    /// </remarks>
    /// <param name="name">The name of the <see cref="Embed">embed</see>'s author</param>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetAuthor(string name) =>
        SetAuthor(new EmbedAuthor(name));

    /// <summary>
    /// Sets the <see cref="Author">author</see> as the given author with the given <paramref name="name" />.
    /// </summary>
    /// <remarks>
    /// <para>The given <paramref name="name" />, <paramref name="icon" /> and <paramref name="url" /> will be converted to <see cref="EmbedAuthor" />.</para>
    /// </remarks>
    /// <param name="name">The name of the <see cref="Embed">embed</see>'s author</param>
    /// <param name="url">The URL of the <see cref="Embed">embed</see>'s author</param>
    /// <param name="icon">The URL to icon of the <see cref="Embed">embed</see>'s author</param>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetAuthor(string name, Uri? url = null, Uri? icon = null) =>
        SetAuthor(new EmbedAuthor(name, url, icon));

    /// <summary>
    /// Sets the <see cref="Author">author</see> as the given author with the given <paramref name="name" />.
    /// </summary>
    /// <remarks>
    /// <para>The given <paramref name="name" />, <paramref name="icon" /> and <paramref name="url" /> will be converted to <see cref="EmbedAuthor" />.</para>
    /// </remarks>
    /// <param name="name">The name of the <see cref="Embed">embed</see>'s author</param>
    /// <param name="url">The URL of the <see cref="Embed">embed</see>'s author</param>
    /// <param name="icon">The URL to icon of the <see cref="Embed">embed</see>'s author</param>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetAuthor(string name, string? url = null, string? icon = null) =>
        SetAuthor(new EmbedAuthor(name, url, icon));
    #endregion

    #region Method AddField(s)
    /// <summary>
    /// Adds new <paramref name="fields" /> to the <see cref="Fields">current set of fields</see>.
    /// </summary>
    /// <param name="fields">The list of fields to add</param>
    /// <exception cref="OverflowException">When the combined field list exceeds max field limit of <c>25</c></exception>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed AddFields(IEnumerable<EmbedField> fields)
    {
        // Don't allow >25 fields
        if ((Fields?.Count + fields.Count()) > FieldLimit)
            throw new OverflowException("Cannot add more than 25 fields to an embed");
        else if (Fields is null)
            Fields = fields.ToList();
        else
            Fields = Fields.Concat(fields).ToList();
        return this;
    }

    /// <inheritdoc cref="AddFields(IEnumerable{EmbedField})" />
    public Embed AddFields(params EmbedField[] fields) =>
        AddFields((IList<EmbedField>)fields);

    /// <summary>
    /// Adds a new <paramref name="field" /> to the <see cref="Fields">current set of fields</see>.
    /// </summary>
    /// <param name="field">A new field to add</param>
    /// <exception cref="OverflowException">When the combined field list exceeds max field limit of <c>25</c></exception>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed AddField(EmbedField field) =>
        AddFields(field);

    /// <summary>
    /// Adds a new field to the <see cref="Fields">current set of fields</see>.
    /// </summary>
    /// <param name="name">The title text of the new field</param>
    /// <param name="value">The description text of the new field</param>
    /// <param name="inline">Whether the field should be in the same row with other fields</param>
    /// <exception cref="OverflowException">When the combined field list exceeds max field limit of <c>25</c></exception>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed AddField(string name, string value, bool inline = false) =>
        AddField(new EmbedField(name, value, inline));

    /// <inheritdoc cref="AddField(string, string, bool)" />
    /// <remarks>
    /// <para>The given <paramref name="value" /> will be converted to string</para>
    /// </remarks>
    public Embed AddField(string name, object? value, bool inline = false) =>
        AddField(name, value?.ToString() ?? string.Empty, inline);
    #endregion

    #region Methods Media
    /// <summary>
    /// Sets the <see cref="Image">image</see> as the given <paramref name="value" />.
    /// </summary>
    /// <param name="value">The value of the <see cref="Embed">embed's</see> image</param>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetImage(EmbedMedia value)
    {
        Image = value;
        return this;
    }

    /// <inheritdoc cref="SetImage(EmbedMedia)" />
    /// <remarks>
    /// <para>The given <paramref name="url" /> will be used to display an image.</para>
    /// </remarks>
    /// <param name="url">The URL to the <see cref="Embed">embed's</see> image</param>
    public Embed SetImage(Uri url) =>
        SetImage(new EmbedMedia(url));

    /// <inheritdoc cref="SetImage(Uri)" />
    public Embed SetImage(string url) =>
        SetImage(new Uri(url));

    /// <summary>
    /// Sets the <see cref="Thumbnail">thumbnail</see> as the given <paramref name="value" />.
    /// </summary>
    /// <param name="value">The value of the <see cref="Embed">embed's</see> thumbnail</param>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetThumbnail(EmbedMedia value)
    {
        Thumbnail = value;
        return this;
    }

    /// <inheritdoc cref="SetThumbnail(EmbedMedia)" />
    /// <remarks>
    /// <para>The given <paramref name="url" /> will be used to display a thumbnail.</para>
    /// </remarks>
    /// <param name="url">The URL to the <see cref="Embed">embed's</see> thumbnail</param>
    public Embed SetThumbnail(Uri url)
    {
        Thumbnail = new EmbedMedia(url);
        return this;
    }

    /// <inheritdoc cref="SetThumbnail(Uri)" />
    public Embed SetThumbnail(string url) =>
        SetThumbnail(new Uri(url));
    #endregion

    #region Method SetFooter
    /// <summary>
    /// Sets the <see cref="Footer">footer</see> as the given <paramref name="value" />.
    /// </summary>
    /// <param name="value">The footer's content of the <see cref="Embed">embed</see></param>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetFooter(EmbedFooter value)
    {
        Footer = value;
        return this;
    }

    /// <inheritdoc cref="SetFooter(EmbedFooter)" />
    /// <remarks>
    /// <para>A <see cref="EmbedFooter">footer</see> will be generated from the given <paramref name="text" />.</para>
    /// </remarks>
    /// <param name="text">The text contents of the footer</param>
    public Embed SetFooter(string text)
    {
        if (text is null)
            throw new ArgumentNullException(nameof(text));

        return SetFooter(new EmbedFooter(text));
    }

    /// <inheritdoc cref="SetFooter(EmbedFooter)" />
    /// <remarks>
    /// <para>A <see cref="EmbedFooter">footer</see> will be generated from the given <paramref name="text" /> and <paramref name="icon" />.</para>
    /// </remarks>
    /// <param name="text">The text contents of the footer</param>
    /// <param name="icon">URL to the icon's image that will be displayed in the left side of the footer</param>
    public Embed SetFooter(string text, Uri? icon = null) =>
        SetFooter(new EmbedFooter(text, icon));

    /// <inheritdoc cref="SetFooter(string, Uri?)" />
    public Embed SetFooter(string text, string? icon = null) =>
        SetFooter(text, icon is null ? null : new Uri(icon));

    // <inheritdoc cref="SetFooter(EmbedFooter)" />
    /// <remarks>
    /// <para>A <see cref="EmbedFooter">footer</see> will be generated from the given <paramref name="value" />.</para>
    /// <para><paramref name="value" /> parameter will be converted to string.</para>
    /// </remarks>
    /// <param name="value">The text contents of the footer</param>
    public Embed SetFooter(object? value) =>
        SetFooter(value?.ToString() ?? string.Empty);

    // <inheritdoc cref="SetFooter(EmbedFooter)" />
    /// <remarks>
    /// <para>A <see cref="EmbedFooter">footer</see> will be generated from the given <paramref name="value" /> and <paramref name="icon" />.</para>
    /// <para><paramref name="value" /> parameter will be converted to string.</para>
    /// </remarks>
    /// <param name="value">The text contents of the footer</param>
    /// <param name="icon">URL to the icon's image that will be displayed in the left side of the footer</param>
    public Embed SetFooter(object? value, Uri? icon = null) =>
        SetFooter(value?.ToString() ?? string.Empty, icon);

    /// <inheritdoc cref="SetFooter(object?, Uri?)" />
    public Embed SetFooter(object? text, string? icon = null) =>
        SetFooter(text, icon is null ? null : new Uri(icon));
    #endregion

    /// <summary>
    /// Sets the <see cref="Timestamp">timestamp</see> as the given <paramref name="value" />.
    /// </summary>
    /// <param name="value">The timestamp of the <see cref="Embed">embed</see></param>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetTimestamp(DateTime value)
    {
        Timestamp = value;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Timestamp">timestamp</see> as the <see cref="DateTime.UtcNow">current UTC time</see>.
    /// </summary>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetTimestamp() =>
        SetTimestamp(DateTime.UtcNow);

    #region Method SetColor
    /// <summary>
    /// Sets the <see cref="Color">left-side colour</see> as the given <paramref name="value" />.
    /// </summary>
    /// <param name="value">The left-side colour of the <see cref="Embed">embed</see></param>
    /// <returns>Current <see cref="Embed" /> instance</returns>
    public Embed SetColor(SystemColor value)
    {
        Color = value;
        return this;
    }

    /// <inheritdoc cref="SetColor(SystemColor)" />
    /// <remarks>
    /// <para>A new <see cref="Color" /> instance will be created from the given <paramref name="argb" /> value.</para>
    /// </remarks>
    /// <example>
    /// <code language="csharp">
    /// // Alpha channel is ignored
    /// embed.SetColor(0xFFFFFF);
    /// </code>
    /// </example>
    /// <param name="argb">The ARGB value of the <see cref="Color">embed's colour</see>.</param>
    public Embed SetColor(int argb) =>
        SetColor(SystemColor.FromArgb(argb));

    /// <inheritdoc cref="SetColor(SystemColor)" />
    /// <remarks>
    /// <para>A new <see cref="Color" /> instance will be created from the given <paramref name="red" /> channels, <paramref name="green" /> channels and <paramref name="blue" /> channel values.</para>
    /// </remarks>
    /// <example>
    /// <code language="csharp">
    /// embed.SetColor(0xFF, 0xFF, 0xFF);
    /// </code>
    /// </example>
    /// <param name="red">The red channel's strength of the <see cref="Color">colour</see>.</param>
    /// <param name="green">The green channel's strength of the <see cref="Color">colour</see>.</param>
    /// <param name="blue">The blue channel's strength of the <see cref="Color">colour</see>.</param>
    public Embed SetColor(int red, int green, int blue) =>
        SetColor(SystemColor.FromArgb(red, green, blue));
    #endregion

    #endregion
}