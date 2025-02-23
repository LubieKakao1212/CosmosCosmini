using System.ComponentModel;
using Base.Entities.Behaviors;
using Base.Entities.Def;
using CosmosCosmini.Scene;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace Base.Entities;

public class Entity : DefinedPhysicsObject {

    public List<EntityBehavior> Behaviors { get; }

    public EntityDef EntityDef { get; }

    private bool WasConstructed { get; set; }

    public EntityManager Manager { get; }

    public Entity(EntityDef def, World world, EntityManager manager) : base(def.Physics, world) {
        EntityDef = def;
        this.Manager = manager;
        Behaviors = def.Behaviors.Select(behaviorDef => behaviorDef.Instantiate(this)).ToList();
    }
    
    protected virtual void Construct() {
        var sprite = EntityDef.Sprite.Value;
        
        if (sprite != null) {
            _ = new AnimatedDrawableObject(sprite) {
                Parent = this
            };
        }
        
        foreach (var behavior in Behaviors) {
            behavior.Construct();
        }
    }
    protected override void CustomUpdate(GameTime time) {
        base.CustomUpdate(time);
        foreach (var behavior in Behaviors) {
            behavior.Update(time);
        }
    }

    protected override void AddedToScene() {
        base.AddedToScene();
        if (!WasConstructed) {
            Construct();
            WasConstructed = true;
        }
        
        foreach (var behavior in Behaviors) {
            behavior.OnEntityAdded();
        }
    }

    public override void RemovedFromScene() {
        base.RemovedFromScene();
        foreach (var behavior in Behaviors) {
            behavior.OnEntityRemoved();
        }
    }

    public IEnumerable<T> GetBehaviors<T>() where T : class {
        return Behaviors.OfType<T>();
    }

    public T? GetAnyBehavior<T>() where T : class {
        return GetBehaviors<T>().FirstOrDefault();
    }

    public T? GetOnlyBehavior<T>() where T : class {
        T? result = null;
        foreach (var behavior in GetBehaviors<T>()) {
            if (result != null) {
                throw new ApplicationException($"More than one argument of type {typeof(T)}");
            }
            result = behavior;
        }
        return result;
    }

    public T? GetAnyInterface<T>() where T : class {
        return this as T ?? GetAnyBehavior<T>();
    }

    public IEnumerable<T> GetInterfaces<T>() where T : class {
        return this is T cast ? Enumerable.Repeat(cast, 1).Concat(GetBehaviors<T>()) : GetBehaviors<T>();
    }
    
}