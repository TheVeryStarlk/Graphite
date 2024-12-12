namespace Graphite.Abstractions.Eventing;

public delegate Task TaskDelegate<in TEvent>(TEvent original, CancellationToken cancellationToken);