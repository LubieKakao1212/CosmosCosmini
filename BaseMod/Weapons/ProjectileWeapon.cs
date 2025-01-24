using Base.Def.Weapon;
using CosmosCosmini.Core.Def;
using CosmosCosmini.Scene;
using Custom2d_Engine.Physics;
using Custom2d_Engine.Scenes.Factory;
using Microsoft.Xna.Framework;

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
                    Radius = 0.5f,
                    HorizontalRatio = 0.1f
                }
            }
        };

        var owner = _ownerObject;

        var projectile = new DefinedPhysicsObject(def, owner.PhysicsBody.World) {
            Transform = {
                GlobalRotation = owner.Transform.GlobalRotation,
                GlobalPosition = owner.Transform.GlobalPosition + attachmentPoint.Def.LocalPosition.Construct()
            }
        };
        projectile.CreateDrawableChild(Def.Projectile.Value!.SpriteTmp.Value!, localScale: new Vector2(0.5f, 0.05f));
        projectile.PhysicsBody.LinearVelocity = projectile.Transform.Up * 10f;
        
        owner.CurrentHierarchy!.AddObject(projectile);
    }
    
}