namespace NServiceBus.TracingContainer
{
    using System;

    public class BuildEvent : TracingContainerEvent
    {
        public Type TypeToBuild { get; internal set; }

        public override string ToString() => $"{Container} => Build: {TypeToBuild}";
    }
}