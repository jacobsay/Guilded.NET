using System;

namespace Guilded.NET.Base.Permissions
{
    /// <summary>
    /// Permissions related to media.
    /// </summary>
    /// <remarks>
    /// <para>Defines permissions related to media in media channels.</para>
    /// </remarks>
    [Flags]
    public enum MediaPermissions
    {
        /// <summary>
        /// No given permissions.
        /// </summary>
        None = 0,
        /// <summary>
        /// Allows you to create media
        /// </summary>
        CreateMedia = 1,
        /// <summary>
        /// Allows you to see media
        /// </summary>
        SeeMedia = 2,
        /// <summary>
        /// Allows you to edit media created by others and move media items to other channels
        /// </summary>
        ManageMedia = 4,
        /// <summary>
        /// Allows you to remove media created by others
        /// </summary>
        RemoveMedia = 8,

        #region Additional
        /// <summary>
        /// All of the permissions combined.
        /// </summary>
        All = CreateMedia | SeeMedia | ManageMedia | RemoveMedia,
        /// <summary>
        /// All of the manage permissions combined.
        /// </summary>
        Manage = ManageMedia | RemoveMedia,
        /// <summary>
        /// A simple permission combination allowing writing permissions and reading permissions.
        /// </summary>
        Basic = CreateMedia | SeeMedia
        #endregion
    }
}