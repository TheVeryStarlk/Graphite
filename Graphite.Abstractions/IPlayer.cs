using Graphite.Abstractions.Worlds;

namespace Graphite.Abstractions;

public interface IPlayer
{
    public IServer Server { get; }

    public IClient Client { get; }

    public IWorld? World { get; }

    public string Username { get; }

    public ValueTask SpawnAsync(
        IWorld world,
        float x = 0,
        float y = 0,
        float z = 0,
        byte yaw = 0,
        byte pitch = 0);

    public ValueTask SendMessageAsync(string message);

    public void Kick(string reason);
}