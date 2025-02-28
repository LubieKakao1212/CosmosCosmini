using Base.Entities.Behaviors;
using Base.Entities.Def;
using Base.Weapons;
using CosmosCosmini.Core.Math;
using CosmosCosmini.Core.Math.Def;
using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Entities.Def;
using Custom2d_Engine.Physics;
using JustLoaded.Content;

namespace Base.Def.Weapon;

[SubType(typeof(WeaponDef), "projectile")]
public class ProjectileWeaponDef : WeaponDef {

    public required DatabaseReference<EntityDef> Projectile { get; init; }

    public required SamplerDef LaunchVelocity { get; init; }

    public SamplerDef SpawnOffset { get; init; } = new ConstSamplerDef();

    public override WeaponInstance InstantiateWeapon(WeaponsBehavior ownerObject, AttachmentPoint attachmentPoint) {
        return new ProjectileWeapon(this, ownerObject, attachmentPoint);
    }
}