using Base.Def.Weapon.Scatter;
using Base.Entities.Behaviors;
using Base.Weapons;
using CosmosCosmini.Core.Def;
using CosmosCosmini.Core.Serialization;
using JustLoaded.Loading;

namespace Base.Def.Weapon;

[Def("wepo", SearchDir = "ship")]
[CreateDb("weapon")]
public abstract class WeaponDef : PolymorphicDef {

    public required int Damage { get; init; }
    
    //Rounds per second
    public required TimeSpan FireRate { get; init; }
    
    //In distance units
    public double Range { get; init; } = -1;

    public float Recoil { get; init; } = 0;

    //TODO convert to general purpose random
    public WeaponScatterDef Scatter { get; init; } = new IdentityScatterDef();

    public abstract WeaponInstance InstantiateWeapon(WeaponsBehavior ownerObject, AttachmentPoint attachmentPoint);

}