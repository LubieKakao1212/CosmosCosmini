using Base.Def.Entities.Behaviors;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Common;

namespace Base.Entities.Behaviors;

public class PlayerMovementBehaviour(PlayerMovementBehaviourDef def, Entity entity) : EntityBehavior<PlayerMovementBehaviourDef>(def, entity)
{
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        float move =  _entity.Manager.Controls.Move.GetCurrentValue<float>();
        float rotate = _entity.Manager.Controls.Rotate.GetCurrentValue<float>();
        
        _entity.PhysicsBody.ApplyForce(def.EnginePower*move*_entity.Transform.Up);
        _entity.PhysicsBody.ApplyTorque(def.RotationTorque*rotate);
    }
}