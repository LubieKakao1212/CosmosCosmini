using Base.Weapons;
using CosmosCosmini.Core.Serialization;
using Custom2d_Engine.Physics;
using JustLoaded.Content;

namespace Base.Def.Weapon;

[SubType(typeof(WeaponDef), "projectile")]
public class ProjectileWeaponDef : WeaponDef {

    public required DatabaseReference<ProjectileDef> Projectile { get; init; }

    public required float LaunchVelocity { get; set; }

    public override WeaponInstance InstantiateWeapon(PhysicsBodyObject ownerObject, AttachmentPoint attachmentPoint) {
        Console.WriteLine("Projectile");
        return new ProjectileWeapon(this, ownerObject, attachmentPoint);
    }
}