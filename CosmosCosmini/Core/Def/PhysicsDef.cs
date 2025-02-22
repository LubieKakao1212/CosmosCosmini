using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;

namespace CosmosCosmini.Core.Def;

public class PhysicsDef {

    public float LinearDrag { get; init; } = 0;
    public float AngularDrag { get; init; } = 0;
    public MassDataDef? Mass { get; init; } = null;
    public float? Inertia { get; init; } = null;
    public BodyType Type { get; init; } = BodyType.Static;
    public FixtureDef[] Fixtures { get; init; } = [];
    
    public readonly struct MassDataDef() {
        public required float Mass { get; init; } = 0;
        public Vector2Def CenterOfMass { get; init; } = default;
    }
    
    public readonly struct FixtureDef() {
        //Shape
        public required ShapeType Type { get; init; }
        public Vector2Def Center { get; init; } = default;
        public float Density { get; init; } = 1;
        public required float Radius { get; init; }
        //Rectangle only
        public float HorizontalRatio { get; init; } = 1;
        public float Angle { get; init; } = 0;
        
        //Fixture
        public float Friction { get; init; } = 0;
        public float Restitution { get; init; } = 0;
        
        public Fixture Construct() {
            Shape shape;
            switch (Type) {
                case ShapeType.Circle: {
                    shape = new CircleShape(Radius, Density) {
                        Position = Center.Construct()
                    };
                    break;
                }
                case ShapeType.Rectangle: {
                    var p = PolygonTools.CreateRectangle(Radius * HorizontalRatio, Radius, Center.Construct(), Angle);
                    shape = new PolygonShape(p, Density);
                    break;
                }
                default: {
                    shape = new CircleShape(1, 1);
                    break;
                }
            }
            var fixture = new Fixture(shape) {
                Friction = Friction,
                Restitution = Restitution
            };
            return fixture;
        }
        
        public enum ShapeType {
            Circle,
            Rectangle
        }
    }
}