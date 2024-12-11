namespace Graphite.Eventing.Sources.Player;

public sealed record Joining(
	Graphite.Player Player,
	byte ProtocolVersion,
	string Username,
	string VerificationKey,
	byte Unused) : Event<Graphite.Player>(Player)
{
	public string Name { get; set; } = "Graphite";

	public string MessageOfTheDay { get; set; } = "Hello, world!";

	public byte IsOperator { get; set; }
}