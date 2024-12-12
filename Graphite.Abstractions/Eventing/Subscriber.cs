namespace Graphite.Abstractions.Eventing;

public sealed class Subscriber<TSource>(IDictionary<Type, Delegate> events)
{
	public void On<TEvent>(Action<TSource, TEvent> callback) where TEvent : Event<TSource>
	{
		if (!events.TryAdd(typeof(TEvent), callback))
		{
			throw new InvalidOperationException("Event is already registered.");
		}
	}
}