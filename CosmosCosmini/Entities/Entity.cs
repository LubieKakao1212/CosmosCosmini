using CosmosCosmini.Core;
using CosmosCosmini.Entities.Behaviors;
using CosmosCosmini.Entities.Def;
using CosmosCosmini.Scene;
using Custom2d_Engine.Scenes;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace CosmosCosmini.Entities;

public class Entity : DefinedPhysicsObject, IRefVersion
{
    public List<EntityBehavior> Behaviors { get; }

    public EntityDef EntityDef { get; }

    private bool WasConstructed { get; set; }

    public EntityManager Manager { get; }

    private ulong Version { get; set; }

    private bool allowReparent = false;
        
    ulong IRefVersion.Version {
        get => Version;
        set => Version = value;
    }

    public Entity(EntityDef def, World world, EntityManager manager) : base(def.Physics, world) {
        EntityDef = def;
        this.Manager = manager;
        Behaviors = def.Behaviors.Select(behaviorDef => behaviorDef.Instantiate(this)).ToList();
    }
    
    protected virtual void Construct(bool first) {
        if (first) {
            var sprite = EntityDef.Sprite.Value;

            if (sprite != null) {
                _ = new AnimatedDrawableObject(sprite) {
                    Parent = this
                };
            }
        }

        foreach (var behavior in Behaviors) {
            behavior.Construct(first);
        }
    }
    
    protected override void CustomUpdate(GameTime time) {
        base.CustomUpdate(time);
        foreach (var behavior in Behaviors) {
            behavior.Update(time);
        }
    }

    protected override void AddedToScene() {
        //TODO Does not work
        // if (!allowReparent) {
        //     throw new CannotReparentEntityException("Cannot manually reparent or change the scene of an entity", this);
        // }
        base.AddedToScene();
    }

    public override void RemovedFromScene() {
        //TODO Does not work
        // if (!allowReparent) {
        //     throw new CannotReparentEntityException("Cannot manually reparent or change the scene of an entity", this);
        // }
        base.RemovedFromScene();
    }
    
    internal void Despawn() {
        DoDespawn();
        foreach (var behavior in Behaviors) {
            behavior.OnDespawn();
        }
    }

    protected virtual void DoDespawn() { }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="EntitySpawningFailedException"></exception>
    internal void Spawn() {
        Construct(!WasConstructed);
        WasConstructed = true;
        
        DoSpawn();
        foreach (var behavior in Behaviors) {
            behavior.OnSpawn();
        }
    }
    
    /// <summary>
    /// Invoked after <see cref="Construct"/>
    /// </summary>
    protected virtual void DoSpawn() { }

    internal void DoAddToScene(Hierarchy hierarchy) {
        allowReparent = true;
        hierarchy.AddObject(this);
        allowReparent = false;
    }
    
    internal void DoRemoveFromScene() {
        allowReparent = true;
        CurrentHierarchy?.RemoveObject(this);
        allowReparent = false;
    }
    
    public IEnumerable<T> GetBehaviors<T>() where T : class {
        return Behaviors.OfType<T>();
    }

    public T? GetAnyBehavior<T>() where T : class {
        return GetBehaviors<T>().FirstOrDefault();
    }

    public T? GetOnlyBehaviorOrNull<T>() where T : class {
        T? result = null;
        foreach (var behavior in GetBehaviors<T>()) {
            if (result != null) {
                throw new ApplicationException($"More than one behavior of type {typeof(T)}");
            }
            result = behavior;
        }
        return result;
    }
    
    public T GetOnlyBehavior<T>() where T : class {
        return GetOnlyBehaviorOrNull<T>() ?? throw new ApplicationException($"No behavior of type {typeof(T)}");
    }
    
    public T? GetAnyInterface<T>() where T : class {
        return this as T ?? GetAnyBehavior<T>();
    }

    public IEnumerable<T> GetInterfaces<T>() where T : class {
        return this is T cast ? Enumerable.Repeat(cast, 1).Concat(GetBehaviors<T>()) : GetBehaviors<T>();
    }
    
}