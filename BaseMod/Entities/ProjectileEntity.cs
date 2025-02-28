using Base.Entities.Def;
using Base.Entities.Interfaces;
using CosmosCosmini.Entities;
using CosmosCosmini.Entities.Def;
using nkast.Aether.Physics2D.Dynamics;

namespace Base.Entities;

public class ProjectileEntity(EntityDef def, World world, EntityManager manager) : Entity(def, world, manager), IImpactDamageSource {

    public int Damage { get; set; }

    public int HandleImpact(Fixture cause, Fixture victim, Entity victimEntity) {
        CurrentHierarchy?.RemoveObject(this);
        return Damage;
    }

    protected override void Construct() {
        base.Construct();

        foreach (var fixture in PhysicsBody.FixtureList) {
            fixture.IsSensor = true;
        }
    }
}