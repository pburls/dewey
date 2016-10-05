﻿using Moq;
using Xunit;

namespace Dewey.Messaging.Test
{
    public class EventAggregatorTest
    {
        EventAggregator eventAggregator;

        public EventAggregatorTest()
        {
            eventAggregator = new EventAggregator();
        }

        [Fact]
        public void Test_PublishEvent_Invokes_Event_Handlers_Subscribed_For_The_Events_Type()
        {
            //Given
            var @event = new TestEventA();
            var mockTestEventHandler = new Mock<IEventHandler<TestEventA>>();
            eventAggregator.Subscribe(mockTestEventHandler.Object);

            //When
            eventAggregator.PublishEvent(@event);

            //Then
            mockTestEventHandler.Verify(x => x.Handle(@event), Times.Once);
        }

        [Fact]
        public void Test_PublishEvent_Invokes_Event_Handlers_Subscribed_For_The_Events_Inherited_Types()
        {
            //Given
            var @event = new TestEventA();
            var mockBaseEventHandler = new Mock<IEventHandler<TestEventBase>>();
            var mockIEventHandler = new Mock<IEventHandler<IEvent>>();
            eventAggregator.Subscribe(mockBaseEventHandler.Object);
            eventAggregator.Subscribe(mockIEventHandler.Object);

            //When
            eventAggregator.PublishEvent(@event);

            //Then
            mockBaseEventHandler.Verify(x => x.Handle(@event), Times.Once);
            mockIEventHandler.Verify(x => x.Handle(@event), Times.Once);
        }

        [Fact]
        public void Test_PublishEvent_Only_Invokes_Event_Handlers_Subscribed_For_The_Events_Inherited_Types()
        {
            //Given
            var @event = new TestEventB();
            var mockBaseEventHandler = new Mock<IEventHandler<TestEventBase>>();
            var mockIEventHandler = new Mock<IEventHandler<IEvent>>();
            eventAggregator.Subscribe(mockBaseEventHandler.Object);
            eventAggregator.Subscribe(mockIEventHandler.Object);

            //When
            eventAggregator.PublishEvent(@event);

            //Then
            mockBaseEventHandler.Verify(x => x.Handle(It.IsAny<TestEventBase>()), Times.Never);
            mockIEventHandler.Verify(x => x.Handle(@event), Times.Once);
        }
    }
}
