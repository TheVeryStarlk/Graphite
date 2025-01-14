﻿namespace Graphite.Abstractions.Eventing.Sources.Listener;

public sealed class Stopping(string reason) : Event<IServer>
{
    public string Reason => reason;

    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);
}