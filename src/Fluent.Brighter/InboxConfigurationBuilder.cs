using System;

using Paramore.Brighter;
using Paramore.Brighter.Inbox;

namespace Fluent.Brighter;

public sealed class InboxConfigurationBuilder
{
    private IAmAnInbox? _inbox;
    public InboxConfigurationBuilder SetInbox(IAmAnInbox? inbox)
    {
        _inbox = inbox;
        return this;
    }

    private InboxScope _scope = InboxScope.All;
    public InboxConfigurationBuilder SetScope(InboxScope scope)
    {
        _scope = scope;
        return this;
    }

    
    private bool _onceOnly = true; 
    public InboxConfigurationBuilder SetOnceOnly(bool onceOnly)
    {
        _onceOnly = onceOnly;
        return this;
    }

    
    private OnceOnlyAction _actionOnExists = OnceOnlyAction.Throw; 
    public InboxConfigurationBuilder SetActionOnExists(OnceOnlyAction actionOnExists)
    {
        _actionOnExists = actionOnExists;
        return this;
    }

    
    private Func<Type, string>? _context;
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