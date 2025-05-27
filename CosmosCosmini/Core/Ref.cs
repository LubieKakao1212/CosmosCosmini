namespace CosmosCosmini.Core;

/// <summary>
/// A reference to an object which is valid only for one version
/// </summary>
/// <param name="value">cannot be null</param>
/// <typeparam name="T"></typeparam>
public class VRef<T>(T value) where T : class, IRefVersion {
    private readonly ulong _version = value.Version;

    /// <summary>
    /// Returns null only when version in different from initial
    /// </summary>
    public T? Value {
        get {
            if (value.Version == _version) {
                return value;
            }
            else {
                return null;
            }
        }
    }
}

public interface IRefVersion {
    public ulong Version { get; protected set; }

    public void IncrementVersion() {
        Version++;
    }
}