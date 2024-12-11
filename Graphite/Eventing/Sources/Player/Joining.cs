namespace Graphite.Eventing.Sources.Player;

public sealed class Joining(
	Graphite.Player player,
	byte protocolVersion,
	string username,
	string verificationKey,
	byte unused) : Event<Graphite.Player>(player)
{
	public byte ProtocolVersion => protocolVersion;

	public string Username => username;

	public string VerificationKey => verificationKey;

	public byte Unused => unused;

	public string Name { get; set; } = "Graphite";

	public string MessageOfTheDay { get; set; } = "Hello, world!";

	public byte IsOperator { get; set; }
}