using Base.Def.Weapon;
using CosmosCosmini;
using Custom2d_Engine.Physics;

namespace Base.Weapons;

public class HitscanWeapon(HitscanWeaponDef def, PhysicsBodyObject ownerObject, AttachmentPoint attachmentPoint) : WeaponInstance<HitscanWeaponDef>(def, ownerObject, attachmentPoint) {
    
    protected override void DoShoot2(float globalDirection) {
        CosmosGame.Logger.Info("Pew Pew!");
    }
    
}