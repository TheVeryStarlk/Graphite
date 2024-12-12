using Graphite;
using Graphite.Eventing.Sources.Server;
using Graphite.Hosting;
using Graphite.Worlds;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.Logging.SetMinimumLevel(LogLevel.Trace);

builder.Services.AddGraphite();

var host = builder.Build();

host.UseSubscriber<Server>()
	.On<Starting>(async (starting, _) =>
	{
		var worldContainer = starting.Source.WorldContainer;
		await worldContainer.CreateAsync("Default", new EmptyGenerator());
	});

host.Run();