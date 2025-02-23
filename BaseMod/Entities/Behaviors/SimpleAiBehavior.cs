using Base.Entities.Behaviors.Def;
using Base.Weapons;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Collision;

namespace Base.Entities.Behaviors;

public class SimpleAiBehavior(SimpleAiBehaviorDef def, Entity entity) : EntityBehavior<SimpleAiBehaviorDef>(def, entity) {

    private Pid _pidX = new Pid(5f, 0f, 0f, 1f);
    private Pid _pidY = new Pid(5f, 0f, 0f, 1f);

    private Entity? _target = null;
    
    public override void OnEntityAdded() {
        base.OnEntityAdded();
        _pidX.Reset();
        _pidY.Reset();
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);

        var pb = entity.PhysicsBody;

        var pos = entity.Transform.GlobalPosition;

        _target ??= FindTarget(pos);

        if (_target == null) {
            return;
        }
        
        var targetPos = _target.Transform.GlobalPosition;

        var delta = targetPos - pos;
        
        pb.ApplyForce(new Vector2(
            _pidX.Evaluate(delta.X),
            _pidY.Evaluate(delta.Y)));
            
        pb.Rotation = -MathF.Atan2(delta.X, delta.Y);
    }

    private Entity? FindTarget(Vector2 pos) {
        var pb = entity.PhysicsBody;
        var world = pb.World;
        var distanceSq = float.PositiveInfinity;
        Entity? target = null;
        world.QueryAABB(fixture => {
            var bodyHit = fixture.Body;
            if (bodyHit != pb && bodyHit.Tag is Entity entity && entity.EntityDef.Alignment == ((EntityBehavior)this).entity.EntityDef.Alignment.GetOpposite()) {
                var targetPos = bodyHit.Position;
                var distanceToTargetSq = Vector2.DistanceSquared(pos, targetPos);
                if (distanceToTargetSq < distanceSq) {
                    distanceSq = distanceToTargetSq;
                    target = entity;
                }
            }
            return true;
        }, new AABB(pos, 10f, 10f));

        return target;
        // for (int i = 0; i < 64; i++) {
        //     var i1 = (float)i / 64f;
        //     var angle = i1 * MathF.PI * 2f;
        //
        //     var forward = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        //     
        //     world.RayCast((fixture, point, normal, fraction) => );
        //     
        // }
    }
}