using Graphite.Abstractions.Eventing;
using Microsoft.Extensions.Logging;

namespace Graphite.Eventing;

internal sealed class EventDispatcher
{
    private readonly ILogger<EventDispatcher> logger;
    private readonly IDictionary<Type, Delegate> events;

    public EventDispatcher(ILogger<EventDispatcher> logger, Controller controller)
    {
        this.logger = logger;

        var registry = new Registry();
        controller.Register(registry);

        events = registry.Events;
    }

    public async Task<TEvent> DispatchAsync<TEvent, TSource>(TSource source, TEvent original, CancellationToken cancellationToken)
    {
        if (!events.TryGetValue(typeof(TEvent), out var value))
        {
            return original;
        }

        try
        {
            switch (value)
            {
                case TaskDelegate<TSource, TEvent> @delegate:
                    await @delegate(source, original, cancellationToken).ConfigureAwait(false);
                    break;

                case Action<TSource, TEvent> action:
                    action(source, original);
                    break;
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An exception occurred while running event.");
            events.Remove(typeof(TEvent));
        }

        return original;
    }
}