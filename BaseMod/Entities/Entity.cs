using Base.Def.Entities;
using Base.Entities.Behavior;
using CosmosCosmini.Scene;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace Base.Entities;

public class Entity : DefinedPhysicsObject {

    public List<EntityBehavior> Behaviors { get; }

    public EntityDef EntityDef { get; }

    private bool WasConstructed { get; set; }

    public Entity(EntityDef def, World world) : base(def.Physics, world) {
        EntityDef = def;
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

}