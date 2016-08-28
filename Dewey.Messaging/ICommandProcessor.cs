namespace Dewey.Messaging
{
    public interface ICommandProcessor
    {
        object Execute(ICommand command);
        void RegisterHandler<TCommand, TCommandHandler>()
            where TCommand : ICommand
            where TCommandHandler : ICommandHandler<TCommand>;
    }
}