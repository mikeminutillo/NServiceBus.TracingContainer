namespace NServiceBus.TracingContainer
{
    using System;
    using NServiceBus;

    public class ConfigureEvent : TracingContainerEvent
    {
        public Type Component { get; internal set; }
        public DependencyLifecycle Lifecycle { get; internal set; }
        public bool IsFactory { get; internal set; }

        public override string ToString() => IsFactory
            ? $"{Container} => Configure Factory: {Component} [{Lifecycle}]"
            : $"{Container} => Configure: {Component} [{Lifecycle}]";
    }
}