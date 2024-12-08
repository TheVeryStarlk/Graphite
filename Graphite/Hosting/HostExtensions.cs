using Graphite.Eventing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Graphite.Hosting;

public static class HostExtensions
{
	public static Subscriber<T> UseSubscriber<T>(this IHost host)
	{
		var eventDispatcher = host.Services.GetRequiredService<EventDispatcher>();
		return new Subscriber<T>(eventDispatcher);
	}
}