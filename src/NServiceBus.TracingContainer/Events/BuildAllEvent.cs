namespace NServiceBus.TracingContainer
{
    using System;

    public class BuildAllEvent : TracingContainerEvent
    {
        public Type TypeToBuild { get; internal set; }
        public override string ToString() => $"{Container} => Build All: {TypeToBuild}";
    }
}