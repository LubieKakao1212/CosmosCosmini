using Base.Weapons;
using CosmosCosmini.Core.Serialization;
using Custom2d_Engine.Physics;

namespace Base.Def.Weapon;

[SubType(typeof(WeaponDef), "hitscan")]
public class HitscanWeaponDef : WeaponDef {
    
    public override WeaponInstance InstantiateWeapon(PhysicsBodyObject ownerObject, AttachmentPoint attachmentPoint) {
        Console.WriteLine("Hitscan");
        return new HitscanWeapon(this, ownerObject, attachmentPoint);
    }
    
}