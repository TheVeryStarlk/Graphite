namespace Graphite.Eventing;

public delegate Task TaskDelegate<in TEvent>(TEvent original, CancellationToken cancellationToken);