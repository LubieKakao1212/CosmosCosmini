namespace CosmosCosmini.Core.Serialization;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class DefAttribute(string extension) : Attribute {

    public string Extension { get; } = extension;
    public string SearchDir { get; init; } = ".";
    
}