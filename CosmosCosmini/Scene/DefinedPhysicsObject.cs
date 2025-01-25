using CosmosCosmini.Core.Def;
using Custom2d_Engine.Physics;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace CosmosCosmini.Scene;

public class DefinedPhysicsObject : PhysicsBodyObject {

    public World World => PhysicsBody.World;
    
    public DefinedPhysicsObject(PhysicsDef def, World world) : base(world.CreateBody(bodyType: def.Type)) {
        PhysicsBody.Tag = this;
        PhysicsBody.LinearDamping = def.LinearDrag;
        PhysicsBody.AngularDamping = def.AngularDrag;
        //TODO Move to Custom_2D
        PhysicsBody.Enabled = false;
        
        foreach (var fixtureDef in def.Fixtures) {
            PhysicsBody.Add(fixtureDef.Construct());
        }
        if (def.Mass != null) {
            var mass = def.Mass.Value;
            PhysicsBody.Mass = mass.Mass;
            PhysicsBody.LocalCenter = mass.CenterOfMass.Construct();
        }

        if (def.Inertia != null) {
            PhysicsBody.Inertia = def.Inertia.Value;
        }
        
    }

    protected override void CustomUpdate(GameTime time) {
        //TODO Move to Custom_2D
        if (!PhysicsBody.Enabled) {
            PhysicsBody.Enabled = true;
        }
        base.CustomUpdate(time);
    }
    
    protected override void RemovedFromScene() {
        base.RemovedFromScene();
        PhysicsBody.Enabled = false;
    }

    ~DefinedPhysicsObject() {
        PhysicsBody.World.Remove(PhysicsBody);
    }
}