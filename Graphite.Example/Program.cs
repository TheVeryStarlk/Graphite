using Graphite;
using Graphite.Eventing.Sources.Client;
using Graphite.Eventing.Sources.Server;
using Graphite.Hosting;
using Graphite.Networking.Packets.Ingoing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.Logging.SetMinimumLevel(LogLevel.Trace);

builder.Services.AddGraphite();

var host = builder.Build();

host.UseSubscriber<Client>()
	.On<ReceivedPacket>(original =>
	{
		if (original.Packet is not PlayerIdentificationPacket identificationPacket)
		{
			return;
		}

		if (identificationPacket.VerificationKey != "Secret")
		{
			original.Source.Stop("Incorrect verification key.");
		}
	});

host.UseSubscriber<Server>()
	.On<Stopping>(original => original.Reason = "Got tired!");

host.Run();