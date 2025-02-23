using CosmosCosmini.Core.Serialization;

namespace Base.Entities.Behaviors.Def;

[SubType(typeof(EntityBehaviorDef), "hp-behaviour")]
public class HealthBehaviourDef : EntityBehaviorDef
{
    public required int MaxHp { get; init; }
    public override EntityBehavior Instantiate(Entity entity)
    {
        return new HealthBehaviour(this, entity);
        //throw new NotImplementedException();
    }
}