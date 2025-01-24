using Base.Def.Weapon;
using CosmosCosmini.Core.Def;
using CosmosCosmini.Scene;
using Custom2d_Engine.Physics;

namespace Base.Weapons;

public class ProjectileWeapon(ProjectileWeaponDef def, PhysicsBodyObject ownerObject, AttachmentPoint attachmentPoint) : WeaponInstance<ProjectileWeaponDef>(def, ownerObject, attachmentPoint) {
    
    protected override void DoShoot() {
        var def = new PhysicsDef() {
            Mass = new PhysicsDef.MassDataDef() {
                Mass = 0.1f,
            },
            LinearDrag = 0,
            Fixtures = new [] {
                new PhysicsDef.FixtureDef {
                    Type = PhysicsDef.FixtureDef.ShapeType.Rectangle,
                    Radius = 0.1f,
                    HorizontalRatio = 0.1f
                }
            }
        };

        var owner = _ownerObject;
        var ownerRotation = owner.Transform.GlobalRotation;
        
        var projectileRotation = attachmentPoint.Def.Direction + ownerRotation;
        
        var localPos = attachmentPoint.Def.LocalPosition.Construct();
        localPos.Rotate(ownerRotation);
        
        var projectile = new DefinedPhysicsObject(def, owner.PhysicsBody.World) {
            Transform = {
                GlobalRotation = projectileRotation,
                GlobalPosition = owner.Transform.GlobalPosition + localPos
            }
        };
        // projectile.CreateDrawableChild(Def.Projectile.Value!.SpriteTmp.Value!, localScale: new Vector2(0.5f, 0.05f));
        // projectile.PhysicsBody.LinearVelocity = projectile.Transform.Up * 10f;
        new AnimatedDrawableObject(Def.Projectile.Value!.Sprite.Value!) {
            Parent = projectile
        };

        var v = projectile.Transform.Up;
        projectile.PhysicsBody.LinearVelocity = owner.PhysicsBody.LinearVelocity + v * Def.LaunchVelocity;
        
        owner.CurrentHierarchy!.AddObject(projectile);
    }
}