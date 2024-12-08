using Microsoft.Extensions.DependencyInjection;

namespace Graphite.Hosting;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGraphite(this IServiceCollection services)
	{
		return services;
	}
}