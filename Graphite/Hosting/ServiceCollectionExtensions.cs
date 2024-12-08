using Graphite.Eventing;
using Microsoft.Extensions.DependencyInjection;

namespace Graphite.Hosting;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGraphite(this IServiceCollection services)
	{
		services.AddSingleton<EventDispatcher>();
		return services;
	}
}