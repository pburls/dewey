using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Dewey.Messaging
{
    public class CommandProcessor : ICommandProcessor
    {
        readonly Container _container;
        readonly IEventAggregator _eventAggregator;
        readonly Dictionary<Type, object> _commandHandlerFactories;

        public CommandProcessor(Container container, IEventAggregator eventAggregator)
        {
            _container = container;
            _eventAggregator = eventAggregator;

            _commandHandlerFactories = new Dictionary<Type, object>();
        }

        public void RegisterHandlerFactory<TCommand, TCommandHandlerFactory>(TCommandHandlerFactory factory)
            where TCommand : ICommand
            where TCommandHandlerFactory : ICommandHandlerFactory<TCommand>
        {
            Type commandType = typeof(TCommand);

            _commandHandlerFactories.Add(commandType, factory);
        }

        public object Execute(ICommand command)
        {
            Type commandType = command.GetType();
            object commandHandlerFactory;
            object commandHandler = null;

            if (_commandHandlerFactories.TryGetValue(commandType, out commandHandlerFactory))
            {
                Type commandHandlerFactoryGenericType = typeof(ICommandHandlerFactory<>);
                Type commandHandlerFactoryType = commandHandlerFactoryGenericType.MakeGenericType(commandType);

                MethodInfo createMethod = commandHandlerFactoryType.GetMethod("CreateHandler");
                commandHandler = createMethod.Invoke(commandHandlerFactory, null);
                
                MethodInfo executeMethod = commandHandler.GetType().GetMethod("Execute", new[] { commandType });
                executeMethod.Invoke(commandHandler, new[] { command });
            }

            return commandHandler;
        }
    }
}
