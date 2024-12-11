using System.Net;
using Graphite;
using Graphite.Eventing.Sources.Player;
using Graphite.Eventing.Sources.Server;
using Graphite.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.Logging.SetMinimumLevel(LogLevel.Trace);

builder.Services.AddGraphite();

var host = builder.Build();

host.UseSubscriber<Player>()
	.On<Joining>(joining =>
	{
		if (joining.VerificationKey != "Secret")
		{
			joining.Player.Disconnect("Incorrect verification key!");
		}
	});

host.UseSubscriber<Server>()
	.On<Starting>(starting => starting.EndPoint = new(IPAddress.Loopback, 25565));

host.Run();