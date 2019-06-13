namespace NServiceBus.TracingContainer
{
    public class DisposeEvent : TracingContainerEvent
    {
        public override string ToString() => $"{Container} => Disposed";
    }
}