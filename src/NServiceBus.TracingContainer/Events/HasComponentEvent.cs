namespace NServiceBus.TracingContainer
{
    using System;

    public class HasComponentEvent : TracingContainerEvent
    {
        public Type Component { get; internal set; }

        public override string ToString() => $"{Container} => Has Component? {Component}";
    }
}