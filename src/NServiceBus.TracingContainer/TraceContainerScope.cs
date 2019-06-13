using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NServiceBus.TracingContainer;

class TraceContainerScope : ITraceContainerScope
{
    private readonly Action<TracingContainerEvent> onEvent;
    private readonly ConcurrentQueue<TracingContainerEvent> events = new ConcurrentQueue<TracingContainerEvent>();
    private int children;

    public string Name { get; }

    public TraceContainerScope(string name, Action<TracingContainerEvent> onEvent = null)
    {
        this.onEvent = onEvent;
        Name = name;
    }

    public IEnumerable<TracingContainerEvent> Events => events.AsEnumerable();

    public TraceContainerScope CreateChild()
    {
        var childName = $"{Name}.{Interlocked.Increment(ref children)}";
        return new TraceContainerScope(childName, onEvent);
    }

    public void Trace(TracingContainerEvent @event)
    {
        @event.Container = Name;
        onEvent?.Invoke(@event);
        events.Enqueue(@event);
    }
}
