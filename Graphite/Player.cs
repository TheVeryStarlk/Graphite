using System.IO.Compression;
using Graphite.Networking;
using Graphite.Networking.Packets.Outgoing;
using Graphite.Worlds;

namespace Graphite;

public sealed class Player(Client client, Server server, string username)
{
	public Client Client => client;

	public Server Server => server;

	public string Username => username;

	public World? World { get; private set; }

	public float X { get; internal set; }

	public float Y { get; internal set; }

	public float Z { get; internal set; }

	public byte Yaw { get; internal set; }

	public byte Pitch { get; internal set; }

	public async ValueTask SpawnAsync(World world)
	{
		World = world;

		using var result = new MemoryStream();
		await using var compression = new GZipStream(result, CompressionMode.Compress);

		compression.WriteInteger(world.Blocks.Count());
		compression.Write(world.Blocks.Cast<byte>().ToArray());

		compression.Close();

		var parts = result.ToArray().Chunk(1024).ToArray();
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
				X = X,
				Y = Y,
				Z = Z,
				Yaw = Yaw,
				Pitch = Pitch
			}
		]).ConfigureAwait(false);
	}
}