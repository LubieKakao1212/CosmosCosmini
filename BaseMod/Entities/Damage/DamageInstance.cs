using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Entities;
using JustLoaded.Content;
using JustLoaded.Loading;
using YamlDotNet.Serialization;

namespace Base.Entities.Damage;

public struct DamageInstance {

    public required Entity DirectSource { get; init; }
    public required Entity IndirectSource { get; init; }

    public required int Amount { get; init; }
    public required DamageType Type { get; init; }
    
}

[CreateDb("damage-type")]
[Def("dt", SearchDir = "damage")]
public class DamageType {

    public bool IsRoot => Parent == null;
    
    //TODO Setting to null in YAML does not work, if null is needed just don't include this property
    public DatabaseReference<DamageType>? Parent { get; init; } = null;

    public required string Message { get; init; }

    public bool IsOf(DamageType type) {
        if (type == this) {
            return true;
        }
        var res = Parent?.Value?.IsOf(type);
        return res.HasValue && res.Value;
    }
    
}