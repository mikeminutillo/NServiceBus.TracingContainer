namespace NServiceBus.TracingContainer
{
    using System.Collections.Generic;

    public interface ITraceContainerScope
    {
        string Name { get; }
        IEnumerable<TracingContainerEvent> Events { get; }
    }
}