using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Entities;
using CosmosCosmini.Entities.Def;
using nkast.Aether.Physics2D.Dynamics;

namespace Base.Entities.Def;

[SubType(typeof(EntityDef), "projectile")]
public class ProjectileEntityDef : EntityDef {
    
    public override Entity Instantiate(World world, EntityManager manager) {
        return new ProjectileEntity(this, world, manager);
    }
}