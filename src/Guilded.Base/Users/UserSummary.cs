using System;
using System.Threading.Tasks;
using Guilded.Base.Client;
using Newtonsoft.Json;

namespace Guilded.Base.Users;

/// <summary>
/// Global minimal information about a user.
/// </summary>
/// <remarks>
/// <para>Defines a normal user with minimal information.</para>
/// </remarks>
/// <seealso cref="User" />
/// <seealso cref="SocialLink" />
public class UserSummary : ContentModel, IModelHasId<HashId>
{
    #region Properties
    /// <summary>
    /// Gets the identifier of <see cref="User">user</see>.
    /// </summary>
    /// <value><see cref="Id">User ID</see></value>
    /// <seealso cref="User" />
    /// <seealso cref="UserSummary" />
    /// <seealso cref="Type" />
    /// <seealso cref="Name" />
    public HashId Id { get; }

    /// <summary>
    /// Gets the type of <see cref="Users.User">the user</see> they are.
    /// </summary>
    /// <value>User type</value>
    /// <seealso cref="User" />
    /// <seealso cref="UserSummary" />
    /// <seealso cref="Id" />
    /// <seealso cref="Name" />
    public UserType Type { get; }

    /// <summary>
    /// Gets the global username of <see cref="Users.User">the user</see>.
    /// </summary>
    /// <value>Name</value>
    /// <seealso cref="User" />
    /// <seealso cref="UserSummary" />
    /// <seealso cref="Avatar" />
    /// <seealso cref="User.Banner" />
    public string Name { get; }

    /// <summary>
    /// Gets the global avatar of <see cref="Users.User">the user</see>.
    /// </summary>
    /// <value>Media URL</value>
    /// <seealso cref="User" />
    /// <seealso cref="UserSummary" />
    /// <seealso cref="User.Banner" />
    /// <seealso cref="Name" />
    public Uri? Avatar { get; }

    /// <summary>
    /// Gets whether <see cref="Users.User">the user</see> is a <see cref="UserType.Bot">bot</see>.
    /// </summary>
    /// <value>Is a bot</value>
    /// <seealso cref="User" />
    /// <seealso cref="UserSummary" />
    /// <seealso cref="Type" />
    /// <seealso cref="Id" />
    public bool IsBot => Type == UserType.Bot;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="UserSummary" /> with specified properties.
    /// </summary>
    /// <param name="id">The identifier of <see cref="User">user</see></param>
    /// <param name="type">The type of <see cref="Users.User">the user</see> they are</param>
    /// <param name="name">The global username of <see cref="Users.User">the user</see></param>
    /// <param name="avatar">The global avatar of <see cref="Users.User">the user</see></param>
    /// <returns>New <see cref="UserSummary" /> JSON instance</returns>
    /// <seealso cref="UserSummary" />
    [JsonConstructor]
    public UserSummary(
        [JsonProperty(Required = Required.Always)]
        HashId id,

        [JsonProperty(Required = Required.Always)]
        string name,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        Uri? avatar = null,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        UserType type = UserType.User
    ) =>
        (Id, Type, Name, Avatar) = (id, type, name, avatar);
    #endregion

    #region Methods
    /// <inheritdoc cref="BaseGuildedClient.GetSocialLinkAsync(HashId, HashId, SocialLinkType)" />
    public Task<SocialLink> GetSocialLinkAsync(HashId server, SocialLinkType linkType) =>
        ParentClient.GetSocialLinkAsync(server, Id, linkType);

    /// <inheritdoc cref="BaseGuildedClient.UpdateNicknameAsync(HashId, HashId, string)" />
    public Task<string> SetNicknameAsync(HashId server, string nickname) =>
        ParentClient.SetNicknameAsync(server, Id, nickname);

    /// <inheritdoc cref="BaseGuildedClient.DeleteMessageAsync(Guid, Guid)" />
    public Task RemoveNicknameAsync(HashId server) =>
        ParentClient.RemoveNicknameAsync(server, Id);

    /// <inheritdoc cref="BaseGuildedClient.AddMemberRoleAsync(HashId, HashId, uint)" />
    public Task AddMemberRoleAsync(HashId server, uint role) =>
        ParentClient.AddMemberRoleAsync(server, Id, role);

    /// <inheritdoc cref="BaseGuildedClient.RemoveMemberRoleAsync(HashId, HashId, uint)" />
    public Task RemoveMemberRoleAsync(HashId server, uint role) =>
        ParentClient.RemoveMemberRoleAsync(server, Id, role);

    /// <inheritdoc cref="BaseGuildedClient.AddXpAsync(HashId, HashId, short)" />
    public Task<long> AddXpAsync(HashId server, short amount) =>
        ParentClient.AddXpAsync(server, Id, amount);

    /// <inheritdoc cref="BaseGuildedClient.SetXpAsync(HashId, HashId, long)" />
    public Task<long> SetXpAsync(HashId server, long amount) =>
        ParentClient.SetXpAsync(server, Id, amount);

    /// <inheritdoc cref="BaseGuildedClient.RemoveMemberAsync(HashId, HashId)" />
    public Task RemoveMemberAsync(HashId server) =>
        ParentClient.RemoveMemberAsync(server, Id);

    /// <inheritdoc cref="BaseGuildedClient.AddMemberBanAsync(HashId, HashId, string?)" />
    public Task AddMemberBanAsync(HashId server, string? reason = null) =>
        ParentClient.AddMemberBanAsync(server, Id, reason);

    /// <inheritdoc cref="BaseGuildedClient.RemoveMemberBanAsync(HashId, HashId)" />
    public Task RemoveMemberBanAsync(HashId server) =>
        ParentClient.RemoveMemberBanAsync(server, Id);

    /// <inheritdoc cref="BaseGuildedClient.GetMemberBanAsync(HashId, HashId)" />
    public Task GetMemberBanAsync(HashId server) =>
        ParentClient.GetMemberBanAsync(server, Id);

    #region Methods Obsolete
    /// <inheritdoc cref="BaseGuildedClient.UpdateNicknameAsync(HashId, HashId, string)" />
    [Obsolete("Use `SetNicknameAsync` instead")]
    public Task<string> UpdateNicknameAsync(HashId server, string nickname) =>
        SetNicknameAsync(server, nickname);

    /// <inheritdoc cref="BaseGuildedClient.DeleteMessageAsync(Guid, Guid)" />
    [Obsolete("Use `RemoveNicknameAsync` instead")]
    public Task DeleteNicknameAsync(HashId server) =>
        RemoveNicknameAsync(server);

    /// <inheritdoc cref="BaseGuildedClient.AddMemberRoleAsync(HashId, HashId, uint)" />
    [Obsolete("Use `AddMemberRoleAsync` instead")]
    public Task AddRoleAsync(HashId server, uint role) =>
        AddMemberRoleAsync(server, role);

    /// <inheritdoc cref="BaseGuildedClient.RemoveMemberRoleAsync(HashId, HashId, uint)" />
    [Obsolete("Use `RemoveMemberRoleAsync` instead")]
    public Task RemoveRoleAsync(HashId server, uint role) =>
        RemoveMemberRoleAsync(server, role);

    /// <inheritdoc cref="BaseGuildedClient.GetMemberBanAsync(HashId, HashId)" />
    [Obsolete("Use `GetMemberBanAsync` instead")]
    public Task GetBanAsync(HashId server) =>
        ParentClient.GetMemberBanAsync(server, Id);
    #endregion

    #endregion

    #region Overrides
    /// <summary>
    /// Returns the string representation of this <see cref="UserSummary" /> instance.
    /// </summary>
    /// <remarks>
    /// <para>The mention syntax of the user will be returned.</para>
    /// </remarks>
    /// <example>
    /// <para>An example of a code with clearly defined identifier:</para>
    /// <code lang="csharp">
    /// UserSummary user = new(new HashId("R40Mp0Wd"), UserType.User, "Example");
    /// Console.WriteLine("Here's the user: {0}", user);
    /// </code>
    /// <para>The output of the code above:</para>
    /// <code lang="bash">
    /// Here's the user: &lt;@R40Mp0Wd&gt;
    /// </code>
    /// </example>
    /// <returns>Markdown user mention</returns>
    public override string ToString() =>
        $"<@{Id}>";
    #endregion
}