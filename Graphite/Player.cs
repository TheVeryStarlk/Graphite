using Graphite.Abstractions;

namespace Graphite;

internal sealed class Player(string name) : IPlayer
{
	public string Name => name;
}