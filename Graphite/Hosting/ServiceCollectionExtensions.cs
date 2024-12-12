using Graphite.Abstractions;
using Graphite.Abstractions.Eventing;
using Graphite.Abstractions.Worlds;
using Graphite.Eventing;
using Graphite.Worlds;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Graphite.Hosting;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGraphite<T>(this IServiceCollection services) where T : Controller
	{
		services.AddHostedService<WorkerService>();

		services.AddSingleton<Controller, T>();
		services.AddSingleton<IWorldContainer, WorldContainer>();
		services.AddSingleton<EventDispatcher>();
		services.AddSingleton<IPlayerStore, PlayerStore>();

		services.AddTransient<Listener>();

		services.AddTransient<IConnectionListenerFactory>(provider => new SocketTransportFactory(
			Options.Create(new SocketTransportOptions()),
			provider.GetRequiredService<ILoggerFactory>()));

		services.AddTransient<Func<ConnectionContext, Client>>(provider =>
			connection => new Client(
				provider.GetRequiredService<ILogger<Client>>(),
				connection,
				provider.GetRequiredService<PlayerStore>()));

		return services;
	}
}