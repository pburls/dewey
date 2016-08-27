using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Dewey.Messaging
{
    class CommandProcessor : ICommandProcessor
    {
        readonly Container _container;
        readonly IEventAggregator _eventAggregator;
        readonly Dictionary<Type, Type> _commandHandlers;

        public CommandProcessor(Container container, IEventAggregator eventAggregator)
        {
            _container = container;
            _eventAggregator = eventAggregator;

            _commandHandlers = new Dictionary<Type, Type>();
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
                commandProcessor = _container.GetInstance(commandHandlerType);

                MethodInfo executeMethod = commandHandlerType.GetMethod("Execute");
                executeMethod.Invoke(commandProcessor, new[] { command });
            }

            return commandProcessor;
        }
    }
}
