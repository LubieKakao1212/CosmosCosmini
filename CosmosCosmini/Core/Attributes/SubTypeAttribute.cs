namespace CosmosCosmini.Core.Serialization;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class SubTypeAttribute(Type baseType, string key) : Attribute {

    public string Key { get; init; } = key;
    public Type BaseType { get; init; } = baseType;
    
}