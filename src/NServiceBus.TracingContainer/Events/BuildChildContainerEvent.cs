namespace NServiceBus.TracingContainer
{
    public class BuildChildContainerEvent : TracingContainerEvent
    {
        public ITraceContainerScope Scope { get; internal set; }
        public override string ToString() => $"{Container} => Build Child Container: {Scope.Name}";
    }
}