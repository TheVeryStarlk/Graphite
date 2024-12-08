namespace Graphite.Eventing;

public abstract class Event<T>
{
	public required T Source { get; init; }
}