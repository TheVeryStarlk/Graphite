namespace Graphite.Eventing;

public sealed class Subscriber<TSource>(EventDispatcher eventDispatcher)
{
	public Subscriber<TSource> On<TEvent>(
		TaskDelegate<TEvent> callback,
		Priority priority = Priority.Normal) where TEvent : Event<TSource>
	{
		eventDispatcher.Register(typeof(TEvent), callback, priority);
		return this;
	}

	public Subscriber<TSource> On<TEvent>(
		Action<TEvent> callback,
		Priority priority = Priority.Normal) where TEvent : Event<TSource>
	{
		eventDispatcher.Register(typeof(TEvent), callback, priority);
		return this;
	}
}