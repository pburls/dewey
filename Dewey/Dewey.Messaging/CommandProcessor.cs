using System;
using System.Collections.Generic;
using System.Reflection;

namespace Dewey.Messaging
{
    public class CommandProcessor : ICommandProcessor
    {
        private IEventAggregator _eventAggregator;
        private Dictionary<Type, Type> _commandHandlers = new Dictionary<Type, Type>();

        public CommandProcessor(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void RegisterHandler<TCommand, TCommandHandler>()
            where TCommand : ICommand
            where TCommandHandler : ICommandHandler<TCommand>
        {
            Type commandType = typeof(TCommand);
            Type commandHandlerType = typeof(TCommandHandler);

            _commandHandlers.Add(commandType, commandHandlerType);
        }

        public object Execute(ICommand command)
        {
            Type commandType = command.GetType();
            Type commandHandlerType = null;
            object commandProcessor = null;

            if (_commandHandlers.TryGetValue(commandType, out commandHandlerType))
            {
                commandProcessor = Activator.CreateInstance(commandHandlerType, this, _eventAggregator);

                MethodInfo executeMethod = commandHandlerType.GetMethod("Execute");
                executeMethod.Invoke(commandProcessor, new[] { command });
            }

            return commandProcessor;
        }
    }
}
