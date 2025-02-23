using Base.Entities;
using Base.Entities.Behaviors;
using CosmosCosmini.Core.Serialization;

namespace Base.Def.Entities.Behaviors;

[SubType(typeof(EntityBehaviorDef), "death-behaviour")]

public class DeathBehaviourDef : EntityBehaviorDef
{
    public override EntityBehavior Instantiate(Entity entity)
    {
        return new DeathBehaviour(this, entity);
    }
}