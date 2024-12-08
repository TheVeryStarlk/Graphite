namespace Graphite.Eventing;

public sealed class EventDispatcher
{
	private readonly Dictionary<Type, Delegate> events = [];

	public void Register(Type type, Delegate callback)
	{
		if (!events.TryAdd(type, callback))
		{
			throw new InvalidOperationException("Event is already registered.");
		}
	}
}