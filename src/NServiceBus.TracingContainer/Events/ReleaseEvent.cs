namespace NServiceBus.TracingContainer
{
    public class ReleaseEvent : TracingContainerEvent
    {
        public object Instance { get; internal set; }
        public override string ToString() => $"{Container} => Release: {Instance}";
    }
}