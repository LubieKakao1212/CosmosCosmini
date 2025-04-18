using System.Diagnostics.CodeAnalysis;
using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Entities;
using CosmosCosmini.Entities.Behaviors;
using CosmosCosmini.Entities.Behaviors.Def;
using Custom2d_Engine.Input;
using Microsoft.Xna.Framework;

namespace Base.Entities.Behaviors;

public class PlayerMovementBehaviour(PlayerMovementBehaviourDef def, Entity entity) : EntityBehavior<PlayerMovementBehaviourDef>(def, entity) {

    [NotNull] private WeaponsBehavior? Weapons { get; set; }

    [NotNull] private Action<IInput>[]? ShootActions { get; set; }

    public override void Construct(bool first) {
        base.Construct(first);
        if (first) {
            Weapons = entity.GetOnlyBehavior<WeaponsBehavior>();
            
            ShootActions = [
                new Action<IInput>(input => Weapons.StartShooting("front-gun")), // TODO unhardcode id
                new Action<IInput>(input => Weapons.StopShooting("front-gun")),  // TODO unhardcode id
                
                new Action<IInput>(input => Weapons.StartShooting("sniper-gun")),// TODO unhardcode id
                new Action<IInput>(input => Weapons.StopShooting("sniper-gun")), // TODO unhardcode id
                
                new Action<IInput>(input => Weapons.StartShooting("boost-gun")), // TODO unhardcode id
                new Action<IInput>(input => Weapons.StopShooting("boost-gun"))   // TODO unhardcode id
            ];
        }
    }

    public override void OnSpawn() {
        base.OnSpawn();
        
        entity.Manager.Controls.MainAction.Started += ShootActions[0];
        entity.Manager.Controls.MainAction.Canceled += ShootActions[1];
        entity.Manager.Controls.SecondaryAction.Started += ShootActions[2];
        entity.Manager.Controls.SecondaryAction.Canceled += ShootActions[3];
        entity.Manager.Controls.Boost.Started += ShootActions[4];
        entity.Manager.Controls.Boost.Canceled += ShootActions[5];
    }

    public override void OnDespawn() {
        base.OnDespawn();
        
        entity.Manager.Controls.MainAction.Started -= ShootActions[0];
        entity.Manager.Controls.MainAction.Canceled -= ShootActions[1];
        entity.Manager.Controls.SecondaryAction.Started -= ShootActions[2];
        entity.Manager.Controls.SecondaryAction.Canceled -= ShootActions[3];
        entity.Manager.Controls.Boost.Started -= ShootActions[4];
        entity.Manager.Controls.Boost.Canceled -= ShootActions[5];
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

[SubType(typeof(EntityBehaviorDef), "player-movement-behaviour")]
public class PlayerMovementBehaviourDef : EntityBehaviorDef
{
    public required float EnginePower { get; init; } 
    public required float RotationTorque { get; init; }
    public override EntityBehavior Instantiate(Entity entity)
    {
        return new PlayerMovementBehaviour(this, entity);
    }
}