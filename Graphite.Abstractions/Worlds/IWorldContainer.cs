namespace Graphite.Abstractions.Worlds;

public interface IWorldContainer
{
	public IReadOnlyDictionary<string, IWorld> Worlds { get; }

	public void Create(string name, short width = 128, short height = 64, short length = 128);
}