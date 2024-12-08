using Microsoft.Extensions.Logging;

namespace Graphite.Eventing;

public sealed class EventDispatcher(ILogger<EventDispatcher> logger)
{
	private readonly Dictionary<Type, List<(Delegate Callback, Priority Priority)>> types = [];

	public void Register(Type type, Delegate callback, Priority priority)
	{
		if (types.TryGetValue(type, out var pairs))
		{
			pairs.Add((callback, priority));
		}
		else
		{
			types.Add(type, [(callback, priority)]);
		}
	}

	public async Task<T> DispatchAsync<T>(T original, CancellationToken cancellationToken)
	{
		if (!types.TryGetValue(typeof(T), out var events))
		{
			return original;
		}

		foreach (var pair in events.OrderBy(pair => pair.Priority))
		{
			try
			{
				switch (pair.Callback)
				{
					case TaskDelegate<T> taskDelegate:
						await taskDelegate(original, cancellationToken).ConfigureAwait(false);
						break;

					case Action<T> action:
						action(original);
						break;
				}
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "An exception occurred while running event.");
				types.Remove(typeof(T));
			}
		}

		return original;
	}
}