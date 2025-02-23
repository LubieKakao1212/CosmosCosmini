using Base.Entities.Behaviors.Def;
using Microsoft.Xna.Framework;

namespace Base.Entities.Behaviors;

public abstract class EntityBehavior(Entity entity) {
    public readonly Entity entity = entity;

    public virtual void Construct() { }
    
    public virtual void OnEntityAdded() { }
    
    public virtual void Update(GameTime gameTime) { }

    public virtual void OnEntityRemoved() { }
}

public abstract class EntityBehavior<TDef>(TDef def, Entity entity) : EntityBehavior(entity) where TDef : EntityBehaviorDef {
    public TDef Def { get; } = def;
}