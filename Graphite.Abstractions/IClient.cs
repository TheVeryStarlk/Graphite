namespace Graphite.Abstractions;

public interface IClient
{
	public void Stop(string reason);
}