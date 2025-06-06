namespace CosmosCosmini.Entities.Tags;

public class FixtureTag {

    public required string[] Keywords { private get; init; }

    private readonly List<object> _companions = new();
    
    public bool HasKeyword(string keyword) {
        return Keywords.Contains(keyword);
    }

    public IEnumerable<T> CompanionsOfType<T>() {
        return _companions.OfType<T>();
    }
    
    public void AddCompanion(object companion) {
        if (_companions.Contains(companion)) {
            return;
        }
        _companions.Add(companion);
    }

    public void RemoveCompanion(object companion) {
        _companions.Remove(companion);
    }
    
}