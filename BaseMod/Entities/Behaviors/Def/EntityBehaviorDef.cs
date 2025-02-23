using CosmosCosmini.Core.Def;

namespace Base.Entities.Behaviors.Def;

public abstract class EntityBehaviorDef : PolymorphicDef {

    public abstract EntityBehavior Instantiate(Entity entity);


}