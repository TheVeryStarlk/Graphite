namespace Graphite.Abstractions.Networking;

public interface IPacket;

public interface IIngoingPacket<out T> where T : IIngoingPacket<T>, IPacket
{
	public abstract static byte Type { get; }

	public abstract static int Length { get; }

	public abstract static T Create(SpanReader reader);
}

public interface IOutgoingPacket : IPacket
{
	public byte Type { get; }

	public int Length { get; }

	public void Write(ref SpanWriter writer);
}