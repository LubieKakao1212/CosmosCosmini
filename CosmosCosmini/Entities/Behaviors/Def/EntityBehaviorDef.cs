using CosmosCosmini.Core.Def;

namespace CosmosCosmini.Entities.Behaviors.Def;

public abstract class EntityBehaviorDef : PolymorphicDef {

    public abstract EntityBehavior Instantiate(Entity entity);


}