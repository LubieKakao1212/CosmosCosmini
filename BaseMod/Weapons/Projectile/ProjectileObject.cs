using Base.Constants;
using Base.Def.Weapon;
using CosmosCosmini.Scene;
using nkast.Aether.Physics2D.Dynamics;

namespace Base.Weapons.Projectile;

public class ProjectileObject : DefinedPhysicsObject {

    protected readonly ProjectileDef _def;

    public ProjectileObject(ProjectileDef def, World world) : base(def.Physics, world) {
        _def = def;

        var pointOffset = def.ImpactPointOffset;
        var pointRadius = def.PointRadius;
        
        var fixture = PhysicsBody.CreateCircle(pointRadius, 1f, pointOffset.Construct());

        fixture.CollisionCategories = Collisions.Cats.Projectiles;
        fixture.CollidesWith = Collisions.CollidesWith.FriendlyProjectiles.With(Collisions.Cats.Projectiles, /*TODO*/false);
        fixture.IsSensor = true;
        fixture.OnCollision = (sender, other, contact) => {
            CurrentHierarchy!.RemoveObject(this);
            
            //TODO Deal Damage
            return true;
        };
        
        new AnimatedDrawableObject(_def.Sprite.Value!) {
            Parent = this,
        };
    }
}
