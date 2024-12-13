using Graphite.Abstractions.Eventing;
using Microsoft.Extensions.Logging;

namespace Graphite.Eventing;

internal sealed class EventDispatcher
{
	private readonly ILogger<EventDispatcher> logger;
	private readonly IDictionary<Type, Delegate> events;

	public EventDispatcher(ILogger<EventDispatcher> logger, Controller controller)
	{
		this.logger = logger;

		var registry = new Registry();
		controller.Register(registry);

		events = registry.Events;
	}

	public async Task<T> DispatchAsync<T>(T original, CancellationToken cancellationToken)
	{
		if (!events.TryGetValue(typeof(T), out var value))
		{
			return original;
		}

		try
		{
			switch (value)
			{
				case TaskDelegate<T> @delegate:
					await @delegate(original, cancellationToken).ConfigureAwait(false);
					break;

				case Action<T> action:
					action(original);
					break;
			}
		}
		catch (Exception exception)
		{
			logger.LogError(exception, "An exception occurred while running event.");
			events.Remove(typeof(T));
		}

		return original;
	}
}