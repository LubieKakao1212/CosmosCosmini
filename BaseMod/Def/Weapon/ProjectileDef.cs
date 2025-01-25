using CosmosCosmini.Core.Def;
using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Graphics;
using JustLoaded.Content;
using JustLoaded.Loading;
using nkast.Aether.Physics2D.Dynamics;

namespace Base.Def.Weapon;

[Def("proj", SearchDir = "ship")]
[CreateDb("projectile")]
public class ProjectileDef {
    
    public required DatabaseReference<AnimatedSprite> Sprite { get; init; }

    public TimeSpan MaxLifespan { get; init; } = TimeSpan.FromSeconds(30);

    public PhysicsDef Physics { get; init; } = new() {
        Mass = new PhysicsDef.MassDataDef() {
            Mass = 0.1f
        },
        AngularDrag = 0,
        LinearDrag = 0,
        Type = BodyType.Dynamic
    };

    public Vector2Def ImpactPointOffset { get; init; } = new() { x = 0, y = 0 };

    public float PointRadius { get; init; } = 0.1f;
}