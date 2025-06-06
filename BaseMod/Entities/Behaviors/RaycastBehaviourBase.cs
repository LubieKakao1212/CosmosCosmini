using CosmosCosmini.Entities;
using CosmosCosmini.Entities.Behaviors;
using CosmosCosmini.Entities.Behaviors.Def;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace Base.Entities.Behaviors;

public abstract class RaycastBehaviourBase<TDef>(TDef def, Entity entity) : EntityBehavior<RaycastBehaviourDef>(def, entity) where TDef : RaycastBehaviourDef {

    protected TargetSelector Target { get; set; } = TargetSelector.Closest;

    public override void Update(GameTime gameTime) {
        var hits = new List<RaycastHit>();
        var pos = entity.Transform.GlobalPosition;
        entity.World.RayCast((fixture, point, normal, fraction) => {
            var hit = new RaycastHit {
                entity = (Entity)fixture.Body.Tag,
                fixture = fixture,
                point = point,
                normal = normal
            };
            var f = fraction;
            if (IsValidHit(hit)) {
                if (Target == TargetSelector.Closest) {
                    hits.Clear();
                }
                hits.Add(hit);
            }
            else {
                //Don't clip if target is not valid
                f = 1;
            }
            return Target switch {
                TargetSelector.All => 1,
                TargetSelector.Any => 0,
                TargetSelector.Closest => f,
                _ => throw new ArgumentOutOfRangeException()
            };
        }, pos, pos + GetRay());

        foreach (var hit in hits) {
            DoHit(hit);
        }
    }

    public abstract bool IsValidHit(in RaycastHit hit);
    
    public abstract void DoHit(in RaycastHit hit);

    public virtual Vector2 GetRay() {
        return entity.PhysicsBody.LinearVelocity;
    }
    
    public struct RaycastHit {
        public Entity entity;
        public Fixture fixture;
        public Vector2 point;
        public Vector2 normal;
    }

    public enum TargetSelector {
        Closest,
        All,
        Any
    }
}

public abstract class RaycastBehaviourDef : EntityBehaviorDef {

    public bool HitSelf { get; init; }

}
