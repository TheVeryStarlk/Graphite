using Graphite.Abstractions;
using Graphite.Abstractions.Eventing.Sources.Player;
using Graphite.Abstractions.Networking;
using Graphite.Abstractions.Networking.Packets.Ingoing;
using Graphite.Abstractions.Networking.Packets.Outgoing;
using Graphite.Eventing;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using System.Buffers;
using System.Threading.Channels;

namespace Graphite;

internal sealed class Client(
	ILogger<Client> logger,
	PlayerStore playerStore,
	EventDispatcher eventDispatcher,
	ConnectionContext connection,
	byte identifier) : IClient, IDisposable
{
	public byte Identifier => identifier;

	private readonly Channel<IPacket> ingoing = Channel.CreateUnbounded<IPacket>();
	private readonly Channel<IOutgoingPacket> outgoing = Channel.CreateUnbounded<IOutgoingPacket>();
	private readonly CancellationTokenSource source = CancellationTokenSource.CreateLinkedTokenSource(connection.ConnectionClosed);

	private string reason = "No reason specified.";
	private Player? player;

	public Task StartAsync()
	{
		return Task.WhenAll(HandlingAsync(), ReadingAsync(), WritingAsync());
	}

	public async ValueTask WriteAsync(params IOutgoingPacket[] packets)
	{
		foreach (var packet in packets)
		{
			await outgoing.Writer.WriteAsync(packet).ConfigureAwait(false);
		}
	}

	private async Task HandlingAsync()
	{
		try
		{
			await foreach (var packet in ingoing.Reader.ReadAllAsync(source.Token).ConfigureAwait(false))
			{
				switch (packet)
				{
					case PlayerIdentificationPacket current:
						player = new Player(this, current.Username);

						playerStore.Add(player);

						var joining = new Joining(
							current.ProtocolVersion,
							current.Username,
							current.VerificationKey,
							current.Unused);

						joining = await eventDispatcher.DispatchAsync(joining, source.Token).ConfigureAwait(false);

						var identification = new ServerIdentificationPacket
						{
							Name = joining.Name,
							MessageOfTheDay = joining.MessageOfTheDay,
							IsOperator = joining.IsOperator,
						};

						await outgoing.Writer.WriteAsync(identification).ConfigureAwait(false);
						break;
				}
			}
		}
		catch (Exception exception) when (exception is OperationCanceledException or ConnectionResetException)
		{
			// Nothing.
		}
		catch (Exception exception)
		{
			logger.LogError(exception, "Unexpected exception while handling client");
		}

		playerStore.Remove(player?.Username);

		await eventDispatcher.DispatchAsync(new Leaving(), source.Token).ConfigureAwait(false);

		// We can't use the write method since the write loop has finished,
		// hence we need to manually send the disconnect packet here.
		try
		{
			var packet = new DisconnectPacket
			{
				Reason = reason
			};

			// Disconnect packet's size is 64 without the type byte.
			// So in total the whole payload is 65.
			Span<byte> buffer = stackalloc byte[65];

			Protocol.Write(buffer, packet);

			connection.Transport.Output.Write(buffer);

			await connection.Transport.Output.FlushAsync().ConfigureAwait(false);

			// Give the client a time span of a full Minecraft tick before we abort the connection.
			await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
		}
		catch
		{
			// Nothing to do here.
		}

		connection.Abort(new ConnectionAbortedException(reason));

		await connection.DisposeAsync().ConfigureAwait(false);
	}

	private async Task ReadingAsync()
	{
		while (!source.IsCancellationRequested)
		{
			try
			{
				var result = await connection.Transport.Input.ReadAsync(source.Token).ConfigureAwait(false);

				var buffer = result.Buffer;
				var consumed = buffer.Start;
				var examined = buffer.End;

				try
				{
					if (Protocol.TryRead(ref buffer, out var packet))
					{
						consumed = buffer.Start;
						examined = consumed;

						await ingoing.Writer.WriteAsync(packet).ConfigureAwait(false);
					}

					if (result.IsCompleted)
					{
						// See if it is the right decision to throw or to break and return default.
						throw new OperationCanceledException();
					}
				}
				finally
				{
					connection.Transport.Input.AdvanceTo(consumed, examined);
				}
			}
			catch (Exception exception) when (exception is OperationCanceledException or ConnectionResetException)
			{
				break;
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "Unexpected exception while reading from client");
				break;
			}
		}
	}

	private async Task WritingAsync()
	{
		try
		{
			await foreach (var packet in outgoing.Reader.ReadAllAsync(source.Token).ConfigureAwait(false))
			{
				var total = packet.Length + 1;

				Protocol.Write(connection.Transport.Output.GetSpan(total), packet);

				connection.Transport.Output.Advance(total);

				await connection.Transport.Output.FlushAsync(source.Token).ConfigureAwait(false);
			}
		}
		catch (Exception exception) when (exception is OperationCanceledException or ConnectionResetException)
		{
			// Nothing.
		}
		catch (Exception exception)
		{
			logger.LogError(exception, "Unexpected exception while writing to client");
		}
	}

	public void Stop(string stop)
	{
		reason = stop;
		source.Cancel();
	}

	public void Dispose()
	{
		source.Dispose();
	}
}