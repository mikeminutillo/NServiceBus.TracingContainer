namespace NServiceBus.TracingContainer
{
    using System.Collections.Generic;

    public static class TracingContainerExtensions
    {
        public static IEnumerable<TracingContainerEvent> AllEvents(this TracingContainer container)
        {
            return container.Trace.AllEvents();
        }

        public static IEnumerable<TracingContainerEvent> AllEvents(this ITraceContainerScope scope)
        {
            foreach (var @event in scope.Events)
            {
                yield return @event;
                if (@event is BuildChildContainerEvent childContainerEvent)
                {
                    foreach (var childEvent in childContainerEvent.Scope.AllEvents())
                    {
                        yield return childEvent;
                    }
                }
            }

        }
    }
}