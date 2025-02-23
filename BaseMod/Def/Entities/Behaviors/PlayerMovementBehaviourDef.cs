using Base.Entities;
using Base.Entities.Behaviors;
using CosmosCosmini.Core.Serialization;

namespace Base.Def.Entities.Behaviors;

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