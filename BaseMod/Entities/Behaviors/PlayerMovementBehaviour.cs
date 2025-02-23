using Base.Entities.Behaviors.Def;
using Microsoft.Xna.Framework;

namespace Base.Entities.Behaviors;

public class PlayerMovementBehaviour(PlayerMovementBehaviourDef def, Entity entity) : EntityBehavior<PlayerMovementBehaviourDef>(def, entity)
{
    
    public override void Construct() {
        base.Construct();

        var weapons = entity.GetOnlyBehavior<WeaponsBehavior>()!;
        
        entity.Manager.Controls.MainAction.Started += input => weapons.StartShooting("front-gun"); // TODO unhardcode id
        entity.Manager.Controls.MainAction.Canceled += input => weapons.StopShooting("front-gun"); // TODO unhardcode id
        
        entity.Manager.Controls.SecondaryAction.Started += input => weapons.StartShooting("sniper-gun"); // TODO unhardcode id
        entity.Manager.Controls.SecondaryAction.Canceled += input => weapons.StopShooting("sniper-gun"); // TODO unhardcode id
        
        // entity.Manager.Controls.MainAction.Started += input => weapons.StartShooting("boost-gun"); // TODO unhardcode id
        // entity.Manager.Controls.MainAction.Canceled += input => weapons.StopShooting("boost-gun"); // TODO unhardcode id
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        float move =  entity.Manager.Controls.Move.GetCurrentValue<float>();
        float rotate = entity.Manager.Controls.Rotate.GetCurrentValue<float>();
        
        entity.PhysicsBody.ApplyForce(def.EnginePower*move*entity.Transform.Up);
        entity.PhysicsBody.ApplyTorque(def.RotationTorque*rotate);
    }
}