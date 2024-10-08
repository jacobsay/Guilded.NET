using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Guilded.Base.Client;
using Guilded.Base.Embeds;
using Newtonsoft.Json;

namespace Guilded.Base;

/// <summary>
/// Represents an object that uses parent <see cref="BaseGuildedClient">Guilded client</see> for its methods.
/// </summary>
public interface IHasParentClient
{
    /// <summary>
    /// Gets <see cref="BaseGuildedClient">the parent client</see> that adopts <see cref="ContentModel">this object</see>.
    /// </summary>
    /// <value>Client</value>
    /// <seealso cref="ContentModel.OnDeserialized" />
    /// <seealso cref="BaseGuildedClient" />
    BaseGuildedClient ParentClient { get; }
}

/// <summary>
/// Represents a base for Guilded models that require a <see cref="ParentClient">client</see>.
/// </summary>
/// <remarks>
/// <para>This allows having methods like <see cref="Content.Message.CreateMessageAsync(string, IList{Embed}, IList{Guid}, bool, bool)" />, where it requires to call the parent client's methods.</para>
/// </remarks>
/// <seealso cref="BaseGuildedClient" />
public abstract class ContentModel : IHasParentClient
{
#nullable disable

    #region Properties
    /// <inheritdoc />
    [JsonIgnore]
    public BaseGuildedClient ParentClient { get; protected set; }
    #endregion

    #region Methods
    /// <summary>
    /// Adds a <see cref="ParentClient">parent client</see> if the context contains it.
    /// </summary>
    /// <param name="context">The context given by the serializer</param>
    /// <seealso cref="ParentClient" />
    /// <seealso cref="BaseGuildedClient" />
    [OnDeserialized]
    internal void OnDeserialized(StreamingContext context)
    {
        if (context.Context is BaseGuildedClient client)
            ParentClient = client;
    }
    #endregion

#nullable restore
}