using System;

using Paramore.Brighter;
using Paramore.Brighter.Inbox;

namespace Fluent.Brighter;

/// <summary>
/// Builder for configuring inbox settings used to ensure message idempotency and deduplication
/// </summary>
/// <remarks>
/// Provides a fluent interface to configure inbox behavior for Brighter message processing.
/// The inbox pattern ensures messages are processed only once, maintaining idempotency.
/// </remarks>
public sealed class InboxConfigurationBuilder
{
    private IAmAnInbox? _inbox;
    
    /// <summary>
    /// Sets the inbox implementation to use for message tracking
    /// </summary>
    /// <param name="inbox">The inbox instance (null to disable inbox functionality)</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public InboxConfigurationBuilder SetInbox(IAmAnInbox? inbox)
    {
        _inbox = inbox;
        return this;
    }

    private InboxScope _scope = InboxScope.All;
    
    /// <summary>
    /// Sets the scope of messages to apply inbox processing to
    /// </summary>
    /// <param name="scope">The scope of messages (Commands, Events, or All)</param>
    /// <returns>The builder instance for fluent chaining</returns>
    /// <seealso cref="InboxScope"/>
    public InboxConfigurationBuilder SetScope(InboxScope scope)
    {
        _scope = scope;
        return this;
    }

    
    private bool _onceOnly = true; 
    
    /// <summary>
    /// Enables or disables once-only processing (idempotency guarantee)
    /// </summary>
    /// <param name="onceOnly">True to ensure messages are processed only once (default: true)</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public InboxConfigurationBuilder SetOnceOnly(bool onceOnly)
    {
        _onceOnly = onceOnly;
        return this;
    }

    
    private OnceOnlyAction _actionOnExists = OnceOnlyAction.Throw; 
    
    /// <summary>
    /// Sets the action to take when a duplicate message is detected
    /// </summary>
    /// <param name="actionOnExists">Action to perform (Throw, Warn, or Ignore)</param>
    /// <returns>The builder instance for fluent chaining</returns>
    /// <seealso cref="OnceOnlyAction"/>
    public InboxConfigurationBuilder SetActionOnExists(OnceOnlyAction actionOnExists)
    {
        _actionOnExists = actionOnExists;
        return this;
    }

    
    private Func<Type, string>? _context;
    
    /// <summary>
    /// Sets the context provider function for generating unique message identifiers
    /// </summary>
    /// <param name="context">
    /// Function that returns a unique key for a message type. 
    /// Uses message type name if not specified.
    /// </param>
    /// <returns>The builder instance for fluent chaining</returns>
    public InboxConfigurationBuilder SetContext(Func<Type, string>? context)
    {
        _context = context;
        return this;
    }

    internal InboxConfiguration Build()
    {
        return new InboxConfiguration(
            inbox: _inbox,
            scope: _scope,
            onceOnly: _onceOnly,
            actionOnExists: _actionOnExists,
            context: _context
        );
    }
}