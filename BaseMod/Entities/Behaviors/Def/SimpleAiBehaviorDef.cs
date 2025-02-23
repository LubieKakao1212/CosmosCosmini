using CosmosCosmini.Core.Serialization;

namespace Base.Entities.Behaviors.Def;

[SubType(typeof(EntityBehaviorDef), "simple-ai")]
public class SimpleAiBehaviorDef : EntityBehaviorDef {
    public override EntityBehavior Instantiate(Entity entity) {
        return new SimpleAiBehavior(this, entity);
    }
}