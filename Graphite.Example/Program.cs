using Graphite;
using Graphite.Eventing.Sources.Player;
using Graphite.Eventing.Sources.Server;
using Graphite.Hosting;
using Graphite.Worlds;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.Logging.SetMinimumLevel(LogLevel.Trace);

builder.Services.AddGraphite();

var host = builder.Build();

const string name = "Default";

host.UseSubscriber<Server>()
	.On<Starting>(async (starting, _) =>
	{
		var worldContainer = starting.Source.WorldContainer;
		await worldContainer.CreateAsync(name, new EmptyGenerator());
	})
	.On<Stopping>(stopping => stopping.Source.WorldContainer.Delete(name));

host.UseSubscriber<Player>()
	.On<Joining>(async (joining, _) =>
	{
		var world = joining.Source.Server.WorldContainer.Worlds[name];
		await joining.Source.SpawnAsync(world);
	});

host.Run();