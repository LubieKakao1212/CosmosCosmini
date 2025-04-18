using CosmosCosmini.Entities.Behaviors.Def;
using Microsoft.Xna.Framework;

namespace CosmosCosmini.Entities.Behaviors;

public abstract class EntityBehavior(Entity entity) {
    public readonly Entity entity = entity;

    public virtual void Construct(bool first) { }
    
    public virtual void OnSpawn() { }
    
    public virtual void Update(GameTime gameTime) { }

    public virtual void OnDespawn() { }
}

public abstract class EntityBehavior<TDef>(TDef def, Entity entity) : EntityBehavior(entity) where TDef : EntityBehaviorDef {
    public TDef Def { get; } = def;
}