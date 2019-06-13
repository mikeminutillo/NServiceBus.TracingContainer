namespace NServiceBus.TracingContainer
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using NServiceBus;
    using ObjectBuilder.Common;

    public class TracingContainer : IContainer
    {
        private readonly IContainer innerContainer;
        private readonly TraceContainerScope scope;

        private static IContainer DefaultNsbContainer()
            => (IContainer) Activator.CreateInstance(
                Type.GetType("NServiceBus.LightInjectObjectBuilder, NServiceBus.Core")
                ?? throw new Exception("Cannot determine default container. Please specify an inner container"));

        public TracingContainer(IContainer innerContainer = null, Action<TracingContainerEvent> onEvent = null) : this(innerContainer ?? DefaultNsbContainer(), null, onEvent)
        {
        }

        internal TracingContainer(IContainer innerContainer, TraceContainerScope scope, Action<TracingContainerEvent> onEvent = null)
        {
            this.innerContainer = innerContainer;
            this.scope = scope ?? new TraceContainerScope("Main", onEvent);
        }

        public ITraceContainerScope Trace => scope;

        void IDisposable.Dispose()
        {
            scope.Trace(new DisposeEvent());
            innerContainer.Dispose();
        }

        object IContainer.Build(Type typeToBuild)
        {
            scope.Trace(new BuildEvent { TypeToBuild = typeToBuild });
            return innerContainer.Build(typeToBuild);
        }

        IContainer IContainer.BuildChildContainer()
        {
            var childScope = scope.CreateChild();
            scope.Trace(new BuildChildContainerEvent { Scope = childScope });
            return new TracingContainer(innerContainer.BuildChildContainer(), childScope);
        }

        IEnumerable<object> IContainer.BuildAll(Type typeToBuild)
        {
            scope.Trace(new BuildAllEvent { TypeToBuild = typeToBuild });
            return innerContainer.BuildAll(typeToBuild);
        }

        void IContainer.Configure(Type component, DependencyLifecycle dependencyLifecycle)
        {
            scope.Trace(new ConfigureEvent { Component = component, Lifecycle = dependencyLifecycle });
            innerContainer.Configure(component, dependencyLifecycle);
        }

        void IContainer.Configure<T>(Func<T> component, DependencyLifecycle dependencyLifecycle)
        {
            scope.Trace(new ConfigureEvent { Component = typeof(T), Lifecycle = dependencyLifecycle, IsFactory = true });
            innerContainer.Configure(component, dependencyLifecycle);
        }

        void IContainer.RegisterSingleton(Type lookupType, object instance)
        {
            scope.Trace(new SingletonRegistrationEvent { LookupType = lookupType });
            innerContainer.RegisterSingleton(lookupType, instance);
        }

        bool IContainer.HasComponent(Type componentType)
        {
            scope.Trace(new HasComponentEvent { Component = componentType });
            return innerContainer.HasComponent(componentType);
        }

        void IContainer.Release(object instance)
        {
            scope.Trace(new ReleaseEvent { Instance = instance });
            innerContainer.Release(instance);
        }

        public void Milestone(string milestone)
        {
            scope.Trace(new LifecycleEvent { Milestone = milestone });
        }
    }
}