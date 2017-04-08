using Ark3.Command;
using Ark3.Event;
using System;

namespace Dewey.Messaging
{
    public interface ICommandCompleteEvent : IEvent
    {
        ICommand Command { get; }
        bool IsSuccessful { get; }
        TimeSpan ElapsedTime { get; }
    }
}
