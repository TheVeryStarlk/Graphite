using System.Net;
using Graphite;
using Graphite.Eventing.Sources.Server;
using Graphite.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.Logging.SetMinimumLevel(LogLevel.Trace);

builder.Services.AddGraphite();

var host = builder.Build();

host.UseSubscriber<Server>()
	.On<Starting>(original =>
	{
		original.EndPoint = new IPEndPoint(IPAddress.Loopback, 25565);

		var logger = host.Services.GetRequiredService<ILogger<Program>>();
		logger.LogInformation("Called from the event!");
	})
	.On<Stopping>(original => original.Reason = "Got tired!");

host.Run();