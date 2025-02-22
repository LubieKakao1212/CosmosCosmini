using Base.Entities;
using Base.Entities.Behavior;
using CosmosCosmini.Core.Def;

namespace Base.Def.Entities.Behaviors;

public abstract class EntityBehaviorDef : PolymorphicDef {

    public abstract EntityBehavior Instantiate(Entity entity);


}