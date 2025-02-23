using Base.Def.Weapon;
using Base.Entities;
using Base.Entities.Behaviors;
using CosmosCosmini.Core.Math;
using Custom2d_Engine.Ticking;

namespace Base.Weapons;

public class ProjectileWeapon(ProjectileWeaponDef def, WeaponsBehavior ownerBehavior, AttachmentPoint attachmentPoint) : WeaponInstance<ProjectileWeaponDef>(def, ownerBehavior, attachmentPoint) {

    protected ISampler Velocity = def.LaunchVelocity.Instantiate();
    protected ISampler Offset = def.SpawnOffset.Instantiate();
    
    protected override void DoShoot2(float globalDirection) {
        var entity = _ownerBehavior.entity;

        var projectileDef = Def.Projectile.Value!;
        
        var projectileRotation = globalDirection;
        var projectilePos = attachmentPoint.Def.LocalPosition.Construct();
        projectilePos = entity.Transform.LocalToWorld.TransformPoint(projectilePos);
        var projectileEntity = entity.Manager.CreateEntity(projectileDef, projectilePos);
        
        projectileEntity.Transform.GlobalRotation = projectileRotation;
        var up = projectileEntity.Transform.Up;
        projectileEntity.Transform.GlobalPosition += up * Offset.Next();
        
        projectileEntity.PhysicsBody.LinearVelocity = up * Velocity.Next() + entity.PhysicsBody.LinearVelocity;
        
        if (projectileEntity is ProjectileEntity proj) {
            proj.Damage = Def.Damage;
        }
        
        IEnumerator<TimeSpan> Sequence() {
            yield return TimeSpan.FromSeconds(1f);
            projectileEntity.CurrentHierarchy!.RemoveObject(projectileEntity);
        }
        
        projectileEntity.AddActionSequence(Sequence());
        entity.CurrentHierarchy!.AddObject(projectileEntity);
    }
}