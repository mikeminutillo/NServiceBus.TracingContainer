namespace TraceContainerUsage
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.TracingContainer;

    class Program
    {
        static async Task Main(string[] args)
        {
            var cfg = new EndpointConfiguration("TraceContainerUsage");

            var transport = cfg.UseTransport<LearningTransport>();
            cfg.UsePersistence<LearningPersistence>();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(SomeMessage), "null");

            var container = new TracingContainer(onEvent: Console.WriteLine);

            cfg.UseContainer(container);

            container.Milestone("Before Endpoint.Create");

            var startableEndpoint = await Endpoint.Create(cfg).ConfigureAwait(false);

            container.Milestone("Before endpoint.Start");

            var endpoint = await startableEndpoint.Start();

            Console.WriteLine();
            Console.WriteLine("Press any key to send a message. Press ESC to continue.");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                container.Milestone("Before endpoint.Send");

                await endpoint.Send(new SomeMessage());
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to send a message that gets handled. Press ESC to continue.");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                container.Milestone("Before endpoint.Send Handled");

                await endpoint.SendLocal(new MessageThatGetsHandled());
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to publish a message. Press ESC to continue.");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                container.Milestone("Before endpoint.Publish");

                await endpoint.Publish(new SomeEvent());
            }

            Console.WriteLine();
            Console.WriteLine("Press ESC to stop endpoint");
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {

            }
            
            container.Milestone("Before endpoint.Stop");

            await endpoint.Stop();

            File.WriteAllLines("ContainerTrace.txt", container.AllEvents().Select(e => e.ToString()));

            var duplicateConfigurations = container.AllEvents()
                .OfType<ConfigureEvent>()
                .GroupBy(g => g.Component)
                .Select(g => new {Component = g.Key, Count = g.Count()})
                .Where(g => g.Count > 1)
                .ToArray();

            Console.WriteLine("DUPLICATE CONFIGURATIONS");
            foreach (var g in duplicateConfigurations)
            {
                Console.WriteLine($"{g.Component}: {g.Count}");
            }

            Console.WriteLine();
            Console.WriteLine("A report has been written to ContainerTrace.txt");
            Console.WriteLine("Press ENTER to close");
            Console.ReadLine();
        }
    }

    public class SomeEvent : IEvent
    {

    }

    public class SomeMessage : IMessage
    {
       
    }

    public class MessageThatGetsHandled : IMessage
    {

    }

    public class MessageHandler : IHandleMessages<MessageThatGetsHandled>
    {
        public Task Handle(MessageThatGetsHandled message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }
}
