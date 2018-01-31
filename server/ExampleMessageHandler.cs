using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using shared;

namespace server
{
	public class ExampleMessageHandler : IHandleMessages<ExampleMessage>
	{
		static ILog log = LogManager.GetLogger<ExampleMessageHandler>();

		public Task Handle(ExampleMessage message, IMessageHandlerContext context)
		{
			log.Info($"Handling message id: {message.Id}");
			return Task.FromResult(0);
		}
	}
}
