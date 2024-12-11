namespace Graphite.Eventing;

public abstract record Event<T>(T Source);