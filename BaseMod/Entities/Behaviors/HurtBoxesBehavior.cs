using System.Diagnostics.CodeAnalysis;
using Base.Entities.Damage;
using Base.Entities.Interfaces;
using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Entities;
using CosmosCosmini.Entities.Behaviors;
using CosmosCosmini.Entities.Behaviors.Def;
using CosmosCosmini.Entities.Tags;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace Base.Entities.Behaviors;

public class HurtBoxesBehavior(HurtBoxesBehaviorDef def, Entity entity) : EntityBehavior<HurtBoxesBehaviorDef>(def, entity) {

    private readonly List<DamageInstance> _damageList = new();

    [NotNull] private HealthBehaviour? Health { get; set; }
    
    public override void Construct(bool first) {
        base.Construct(first);
        if (first) {
            Health = entity.GetOnlyBehavior<HealthBehaviour>() ?? throw new ApplicationException($"{nameof(HurtBoxesBehavior)} requires {nameof(HealthBehaviour)}");

            var hurtResponder = new DamageHurtResponder { HurtBoxesBehavior = this };
            
            foreach (var fixture in entity.PhysicsBody.FixtureList) {
                var tag = (FixtureTag) fixture.Tag;
                bool flag = false;
                foreach (var validTag in def.ValidTags) {
                    flag |= tag.HasKeyword(validTag);
                }
                
                if (flag) {
                    tag.AddCompanion(hurtResponder);
                }
            }
        }
    }
    
    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        foreach (var damage in _damageList) {
            Health.ReceiveDamage(damage.Amount);
        }
        _damageList.Clear();
    }

    public class DamageHurtResponder : IHurtResponder {

        public required HurtBoxesBehavior HurtBoxesBehavior { private get; init; }

        public void OnHurt(in DamageInstance damage) {
            HurtBoxesBehavior._damageList.Add(damage);
        }
    }
}

[SubType(typeof(EntityBehaviorDef), "hurtbox")]
public class HurtBoxesBehaviorDef : EntityBehaviorDef {

    public required List<string> ValidTags { get; init; }

    public override EntityBehavior Instantiate(Entity entity) {
        return new HurtBoxesBehavior(this, entity);
    }
}