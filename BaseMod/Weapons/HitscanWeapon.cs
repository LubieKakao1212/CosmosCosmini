using Base.Def.Weapon;
using Base.Entities.Behaviors;
using CosmosCosmini;
using Custom2d_Engine.Physics;

namespace Base.Weapons;

public class HitscanWeapon(HitscanWeaponDef def, WeaponsBehavior ownerBehavior, AttachmentPoint attachmentPoint) : WeaponInstance<HitscanWeaponDef>(def, ownerBehavior, attachmentPoint) {
    
    protected override void DoShoot2(float globalDirection) {
        CosmosGame.Logger.Info("Pew Pew!");
    }
    
}