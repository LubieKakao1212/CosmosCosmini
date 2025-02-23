using System.Diagnostics.CodeAnalysis;
using Base.Entities.Behaviors.Def;
using Base.Entities.Interfaces;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace Base.Entities.Behaviors;

public class HurtBoxesBehavior(HurtBoxesBehaviorDef def, Entity entity) : EntityBehavior<HurtBoxesBehaviorDef>(def, entity) {

    private readonly List<int> _damageList = new();

    [NotNull] private HealthBehaviour? Health { get; set; }

    public override void Construct() {
        base.Construct();
        Health = entity.GetOnlyBehavior<HealthBehaviour>() ?? throw new ApplicationException($"{nameof(HurtBoxesBehavior)} requires {nameof(HealthBehaviour)}");
        foreach (var fixture in entity.PhysicsBody.FixtureList) {
            if (Def.ValidTags.Contains(fixture.Tag)) {
                fixture.IsSensor = true;
                fixture.OnCollision += (sender, other, contact) => {
                    if (other.Body.Tag is Entity otherEntity) { 
                        int damage = 0;
                        foreach (var source in otherEntity.GetInterfaces<IImpactDamageSource>()) {
                            damage += source.HandleImpact(other, sender, entity);
                        }
                        OnHit(otherEntity, fixture, damage);
                    }
                    return true;
                };
            }
        }
    }

    public virtual void OnHit(Entity? directSource, Fixture fixtureHit, int damage) {
        _damageList.Add(damage);
    }
    
    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        foreach (var damage in _damageList) {
            Health.ReceiveDamage(damage);
        }
        _damageList.Clear();
    }
}