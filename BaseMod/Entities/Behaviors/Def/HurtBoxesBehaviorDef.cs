using CosmosCosmini.Core.Serialization;

namespace Base.Entities.Behaviors.Def;

[SubType(typeof(EntityBehaviorDef), "hurtbox")]
public class HurtBoxesBehaviorDef : EntityBehaviorDef {

    public required List<string> ValidTags { get; init; }

    public override EntityBehavior Instantiate(Entity entity) {
        return new HurtBoxesBehavior(this, entity);
    }
}