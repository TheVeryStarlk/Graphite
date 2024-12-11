namespace Graphite.Eventing.Sources.Server;

public sealed record Stopping(Graphite.Server Server, string Reason) : Event<Graphite.Server>(Server)
{
	public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);
}