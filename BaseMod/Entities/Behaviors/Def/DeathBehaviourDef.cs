using CosmosCosmini.Core.Serialization;

namespace Base.Entities.Behaviors.Def;

[SubType(typeof(EntityBehaviorDef), "death-behaviour")]

public class DeathBehaviourDef : EntityBehaviorDef
{
    public override EntityBehavior Instantiate(Entity entity)
    {
        return new DeathBehaviour(this, entity);
    }
}