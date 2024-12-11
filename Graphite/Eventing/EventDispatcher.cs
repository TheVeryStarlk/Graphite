using Microsoft.Extensions.Logging;

namespace Graphite.Eventing;

public sealed class EventDispatcher(ILogger<EventDispatcher> logger)
{
	private readonly Dictionary<Type, Delegate> events = [];

	public void Register(Type type, Delegate callback)
	{
		if (!events.TryAdd(type, callback))
		{
			throw new InvalidOperationException("Event is already registered.");
		}
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