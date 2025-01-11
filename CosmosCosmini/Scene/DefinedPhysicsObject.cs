using CosmosCosmini.Core.Def;
using CosmosCosmini.Def;
using Custom2d_Engine.Physics;
using nkast.Aether.Physics2D.Dynamics;

namespace CosmosCosmini.Scene;

public class DefinedPhysicsObject : PhysicsBodyObject {

    public World World => PhysicsBody.World;
    
    public DefinedPhysicsObject(PhysicsDef def, World world) : base(world.CreateBody(bodyType: def.type)) {
        PhysicsBody.Tag = this;
        PhysicsBody.LinearDamping = def.linearDrag;
        PhysicsBody.AngularDamping = def.angularDrag;
        foreach (var fixtureDef in def.fixtures) {
            PhysicsBody.Add(fixtureDef.Construct());
        }
        if (def.mass != null) {
            var mass = def.mass.Value;
            PhysicsBody.Mass = mass.mass;
            PhysicsBody.LocalCenter = mass.centerOfMass.Construct();
        }
    }
    
}