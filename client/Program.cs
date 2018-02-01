using System;
using NServiceBus;
using shared;

namespace client
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("example.client");

			var endpointConfiguration = new EndpointConfiguration(endpointName: "example.client");
			endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
			endpointConfiguration.EnableInstallers();
			var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
			transport.UseDirectRoutingTopology();
			transport.ConnectionString("host=rabbitserver");

			var conventions = endpointConfiguration.Conventions();
			conventions.DefiningCommandsAs(
				type =>
				{
					return type.Namespace == "shared";
				});

			var endpointInstance = Endpoint.Start(endpointConfiguration).Result;
			SendOrder(endpointInstance);

		}
		static void SendOrder(IEndpointInstance endpointInstance)
		{
			Console.WriteLine("Press enter to send a message");
			Console.WriteLine("Press any key to exit");

			while (true)
			{
				var key = Console.ReadKey();
				Console.WriteLine();

				if (key.Key != ConsoleKey.Enter)
				{
					return;
				}

				var exampleMessage = new ExampleMessage
				{
					Id = Guid.NewGuid()
				};

				endpointInstance.Send("example.server", exampleMessage);

				Console.WriteLine($"Sent a message with id: {exampleMessage.Id:N}");
			}
		}

	}
}
