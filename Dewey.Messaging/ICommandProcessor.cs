namespace Dewey.Messaging
{
    public interface ICommandProcessor
    {
        object Execute(ICommand command);
        void RegisterHandlerFactory<TCommand, TCommandHandlerFactory>(TCommandHandlerFactory factory)
            where TCommand : ICommand
            where TCommandHandlerFactory : ICommandHandlerFactory<TCommand>;
    }
}