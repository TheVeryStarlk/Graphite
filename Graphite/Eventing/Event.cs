namespace Graphite.Eventing;

public abstract class Event<T>(T source)
{
	public T Source => source;
}