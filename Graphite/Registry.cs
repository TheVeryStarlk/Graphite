namespace Graphite;

public sealed class Registry
{
	public IDictionary<Type, Delegate> Events { get; } = new Dictionary<Type, Delegate>();

	public void For<T>(Action<Subscriber<T>> configure)
	{
		var subscriber = new Subscriber<T>(Events);
		configure(subscriber);
	}
}