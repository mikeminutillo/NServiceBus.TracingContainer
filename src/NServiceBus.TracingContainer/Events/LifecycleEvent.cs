namespace NServiceBus.TracingContainer
{
    public class LifecycleEvent : TracingContainerEvent
    {
        public string Milestone { get; internal set; }

        public override string ToString() => $"{Container} => Milestone: {Milestone}";
    }
}