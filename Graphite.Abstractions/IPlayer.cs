namespace Graphite.Abstractions;

public interface IPlayer
{
	public IClient Client { get; }

	public string Name { get; }

	public void Kick(string reason);
}