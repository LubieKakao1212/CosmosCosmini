using Base.Entities;
using Base.Entities.Behaviors;
using CosmosCosmini.Core.Serialization;

namespace Base.Def.Entities.Behaviors;

[SubType(typeof(EntityBehaviorDef), "simple-ai")]
public class SimpleAiBehaviorDef : EntityBehaviorDef {
    public override EntityBehavior Instantiate(Entity entity) {
        return new SimpleAiBehavior(this, entity);
    }
}