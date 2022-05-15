using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Guilded.Base;

namespace Guilded.Commands;

/// <summary>
/// Represents the information about methods that were declared as <see cref="CommandAttribute">commands</see>.
/// </summary>
public class CommandInfo : AbstractCommandInfo<MethodInfo>
{
    private static readonly Type[] _allowedTypes = new Type[]
    {
        typeof(string), typeof(bool),
        typeof(int), typeof(long), typeof(short), typeof(sbyte),
        typeof(uint), typeof(ulong), typeof(ushort), typeof(byte),
        typeof(float), typeof(double), typeof(decimal), typeof(DateTime),
        typeof(Guid), typeof(HashId)
    };
    /// <summary>
    /// Gets the enumerable of command arguments that can be specified by users.
    /// </summary>
    /// <value>Enumerable of command arguments</value>
    public CommandArgumentInfo[] Arguments { get; }
    /// <summary>
    /// Gets whether there is a rest argument for the command.
    /// </summary>
    /// <value>Command has rest argument</value>
    public bool HasRestArgument { get; private set; } = false;
    /// <summary>
    /// Initializes a new instance of <see cref="CommandInfo" /> from the <paramref name="method">command method</paramref>.
    /// </summary>
    /// <param name="method">The method that was declared as a command</param>
    /// <param name="attribute">The command attribute it was given</param>
    /// <param name="parameters">The parameters that will be used as command arguments</param>
    public CommandInfo(MethodInfo method, CommandAttribute attribute, IEnumerable<ParameterInfo> parameters) : base(attribute, method) =>
        Arguments = parameters.Select((arg, argIndex) =>
        {
            // Honestly, I don't know what I am doing. I don't want to do repetitive ifs,
            // especially since it can be a bit slower (see branchless programming). But it
            // still looks bad
            // REVIEW, FIXME: Code below
            if (arg.ParameterType == typeof(string[]))
                // We could probably do it backwards, but eh, better errors?
                if (argIndex + 1 != parameters.Count())
                    throw new InvalidOperationException("String array can only be the last command parameter");
                else HasRestArgument = true;
            else if (!_allowedTypes.Contains(arg.ParameterType))
                throw new InvalidOperationException($"Cannot have a command argument of type {arg.ParameterType}");

            return new CommandArgumentInfo(arg);
        }).ToArray();

    internal bool HasCorrectCount(int count) =>
        HasRestArgument ? count >= Arguments.Count() : count == Arguments.Count();

    /// <summary>
    /// Returns the enumerable of runtime method parameter values.
    /// </summary>
    /// <param name="arguments">The given arguments of the command</param>
    /// <returns>Enumerable of parameters</returns>
    internal IEnumerable<object>? GenerateMethodParameters(IEnumerable<string> arguments)
    {
        var generatedArguments =
            // (CommandEvent invokation, string arg0, int arg1)
            Arguments.Select<CommandArgumentInfo, object>((arg, argIndex) =>
            {
                string stringArgument = arguments.ElementAt(argIndex);

                Type argType = arg.ArgumentType;

                // Rest argument
                if (argType == typeof(string[])) return arguments.Skip(argIndex).ToArray();

                // Could use TryParse, but you can't do `out object` and
                // it would require different name for every parsed item
                // TODO: Use fields for types?
                return
                    argType == typeof(string)
                    ? stringArgument
                    : argType == typeof(bool)
                    ? bool.Parse(stringArgument)
                    : argType == typeof(int)
                    ? int.Parse(stringArgument)
                    : argType == typeof(long)
                    ? long.Parse(stringArgument)
                    : argType == typeof(short)
                    ? short.Parse(stringArgument)
                    : argType == typeof(sbyte)
                    ? sbyte.Parse(stringArgument)
                    : argType == typeof(uint)
                    ? uint.Parse(stringArgument)
                    : argType == typeof(ulong)
                    ? ulong.Parse(stringArgument)
                    : argType == typeof(ushort)
                    ? ushort.Parse(stringArgument)
                    : argType == typeof(byte)
                    ? byte.Parse(stringArgument)
                    : argType == typeof(float)
                    ? float.Parse(stringArgument)
                    : argType == typeof(double)
                    ? double.Parse(stringArgument)
                    : argType == typeof(decimal)
                    ? decimal.Parse(stringArgument)
                    : argType == typeof(DateTime)
                    ? DateTime.Parse(stringArgument)
                    : argType == typeof(Guid)
                    ? new Guid(stringArgument)
                    : argType == typeof(HashId)
                    ? new HashId(stringArgument)
                    : throw new FormatException($"Cannot have type {argType} as a command argument's type");
            });

        return generatedArguments;
    }
    /// <summary>
    /// Invokes the command.
    /// </summary>
    /// <param name="parent">The parent command of this command</param>
    /// <param name="commandEvent">The command event that invoked the command</param>
    /// <param name="arguments">The arguments that have been used to invoke the command</param>
    /// <returns>Whether the command was properly invoked</returns>
    public Task InvokeAsync(CommandBase parent, CommandEvent commandEvent, IEnumerable<object> arguments) =>
        Task.Run(() => Member.Invoke(parent, new object[] { commandEvent }.Concat(arguments.ToArray()).ToArray()));
}
/// <summary>
/// Represents the information about types that were declared as <see cref="CommandAttribute">commands</see>.
/// </summary>
public class CommandContainerInfo : AbstractCommandInfo<Type>
{
    /// <summary>
    /// Gets the created instance of <see cref="CommandAttribute">the command</see> type for this command.
    /// </summary>
    /// <value>Command instance</value>
    public CommandBase Instance { get; }
    /// <inheritdoc cref="CommandBase.Commands" />
    public IEnumerable<ICommandInfo<MemberInfo>> SubCommands => Instance.Commands;
    /// <summary>
    /// Initializes a new instance of <see cref="CommandContainerInfo" /> from the <paramref name="type">command type</paramref>.
    /// </summary>
    /// <param name="type">The type that was declared as a command</param>
    /// <param name="attribute">The command attribute it was given</param>
    /// <param name="instance">Other reflection members that were declared as commands</param>
    public CommandContainerInfo(Type type, CommandAttribute attribute, CommandBase instance) : base(attribute, type) =>
        Instance = instance;
}