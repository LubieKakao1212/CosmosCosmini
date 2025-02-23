using Base.Weapons;
using CosmosCosmini.Core.Serialization;

namespace Base.Entities.Behaviors.Def;

[SubType(typeof(EntityBehaviorDef), "weapons")]
public class WeaponsBehaviorDef : EntityBehaviorDef {

    public required Dictionary<string, AttachmentPointDef> Weapons { get; init; }

    public override EntityBehavior Instantiate(Entity entity) {
        return new WeaponsBehavior(this, entity);
    }
}