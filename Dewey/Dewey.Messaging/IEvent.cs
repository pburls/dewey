namespace Dewey.Messaging
{
    public interface IEvent
    {
    }

    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }

    public interface IEventAggregator
    {
        void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : IEvent;
        void PublishEvent<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
