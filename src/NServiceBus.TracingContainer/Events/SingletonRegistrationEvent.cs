namespace NServiceBus.TracingContainer
{
    using System;

    public class SingletonRegistrationEvent : TracingContainerEvent
    {
        public Type LookupType { get; internal set; }

        public override string ToString() => $"{Container} => Singleton Registered: {LookupType}";
    }
}