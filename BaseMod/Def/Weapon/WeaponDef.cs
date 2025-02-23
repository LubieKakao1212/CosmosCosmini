using Base.Entities.Behaviors;
using Base.Weapons;
using CosmosCosmini.Core.Def;
using CosmosCosmini.Core.Math;
using CosmosCosmini.Core.Math.Def;
using CosmosCosmini.Core.Serialization;
using JustLoaded.Loading;

namespace Base.Def.Weapon;

[Def("wepo", SearchDir = "weapons")]
[CreateDb("weapons")]
public abstract class WeaponDef : PolymorphicDef {

    public required int Damage { get; init; }
    
    //Rounds per second
    public required TimeSpan FireRate { get; init; }
    
    //In distance units
    public double Range { get; init; } = -1;

    public float Recoil { get; init; } = 0;

    public SamplerDef ShotsPerBurst { get; init; } = new ConstSamplerDef() { Value = 1 };
    
    //TODO convert to general purpose random
    public SamplerDef Scatter { get; init; } = new ConstSamplerDef();

    public abstract WeaponInstance InstantiateWeapon(WeaponsBehavior ownerObject, AttachmentPoint attachmentPoint);

}