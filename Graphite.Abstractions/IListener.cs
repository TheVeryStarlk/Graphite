namespace Graphite.Abstractions;

public interface IListener
{
	public void Stop(string reason);
}