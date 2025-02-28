using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Entities;
using CosmosCosmini.Entities.Behaviors;
using CosmosCosmini.Entities.Behaviors.Def;

namespace Base.Entities.Behaviors;

public class FactionAlignmentBehavior(FactionAlignmentBehaviorDef def, Entity entity) : EntityBehavior<FactionAlignmentBehaviorDef>(def, entity) {
    
}

[SubType(typeof(EntityBehaviorDef), "faction-alignment")]
public class FactionAlignmentBehaviorDef : EntityBehaviorDef {

    public required FactionAlignment Alignment { get; init; }

    public override EntityBehavior Instantiate(Entity entity) {
        return new FactionAlignmentBehavior(this, entity);
    }
}