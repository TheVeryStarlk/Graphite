using Graphite;
using Graphite.Eventing.Sources.Server;
using Graphite.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.Logging.SetMinimumLevel(LogLevel.Trace);

builder.Services.AddGraphite();

var host = builder.Build();

host.UseSubscriber<Server>()
	.On<Stopping>(original => original.Reason = "Got tired!");

host.Run();