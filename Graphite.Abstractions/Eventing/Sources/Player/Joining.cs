namespace Graphite.Abstractions.Eventing.Sources.Player;

public sealed class Joining(
    byte protocolVersion,
    string username,
    string verificationKey,
    byte unused) : Event<IPlayer>
{
    public byte ProtocolVersion => protocolVersion;

    public string Username => username;

    public string VerificationKey => verificationKey;

    public byte Unused => unused;

    public string Name { get; set; } = "Graphite";

    public string MessageOfTheDay { get; set; } = "Hello, world!";

    public byte IsOperator { get; set; }
}