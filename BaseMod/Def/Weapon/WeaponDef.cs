using Base.Weapons;
using CosmosCosmini.Core.Def;
using CosmosCosmini.Core.Serialization;
using Custom2d_Engine.Physics;
using JustLoaded.Loading;
using YamlDotNet.Serialization;

namespace Base.Def.Weapon;

[Def("wepo", SearchDir = "ship")]
[CreateDb("weapon")]
public abstract class WeaponDef : PolymorphicDef {

    public required long Damage { get; init; }
    
    //Rounds per second
    public required double Rps { get; init; }
    
    //In distance units
    public double Range { get; init; } = -1;
    
    public abstract WeaponInstance InstantiateWeapon(PhysicsBodyObject ownerObject, AttachmentPoint attachmentPoint);

}