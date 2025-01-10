using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;

namespace CosmosCosmini.Core.Def;

public class PhysicsDef {

    public float linearDrag = 0;
    public float angularDrag = 0;
    public MassDataDef? mass = null;
    public BodyType type = BodyType.Dynamic;
    public FixtureDef[] fixtures = [];
    
    public struct MassDataDef() {
        public required float mass = 0;
        public Vector2Def centerOfMass = default;
    }

    public struct FixtureDef() {
        //Shape
        public required Type type;
        public Vector2Def center = default;
        public float density = 1;
        public required float radius;
        //Rectangle only
        public float horizontalRatio = 1;
        public float angle = 1;
        
        //Fixture
        public float friction = 0;
        public float restitution = 0;
        
        public Fixture Construct() {
            Shape shape;
            switch (type) {
                case Type.Circle: {
                    shape = new CircleShape(radius, density) {
                        Position = center.Construct()
                    };
                    break;
                }
                case Type.Rectangle: {
                    var p = PolygonTools.CreateRectangle(radius * horizontalRatio, radius, center.Construct(), angle);
                    shape = new PolygonShape(p, density);
                    break;
                }
                default: {
                    shape = new CircleShape(1, 1);
                    break;
                }
            }
            var fixture = new Fixture(shape) {
                Friction = friction,
                Restitution = restitution
            };
            return fixture;
        }
        
        public enum Type {
            Circle,
            Rectangle
        }
    }
}