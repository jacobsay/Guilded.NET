using System.Threading.Tasks;

using Guilded.Base.Users;

namespace Guilded.Base;

public abstract partial class BaseGuildedClient
{
    #region Profile info
    /// <summary>
    /// Gets the specified <paramref name="member">member's</paramref> social link based on given <paramref name="linkType">social link type</paramref>.
    /// </summary>
    /// <remarks>
    /// <para>This does not require any permissions to be given.</para>
    /// </remarks>
    /// <param name="server">The server where to fetch user's information</param>
    /// <param name="member">The identifier of <see cref="User">user</see></param>
    /// <param name="linkType">The social link to get</param>
    /// <exception cref="GuildedException" />
    /// <exception cref="GuildedResourceException" />
    /// <exception cref="GuildedAuthorizationException" />
    /// <returns><see cref="SocialLink">Social Link</see></returns>
    public abstract Task<SocialLink> GetSocialLinkAsync(HashId server, HashId member, SocialLinkType linkType);
    #endregion
}