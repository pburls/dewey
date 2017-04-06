namespace Dewey.Messaging
{
    public interface ICommandHandlerFactory<in TCommand> where TCommand : ICommand
    {
        ICommandHandler<TCommand> CreateHandler();
    }
}
