using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Guilded.Base;
using Guilded.Base.Events;

namespace Guilded.Commands;

/// <summary>
/// Represents the module that adds <see cref="CommandAttribute">commands</see> to <see cref="BaseGuildedClient">Guilded clients</see>.
/// </summary>
/// <seealso cref="CommandBase" />
/// <seealso cref="CommandAttribute" />
/// <seealso cref="CommandFallbackAttribute" />
public class CommandModule : CommandBase
{
    #region Fields
    private AbstractGuildedClient? _subscribedClient;

    private IDisposable? _commandSubscription;
    #endregion

    #region Methods
    /// <summary>
    /// Checks if any <see cref="CommandAttribute">commands</see> are called in the message and invokes them.
    /// </summary>
    /// <param name="msgCreated">The supposed command message</param>
    /// <param name="prefix">The current prefix used for the command</param>
    /// <param name="config">The configuration of client's commands</param>
    /// <returns>Any <see cref="CommandAttribute">command</see> has been invoked</returns>
    public virtual async Task<bool> DoCommandsAsync(MessageEvent msgCreated, string prefix, CommandConfiguration config)
    {
        if (!msgCreated.Content!.StartsWith(prefix)) return false;

        string[] splitContent = msgCreated
            .Content[prefix.Length..]
            .Split(config.Separators, config.SplitOptions);

        string commandName = splitContent.First();

        if (string.IsNullOrEmpty(commandName)) return false;

        // First one is the name of the command
        IEnumerable<string> args = splitContent.Skip(1);

        RootCommandEvent context = new(msgCreated, prefix, commandName, args);

        return await InvokeCommandByNameAsync(context, commandName, args).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds the command module to the specified <paramref name="client" /> with the given settings.
    /// </summary>
    /// <param name="client">The client to add command module to</param>
    /// <param name="config">The configuration of the client's commands</param>
    public void AddTo(AbstractGuildedClient client, CommandConfiguration config)
    {
        if (_subscribedClient == client)
            throw new InvalidOperationException("Cannot add the same command module to the client");
        // If we remove the command module from the client, we don't need to have it subscribed
        else if (_subscribedClient is not null)
            _commandSubscription!.Dispose();

        // Command invokation detection
        _commandSubscription =
            client
                .MessageCreated
                .Where(msgCreated => msgCreated.Content is not null)
                .Subscribe(async msgCreated => await DoCommandsAsync(msgCreated, config.Prefix, config).ConfigureAwait(false));
        _subscribedClient = client;
    }

    /// <summary>
    /// Removes the command module from the subscribed client.
    /// </summary>
    public void Remove()
    {
        if (_subscribedClient is null)
            throw new InvalidOperationException("Command module is not attached to any client and cannot be unsubscribed");

        _commandSubscription!.Dispose();
        _subscribedClient = null;
    }
    #endregion
}