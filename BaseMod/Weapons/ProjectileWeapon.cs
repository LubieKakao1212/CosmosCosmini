using Base.Def.Weapon;
using Base.Weapons.Projectile;
using CosmosCosmini.Core.Def;
using Custom2d_Engine.Physics;
using Custom2d_Engine.Ticking;

namespace Base.Weapons;

public class ProjectileWeapon(ProjectileWeaponDef def, PhysicsBodyObject ownerObject, AttachmentPoint attachmentPoint) : WeaponInstance<ProjectileWeaponDef>(def, ownerObject, attachmentPoint) {
    
    protected override void DoShoot2(float globalDirection) {
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
        var projectileRotation = globalDirection;
        
        var projectilePos = attachmentPoint.Def.LocalPosition.Construct();
        projectilePos = owner.Transform.LocalToWorld.TransformPoint(projectilePos);

        var projectile = new ProjectileObject(Def.Projectile.Value!, owner.PhysicsBody.World) {
            Transform = {
                GlobalRotation = projectileRotation,
                GlobalPosition = projectilePos
            }
        };
        
        // var projectile = new DefinedPhysicsObject(def, owner.PhysicsBody.World) {
        //     Transform = {
        //         GlobalRotation = projectileRotation,
        //         GlobalPosition = projectilePos
        //     }
        // };
        // projectile.CreateDrawableChild(Def.Projectile.Value!.SpriteTmp.Value!, localScale: new Vector2(0.5f, 0.05f));
        // projectile.PhysicsBody.LinearVelocity = projectile.Transform.Up * 10f;

        var projectileDef = Def.Projectile.Value!;

        var v = projectile.Transform.Up;
        projectile.PhysicsBody.LinearVelocity = owner.PhysicsBody.LinearVelocity + v * Def.LaunchVelocity;

        IEnumerator<TimeSpan> Sequence() {
            yield return projectileDef.MaxLifespan;
            projectile.CurrentHierarchy!.RemoveObject(projectile);
        }
        
        projectile.AddActionSequence(Sequence());
        
        owner.CurrentHierarchy!.AddObject(projectile);
    }
}