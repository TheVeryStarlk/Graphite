using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Graphite.Hosting;

internal static class ServiceProviderExtensions
{
	public static TTo GetRequiredService<TTo, TFrom>(this IServiceProvider serviceProvider)
		where TTo : class
		where TFrom : class
	{
		return Unsafe.As<TTo>(serviceProvider.GetRequiredService<TFrom>());
	}
}