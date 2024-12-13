using Graphite.Abstractions.Worlds;

namespace Graphite.Abstractions;

public interface IPlayer
{
	public IClient Client { get; }

	public string Username { get; }

	public IWorld? World { get; }

	public ValueTask SpawnAsync(IWorld world);

	public void Kick(string reason);
}