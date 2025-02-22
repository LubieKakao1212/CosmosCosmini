using Base.Def.Entities.Behaviors;
using Microsoft.Xna.Framework;

namespace Base.Entities.Behaviors;

public abstract class EntityBehavior(Entity entity) {
    protected readonly Entity _entity = entity;

    public virtual void Construct() { }
    
    public virtual void OnEntityAdded() { }
    
    public virtual void Update(GameTime gameTime) { }

    public virtual void OnEntityRemoved() { }
}

public abstract class EntityBehavior<TDef>(TDef def, Entity entity) : EntityBehavior(entity) where TDef : EntityBehaviorDef {
    public TDef Def { get; } = def;
}