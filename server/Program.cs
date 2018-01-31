using System;
using NServiceBus;
using shared;

namespace server
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("example.server");

			var endpointConfiguration = new EndpointConfiguration("example.server");
			endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
			endpointConfiguration.EnableInstallers();
			endpointConfiguration.UsePersistence<LearningPersistence>();
			endpointConfiguration.UseTransport<LearningTransport>();
			endpointConfiguration.SendFailedMessagesTo("error");

			var conventions = endpointConfiguration.Conventions();
			conventions.DefiningCommandsAs(
				type =>
				{
					return type.Namespace == "shared";
				});

			var result = Endpoint.Start(endpointConfiguration).Result;
			Console.WriteLine("Press any key to exit");
			Console.ReadKey();
		}
	}
}
