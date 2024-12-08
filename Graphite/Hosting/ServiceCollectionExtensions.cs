using Graphite.Eventing;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Graphite.Hosting;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGraphite(this IServiceCollection services)
	{
		services.AddSingleton<EventDispatcher>();

		services.AddTransient<IConnectionListenerFactory>(provider => new SocketTransportFactory(
			Options.Create(new SocketTransportOptions()),
			provider.GetRequiredService<ILoggerFactory>()));

		services.AddTransient<Server>();

		services.AddHostedService<WorkerService>();

		return services;
	}
}