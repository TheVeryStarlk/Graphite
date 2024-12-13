using Graphite.Abstractions;
using Graphite.Abstractions.Networking;
using Graphite.Abstractions.Networking.Packets.Outgoing;
using Graphite.Abstractions.Worlds;
using Graphite.Worlds;

namespace Graphite;

internal sealed class Player(Client client, string username) : IPlayer
{
    public IClient Client => client;

    public string Username => username;

    public IWorld? World { get; private set; }

    public async ValueTask SpawnAsync(
        IWorld world,
        float x,
        float y,
        float z,
        byte yaw,
        byte pitch)
    {
        World = world;

        var parts = ((World) world).Serialize();
        var packets = new IOutgoingPacket[parts.Length];

        for (var index = 0; index < packets.Length; index++)
        {
            packets[index] = new WorldPacket
            {
                Blocks = parts[index],
                PercentComplete = (byte) (index / (float) (packets.Length - 1) * 100)
            };
        }

        await Client.WriteAsync(
        [
            .. packets,
            new WorldInitializePacket(),
            new WorldFinalizePacket
            {
                Width = world.Width,
                Height = world.Height,
                Length = world.Length
            },
            new SpawnPlayerPacket
            {
                Identifier = 0xFF,
                Username = Username,
                X = x,
                Y = y,
                Z = z,
                Yaw = yaw,
                Pitch = pitch
            }
        ]).ConfigureAwait(false);
    }

    public void Kick(string reason)
    {
        client.Stop(reason);
    }
}