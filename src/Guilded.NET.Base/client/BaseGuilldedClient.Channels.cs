using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Guilded.NET.Base
{
    using Content;
    using Embeds;
    using Teams;
    using Chat;
    /// <summary>
    /// Represents any Guilded client.
    /// </summary>
    public abstract partial class BaseGuildedClient
    {
        #region Webhook
        /// <summary>
        /// Creates a webhook in a given channel.
        /// </summary>
        /// <param name="channelId">ID of the channel to create webhook in</param>
        /// <param name="name">The name of the webhook</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Created webhook</returns>
        public abstract Task<Webhook> CreateWebhookAsync(Guid channelId, string name);
        /// <summary>
        /// Updates webhook's name or profile picture.
        /// </summary>
        /// <param name="channelId">ID of the channel where the webhook is in</param>
        /// <param name="webhookId">ID of the webhook to update</param>
        /// <param name="name">A new name of the webhook</param>
        /// <param name="avatar">Profile picture/icon of the webhook</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Updated webhook</returns>
        public abstract Task<Webhook> UpdateWebhookAsync(Guid channelId, Guid webhookId, string name, Uri avatar);
        /// <summary>
        /// Deletes a webhook.
        /// </summary>
        /// <param name="webhookId">ID of the webhook to delete</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Deleted webhook</returns>
        public abstract Task<Webhook> DeleteWebhookAsync(Guid webhookId);
        /// <summary>
        /// Posts a message using a webhook.
        /// </summary>
        /// <param name="webhookId">ID of the webhook</param>
        /// <param name="token">Token of this webhook</param>
        /// <param name="content">Message to send using the webhook</param>
        /// <param name="embeds">An array of embeds to send</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        public abstract Task ExecuteWebhookAsync(Guid webhookId, string token, string content = null, params Embed[] embeds);
        #endregion


        #region Chat channels
        /// <summary>
        /// Gets messages with a specific limit.
        /// </summary>
        /// <param name="channelId">Channel to get messages in</param>
        /// <param name="limit">How many messages it should get</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>List of messages</returns>
        public abstract Task<IList<Message>> GetMessagesAsync(Guid channelId, uint limit = 50);
        /// <summary>
        /// Gets a message in a specific channel.
        /// </summary>
        /// <param name="channelId">ID of the channel where that message is</param>
        /// <param name="messageId">ID of message it should get</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Message</returns>
        public abstract Task<Message> GetMessageAsync(Guid channelId, Guid messageId);
        /// <summary>
        /// Sends a message into the chat.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="content">The contents of the message in rich text markup</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Message posted</returns>
        public abstract Task<Message> SendMessageAsync(Guid channelId, MessageContent content);
        /// <summary>
        /// Sends a message into the chat.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="content">The contents of the message in Markdown plain text</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Message posted</returns>
        public abstract Task<Message> SendMessageAsync(Guid channelId, string content);
        /// <summary>
        /// Sends a message into the chat.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="format">The composite format string</param>
        /// <param name="args">The arguments of the format string</param>
        /// <returns>Message posted</returns>
        public async Task<Message> SendMessageAsync(Guid channelId, string format, params object[] args) =>
            await SendMessageAsync(channelId, string.Format(format, args));
        /// <summary>
        /// Sends a message into the chat.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="provider">The provider that gives the format string information about the culture</param>
        /// <param name="format">The composite format string</param>
        /// <param name="args">The arguments of the format string</param>
        /// <returns>Message posted</returns>
        public async Task<Message> SendMessageAsync(Guid channelId, IFormatProvider provider, string format, params object[] args) =>
            await SendMessageAsync(channelId, string.Format(provider, format, args));
        /// <summary>
        /// Sends a message into the chat.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="content">The contents of the message in Markdown plain text</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Message posted</returns>
        public async Task<Message> SendMessageAsync(Guid channelId, object content) =>
            await SendMessageAsync(channelId, content.ToString());
        /// <summary>
        /// Edits message of the bot posted in the chat.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message to edit</param>
        /// <param name="content">The new content of the message in rich text markup</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Message edited</returns>
        public abstract Task<Message> EditMessageAsync(Guid channelId, Guid messageId, MessageContent content);
        /// <summary>
        /// Edits message of the bot posted in the chat.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message to edit</param>
        /// <param name="content">The contents of the message in Markdown plain text</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Message edited</returns>
        public abstract Task<Message> EditMessageAsync(Guid channelId, Guid messageId, string content);
        /// <summary>
        /// Edits message of the bot posted in the chat.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message to edit</param>
        /// <param name="format">The composite format string</param>
        /// <param name="args">The arguments of the format string</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Message edited</returns>
        public async Task<Message> EditMessageAsync(Guid channelId, Guid messageId, string format, params object[] args) =>
            await EditMessageAsync(channelId, messageId, string.Format(format, args));
        /// <summary>
        /// Edits message of the bot posted in the chat.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message to edit</param>
        /// <param name="provider">The provider that gives the format string information about the culture</param>
        /// <param name="format">The composite format string</param>
        /// <param name="args">The arguments of the format string</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Message edited</returns>
        public async Task<Message> EditMessageAsync(Guid channelId, Guid messageId, IFormatProvider provider, string format, params object[] args) =>
            await EditMessageAsync(channelId, messageId, string.Format(provider, format, args));
        /// <summary>
        /// Edits message of the bot posted in the chat.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message to edit</param>
        /// <param name="content">The contents of the message in Markdown plain text</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Message edited</returns>
        public async Task<Message> EditMessageAsync(Guid channelId, Guid messageId, object content) =>
            await EditMessageAsync(channelId, messageId, content.ToString());
        /// <summary>
        /// Deletes a message posted in the chat.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message to delete</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        public abstract Task DeleteMessageAsync(Guid channelId, Guid messageId);
        /// <summary>
        /// Add a reaction to a specific message.
        /// </summary>
        /// <param name="channelId">ID of the channel where the message is in</param>
        /// <param name="messageId">ID of the message to add a reaction on</param>
        /// <param name="emoteId">ID of the emote to add</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Reaction added</returns>
        public abstract Task<Reaction> AddReactionAsync(Guid channelId, Guid messageId, uint emoteId);
        /// <summary>
        /// Removes a reaction from a specific message.
        /// </summary>
        /// <param name="channelId">ID of the channel where the message is in</param>
        /// <param name="messageId">ID of the message to remove a reaction from</param>
        /// <param name="emoteId">ID of the emote to remove</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        public abstract Task RemoveReactionAsync(Guid channelId, Guid messageId, uint emoteId);
        #endregion


        /*#region Threads
        /// <summary>
        /// Creates a thread as a response to a message.
        /// </summary>
        /// <param name="channelId">ID of the channel to create thread in</param>
        /// <param name="name">Name of the thread</param>
        /// <param name="message">Message to respond to</param>
        /// <param name="response">Content of the response</param>
        /// <param name="contentType">Type of the channel where thread should be created in</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Thread created</returns>
        Task<ThreadChannel> CreateThreadAsync(Guid channelId, string name, Message message, MessageContent response, ChannelType contentType = ChannelType.Chat);
        /// <summary>
        /// Leaves a thread and no longer receives notifications from it.
        /// </summary>
        /// <param name="threadId">ID of the thread to leave</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task LeaveThreadAsync(Guid threadId);
        #endregion*/

        
        #region Forum channels
        /// <summary>
        /// Creates a forum post in a forum channel.
        /// </summary>
        /// <param name="channelId">The identifier of the channel to post in</param>
        /// <param name="title">The title of the forum post</param>
        /// <param name="content">The content of the forum post</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Forum post created</returns>
        public abstract Task<ForumThread> CreateForumPostAsync(Guid channelId, string title, string content);
        /// <summary>
        /// Creates a forum post in a forum channel.
        /// </summary>
        /// <param name="channelId">The identifier of the channel to post in</param>
        /// <param name="title">The title of the forum post</param>
        /// <param name="content">The content of the forum post</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Forum post created</returns>
        public abstract Task<ForumThread> CreateForumPostAsync(Guid channelId, string title, MessageContent content);
        /// <summary>
        /// Creates a forum post in a forum channel.
        /// </summary>
        /// <param name="channelId">The identifier of the channel to post in</param>
        /// <param name="title">The title of the forum post</param>
        /// <param name="format">The composite format string</param>
        /// <param name="args">The arguments of the format string</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Forum post created</returns>
        public async Task<ForumThread> CreateForumPostAsync(Guid channelId, string title, string format, params object[] args) =>
            await CreateForumPostAsync(channelId, title, string.Format(format, args));
        /// <summary>
        /// Creates a forum post in a forum channel.
        /// </summary>
        /// <param name="channelId">The identifier of the channel to post in</param>
        /// <param name="title">The title of the forum post</param>
        /// <param name="provider">The provider that gives the format string information about the culture</param>
        /// <param name="format">The composite format string</param>
        /// <param name="args">The arguments of the format string</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Forum post created</returns>
        public async Task<ForumThread> CreateForumPostAsync(Guid channelId, string title, IFormatProvider provider, string format, params object[] args) =>
            await CreateForumPostAsync(channelId, title, string.Format(provider, format, args));
        /// <summary>
        /// Creates a forum post in a forum channel.
        /// </summary>
        /// <param name="channelId">The identifier of the channel to post in</param>
        /// <param name="title">The title of the forum post</param>
        /// <param name="content">The content of the forum post</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Forum post created</returns>
        public async Task<ForumThread> CreateForumPostAsync(Guid channelId, string title, object content) =>
            await CreateForumPostAsync(channelId, title, content.ToString());
        #endregion        

        /*
        /// <summary>
        /// Gets forum posts from a specific forum channel.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="beforeDate">Before what date it should get posts</param>
        /// <param name="maxItems">How many forum posts it should get</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Forum post list</returns>
        Task<IList<ForumPost>> GetForumPostsAsync(Guid channelId, uint maxItems = 1000, DateTime? beforeDate = null);
        /// <summary>
        /// Edits a forum post in a specific channel.
        /// </summary>
        /// <param name="channelId">ID of the channel post is in</param>
        /// <param name="postId">ID of the post</param>
        /// <param name="title">New title of this post</param>
        /// <param name="message">New content of the post</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task EditForumPostAsync(Guid channelId, uint postId, string title, MessageContent message);
        /// <summary>
        /// Deletes a forum post in a specific channel.
        /// </summary>
        /// <param name="channelId">ID of the channel where the post is in</param>
        /// <param name="postId">ID of the post to delete</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task DeleteForumPostAsync(Guid channelId, uint postId);
        /// <summary>
        /// Gets replies from a forum post.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="postId">ID of the post it should get replies of</param>
        /// <param name="maxItems">How many forum posts it should get</param>
        /// <param name="afterDate">After what date it should get posts</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Forum reply list</returns>
        Task<IList<ForumReply>> GetForumRepliesAsync(Guid channelId, uint postId, uint maxItems = 10, DateTime? afterDate = null);
        /// <summary>
        /// Replies to a forum post.
        /// </summary>
        /// <param name="channelId">ID of the forum channel</param>
        /// <param name="postId">ID of the post it should reply to</param>
        /// <param name="message">Content of the forum post</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task CreateForumReplyAsync(Guid channelId, uint postId, MessageContent message);
        /// <summary>
        /// Deletes a forum reply/comment.
        /// </summary>
        /// <param name="channelId">ID of the channel where the post is in</param>
        /// <param name="postId">A forum post where reply should be deleted</param>
        /// <param name="replyId">A reply of a forum post that should be deleted</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task DeleteForumReplyAsync(Guid channelId, uint postId, uint replyId);
        /// <summary>
        /// Edits a forum reply.
        /// </summary>
        /// <param name="channelId">ID of the channel where forum post is in</param>
        /// <param name="postId">ID of the post to edit reply in</param>
        /// <param name="replyId">Reply to edit contents of</param>
        /// <param name="content">New content which will replace the old content</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task EditForumReplyAsync(Guid channelId, uint postId, uint replyId, MessageContent content);
        /// <summary>
        /// Adds a reaction to a forum reply or a forum post.
        /// </summary>
        /// <param name="teamId">ID of the team where the post is</param>
        /// <param name="postId">ID of forum post or reply to react on</param>
        /// <param name="emoteId">ID of the emote to react with</param>
        /// <param name="isContentReply">Is it a reaction on a reply</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task AddForumReactionAsync(GId teamId, uint postId, uint emoteId, bool isContentReply = false);
        /// <summary>
        /// Removes a reaction from a forum reply or a forum post.
        /// </summary>
        /// <param name="teamId">ID of the team where the post is</param>
        /// <param name="postId">ID of content or reply to react on</param>
        /// <param name="emoteId">ID of the emote to react with</param>
        /// <param name="isContentReply">Is it a reaction on a reply</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task RemoveForumReactionAsync(GId teamId, uint postId, uint emoteId, bool isContentReply = false)
        #endregion

        #region Document channels
        /// <summary>
        /// Gets all documents within a specific channel with given max count and before given date.
        /// </summary>
        /// <param name="channelId">ID of channel to fetch documents from</param>
        /// <param name="maxItems">Amount of documents to get</param>
        /// <param name="beforeDate">Date before which it should get documents</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>List of documents</returns>
        Task<IList<GuildedDocument>> GetDocumentsAsync(Guid channelId, uint maxItems = 50, DateTime? beforeDate = null);
        /// <summary>
        /// Gets a specific document in a specific channel.
        /// </summary>
        /// <param name="channelId">ID of channel to fetch documents from</param>
        /// <param name="docId">ID of the document</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Document</returns>
        Task<GuildedDocument> GetDocumentAsync(Guid channelId, uint docId);
        /// <summary>
        /// Updates already existing document.
        /// </summary>
        /// <param name="doc">Document to update</param>
        /// <param name="title">New title of the document</param>
        /// <param name="content">New message of the document</param>
        /// <param name="isDraft">Whether it is a draft or not</param>
        /// <returns>Updated document</returns>
        Task<GuildedDocument> UpdateDocumentAsync(GuildedDocument doc, string title = null, MessageContent content = null, bool? isDraft = null);
        /// <summary>
        /// Deletes a document from doc channel.
        /// </summary>
        /// <param name="channelId">ID of the channel where the document is in</param>
        /// <param name="docId">ID of the document to delete</param>
        Task DeleteDocumentAsync(Guid channelId, uint docId);
        #endregion


        #region Media channels
        /// <summary>
        /// Gets all media within a specific channel with given max count and before given date.
        /// </summary>
        /// <param name="channelId">ID of channel to fetch media from</param>
        /// <param name="pageSize">Limit of media to get</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>List of media posts</returns>
        Task<IList<GuildedMedia>> GetMediaAsync(Guid channelId, uint pageSize = 40);
        /// <summary>
        /// Gets all comments in a given media post.
        /// </summary>
        /// <param name="mediaId">ID of the media post</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>List of content replies</returns>
        Task<IList<ContentReply>> GetMediaRepliesAsync(uint mediaId);
        /// <summary>
        /// Posts new media post.
        /// </summary>
        /// <param name="teamId">ID of the team where the channel is</param>
        /// <param name="channelId">ID of the channel where to post it</param>
        /// <param name="src">Source URL</param>
        /// <param name="title">Title of the media</param>
        /// <param name="description">Description of the media</param>
        /// <param name="type">Type of the media: image or video</param>
        /// <param name="gameId">ID of the game of the group</param>
        /// <param name="tags">Tags associated with the media</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Media posted</returns>
        Task<GuildedMedia> PostMediaAsync(GId teamId, Guid channelId, Uri src, string title, string description = "", MediaType type = MediaType.Image, uint? gameId = null, params string[] tags);
        /// <summary>
        /// Deletes media post.
        /// </summary>
        /// <param name="channelId">ID of the channel where the media is</param>
        /// <param name="mediaId">ID of the media to delete</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task DeleteMediaAsync(Guid channelId, uint mediaId);
        #endregion


        #region Calendar channels
        /// <summary>
        /// Gets given amount of events from a specific channel.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="startDate">At which date it should start</param>
        /// <param name="endDate">At which date it should end</param>
        /// <param name="maxItems">How many events it should get</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>List of calendar events</returns>
        Task<IList<CalendarEvent>> GetEventsAsync(Guid channelId, DateTime startDate, DateTime endDate, uint maxItems = 250);
        #endregion

        
        #region Scheduling channels
        /// <summary>
        /// Get availabilities in a schedule channel.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>List of availabilities</returns>
        Task<IList<Availability>> GetSchedulesAsync(Guid channelId);
        /// <summary>
        /// Creates a schedule availability.
        /// </summary>
        /// <param name="channelId">ID of the channel where to create a schedule</param>
        /// <param name="startDate">Start date of this availability</param>
        /// <param name="endDate">End date of this availability</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Created schedule availability</returns>
        Task<IList<Availability>> CreateScheduleAsync(Guid channelId, DateTime startDate, DateTime endDate);
        /// <summary>
        /// Edits a schedule availability.
        /// </summary>
        /// <param name="channelId">ID of the channel where an availability is</param>
        /// <param name="availabilityId">ID of schedule availability to edit</param>
        /// <param name="startDate">Start date of this availability</param>
        /// <param name="endDate">End date of this availability</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Edited schedule availability</returns>
        Task<IList<Availability>> EditScheduleAsync(Guid channelId, uint availabilityId, DateTime startDate, DateTime endDate);
        /// <summary>
        /// Deletes a schedule availability.
        /// </summary>
        /// <param name="channelId">ID of the channel where an availability is</param>
        /// <param name="availabilityId">ID of schedule availability to edit</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task DeleteScheduleAsync(Guid channelId, uint availabilityId);
        #endregion

        #region Announcements channels
        /// <summary>
        /// Gets a list of announcements in a specific channel.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="maxItems">How many announcements it should get</param>
        /// <param name="beforeDate">Before which date it should get announcements</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>List of announcements</returns>
        Task<IList<Announcement>> GetAnnouncementsAsync(Guid channelId, uint maxItems = 10, DateTime? beforeDate = null);
        /// <summary>
        /// Gets a list of announcements in a team.
        /// </summary>
        /// <param name="teamId">ID of the team</param>
        /// <param name="maxItems">How many announcements it should get</param>
        /// <param name="beforeDate">Before which date it should get announcements</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>List of announcements</returns>
        Task<IList<Announcement>> GetAnnouncementsAsync(GId teamId, uint maxItems, DateTime? beforeDate);
        /// <summary>
        /// Gets a list of pinned announcements in a specific channel.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <returns>List of announcements</returns>
        Task<IList<Announcement>> GetPinnedAnnouncementsAsync(Guid channelId);
        /// <summary>
        /// Gets a list of pinned announcements in a team.
        /// </summary>
        /// <param name="teamId">ID of the team</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>List of announcements</returns>
        Task<IList<Announcement>> GetPinnedAnnouncementsAsync(GId teamId);
        /// <summary>
        /// Creates and posts a new announcement.
        /// </summary>
        /// <param name="teamId">ID of the team to create announcement in</param>
        /// <param name="channelId">ID of the channel to create announcement in</param>
        /// <param name="title">Title of the announcement</param>
        /// <param name="content">Content of the announcement</param>
        /// <param name="dontSendNotifications">If it should not send a notification to everyone</param>
        /// <param name="gameId">ID of the group's game</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Created announcement</returns>
        Task<Announcement> PostAnnouncementAsync(GId teamId, Guid channelId, string title, MessageContent content, bool dontSendNotifications = true, uint? gameId = null);
        /// <summary>
        /// Pins or unpins an announcement.
        /// </summary>
        /// <param name="channelId">ID of the channel where announcement is in</param>
        /// <param name="announcementId">ID of the announcement to (un)pin</param>
        /// <param name="isPinned">True - pin announcement, false - unpin announcement</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task PinAnnouncementAsync(Guid channelId, GId announcementId, bool isPinned = true);
        /// <summary>
        /// Updates/edits an announcement.
        /// </summary>
        /// <param name="teamId">ID of the team where announcement is</param>
        /// <param name="channelId">ID of the channel where announcement is</param>
        /// <param name="announcementId">ID of the announcement to edit</param>
        /// <param name="title">New title</param>
        /// <param name="content">New content</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>ID of edited/updated announcement</returns>
        Task<GId> UpdateAnnouncementAsync(GId teamId, Guid channelId, GId announcementId, string title, MessageContent content);
        /// <summary>
        /// Deletes an announcement.
        /// </summary>
        /// <param name="channelId">ID of the channel where announcement is</param>
        /// <param name="announcementId">ID of the announcement to delete</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>Deleted announcement</returns>
        Task<Announcement> DeleteAnnouncementAsync(Guid channelId, GId announcementId);
        #endregion*/


        #region List channels
        /// <summary>
        /// Creates a new list item in a list channel.
        /// </summary>
        /// <param name="channelId">The identifier of the channel to create in</param>
        /// <param name="message">The title content of this list item</param>
        /// <param name="note">The note of this list item</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>List item created</returns>
        public abstract Task<ListItem> CreateListItemAsync(Guid channelId, string message, string note = null);
        /// <summary>
        /// Creates a new list item in a list channel.
        /// </summary>
        /// <param name="channelId">The identifier of the channel to create in</param>
        /// <param name="message">The title content of this list item</param>
        /// <param name="note">The note of this list item</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>List item created</returns>
        public abstract Task<ListItem> CreateListItemAsync(Guid channelId, MessageContent message, MessageContent note = null);
        #endregion


        /*
        /// <summary>
        /// Gets list items in a given channel.
        /// </summary>
        /// <param name="channelId">ID of the channel to get list items from</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>List of list items</returns>
        Task<IList<ListItem>> GetListItemsAsync(Guid channelId);
        /// <summary>
        /// Edits a list item.
        /// </summary>
        /// <remarks>
        /// Changes the content or the note of the given list. To only edit content, keep note null. To only edit note, keep content null.
        /// </remarks>
        /// <example>
        /// Example of editing note:
        /// <code>
        /// EditListItemAsync(channelId, itemId, note: MessageContent.Generate("We will have to either use library X or library Y."));
        /// </code>
        /// Example of editing content:
        /// <code>
        /// EditListItemAsync(channelId, itemId, content: MessageContent.Generate("Create X in Y"));
        /// </code>
        /// </example>
        /// <param name="channelId">ID of the channel where list item is</param>
        /// <param name="itemId">List item to edit</param>
        /// <param name="content">New list item content/message/title(null if you only need to edit a note)</param>
        /// <param name="note">New list item note(null if you only need to edit content)</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task EditListItemAsync(Guid channelId, Guid itemId, MessageContent content, MessageContent note);
        /// <summary>
        /// Deletes a list item.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="itemId">ID of the item</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task DeleteListItemAsync(Guid channelId, Guid itemId);
        #endregion


        #region Content channels
        /// <summary>
        /// Gets all comments in a given document or media.
        /// </summary>
        /// <param name="contentId">ID of content</param>
        /// <param name="type">Type of the channel</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        /// <returns>List of content replies</returns>
        Task<IList<ContentReply>> GetContentRepliesAsync(string contentId, ChannelType type);
        /// <summary>
        /// Creates a reply to a document, media, event or announcement.
        /// </summary>
        /// <param name="teamId">ID of the team where the content is</param>
        /// <param name="channelId">ID of the channel where the content is</param>
        /// <param name="contentId">ID of the content reply is in</param>
        /// <param name="type">Type of the channel this reply is in</param>
        /// <param name="message">New message content to replace with</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task CreateContentReplyAsync(GId teamId, Guid channelId, string contentId, ChannelType type, MessageContent message);
        /// <summary>
        /// Deletes a document or a media reply.
        /// </summary>
        /// <param name="teamId">ID of the team</param>
        /// <param name="contentId">ID of the content</param>
        /// <param name="replyId">ID of the reply to delete</param>
        /// <param name="type">Channel's type</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task DeleteContentReplyAsync(GId teamId, string contentId, uint replyId, ChannelType type);
        /// <summary>
        /// Edits content reply's message.
        /// </summary>
        /// <param name="contentId">ID of the content reply is in</param>
        /// <param name="replyId">ID of the reply to edit</param>
        /// <param name="type">Type of the channel this reply is in</param>
        /// <param name="message">New message content to replace with</param>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task EditContentReplyAsync(string contentId, uint replyId, ChannelType type, MessageContent message);
        /// <summary>
        /// Adds a reaction to a reply or a post.
        /// </summary>
        /// <param name="teamId">ID of the team where the post is</param>
        /// <param name="type">Type of the channel where the post is</param>
        /// <param name="postId">ID of content or reply to react on</param>
        /// <param name="emoteId">ID of the emote to react with</param>
        /// <param name="isContentReply">Is it a reaction on a reply</param>
        /// <exception cref="ArgumentException">When channel type isn't type of a post channel</exception>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task AddReactionAsync(GId teamId, ChannelType type, string postId, uint emoteId, bool isContentReply = false);
        /// <summary>
        /// Removes a reaction from a reply or a post.
        /// </summary>
        /// <param name="teamId">ID of the team where the post is</param>
        /// <param name="type">Type of the channel where the post is</param>
        /// <param name="postId">ID of content or reply to react on</param>
        /// <param name="emoteId">ID of the emote to react with</param>
        /// <param name="isContentReply">Is it a reaction on a reply</param>
        /// <exception cref="ArgumentException">When channel type isn't type of a post channel</exception>
        /// <exception cref="GuildedException">Exception thrown by Guilded API</exception>
        Task RemoveReactionAsync(GId teamId, ChannelType type, string postId, uint emoteId, bool isContentReply = false);
        #endregion*/
    }
}