namespace Graphite.Eventing;

public sealed class Subscriber<TSource>(EventDispatcher eventDispatcher)
{
	public Subscriber<TSource> On<TEvent>(TaskDelegate<TEvent> callback) where TEvent : Event<TSource>
	{
		eventDispatcher.Register(typeof(TEvent), callback);
		return this;
	}

	public Subscriber<TSource> On<TEvent>(Action<TEvent> callback) where TEvent : Event<TSource>
	{
		eventDispatcher.Register(typeof(TEvent), callback);
		return this;
	}
}