using Base.Entities.Damage;
using Base.Entities.Interfaces;
using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Entities;
using CosmosCosmini.Entities.Behaviors;
using CosmosCosmini.Entities.Behaviors.Def;
using CosmosCosmini.Entities.Tags;
using JustLoaded.Content;

namespace Base.Entities.Behaviors;

public class HurtRaycastBehavior(HurtRaycastBehaviorDef def, Entity entity) : RaycastBehaviourBase<HurtRaycastBehaviorDef>(def, entity) {
    
    public override bool IsValidHit(in RaycastHit hit) {
        return ((FixtureTag)hit.fixture.Tag).CompanionsOfType<IHurtResponder>().FirstOrDefault() != null;
    }

    public override void DoHit(in RaycastHit hit) {
        foreach (var responder in ((FixtureTag)hit.fixture.Tag).CompanionsOfType<IHurtResponder>()) {
            responder.OnHurt(new DamageInstance {
                Amount = def.BaseDamage, //TODO add multiplier
                DirectSource = entity,
                IndirectSource = entity, // TODO Change to proper source
                Type = def.DamageType.Value ?? throw new Exception("TODO") // TODO
            });
        }

        if (def.DespawnOnHit) {
            entity.DespawnAndReturn();
        }
    }
}

[SubType(typeof(EntityBehaviorDef), "motion-hurt")]
public class HurtRaycastBehaviorDef : RaycastBehaviourDef {

    public required int BaseDamage { get; init; }

    public bool DespawnOnHit { get; init; } = false;

    public required DatabaseReference<DamageType> DamageType { get; init; }

    public override EntityBehavior Instantiate(Entity entity) {
        return new HurtRaycastBehavior(this, entity);
    }
    
}