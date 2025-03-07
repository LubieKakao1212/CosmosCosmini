using CosmosCosmini.Core.Def;
using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Entities.Behaviors.Def;
using CosmosCosmini.Graphics;
using JustLoaded.Content;
using JustLoaded.Loading;
using nkast.Aether.Physics2D.Dynamics;

namespace CosmosCosmini.Entities.Def;

[Def("e", SearchDir = "entity")]
[CreateDb("entity")]
public class EntityDef : PolymorphicDef {
    public EntityBehaviorDef[] Behaviors { get; init; } = [];
    
    //TODO use shapes from tiled
    public required DatabaseReference<AnimatedSprite> Sprite { get; init; }
    
    //TODO use shapes from tiled
    public PhysicsDef Physics { get; init; } = new PhysicsDef();

    public virtual Entity Instantiate(World world, EntityManager manager) {
        return new Entity(this, world, manager);
    }
}