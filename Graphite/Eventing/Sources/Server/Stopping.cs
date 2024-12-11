namespace Graphite.Eventing.Sources.Server;

public sealed class Stopping(Graphite.Server server, string reason) : Event<Graphite.Server>(server)
{
	public string Reason => reason;

	public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);
}