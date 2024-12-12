namespace Graphite.Abstractions.Worlds;

public interface IWorldContainer
{
	public IReadOnlyDictionary<string, IWorld> Worlds { get; }

	public void Create(string name);
}