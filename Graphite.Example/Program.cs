using Graphite;
using Graphite.Eventing.Sources.Client;
using Graphite.Hosting;
using Graphite.Networking;
using Graphite.Networking.Packets.Ingoing;
using Graphite.Networking.Packets.Outgoing;
using Graphite.Worlds;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.Logging.SetMinimumLevel(LogLevel.Trace);

builder.Services.AddGraphite();

var host = builder.Build();

var world = new World("Default");

host.UseSubscriber<Client>()
	.On<ReceivedPacket>(async (original, _) =>
	{
		switch (original.Packet)
		{
			case PlayerIdentificationPacket:
				var parts = world.Serialize();
				var packets = new IOutgoingPacket[parts.Length];

				for (var index = 0; index < packets.Length; index++)
				{
					packets[index] = new WorldPacket
					{
						Blocks = parts[index],
						PercentComplete = (byte) (index / (float) (packets.Length - 1) * 100)
					};
				}

				await original.Source.WriteAsync(
				[
					new ServerIdentificationPacket
					{
						Name = "Graphite",
						MessageOfTheDay = "Hello, world!",
						IsOperator = 0
					},
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
						Username = "Starlk",
						X = 64,
						Y = 64,
						Z = 64,
						Yaw = 0,
						Pitch = 0
					}
				]);
				break;
		}
	});


host.Run();