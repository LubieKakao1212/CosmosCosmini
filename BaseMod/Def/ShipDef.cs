using Base.Weapons;
using CosmosCosmini.Core.Def;
using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Graphics;
using JustLoaded.Content;
using JustLoaded.Loading;

namespace Base.Def;

[Def("ship", SearchDir = "ship")]
[CreateDb("ship")]
public class ShipDef {
    
    public PhysicsDef Physics { get; init; } = new PhysicsDef();

    public required float MaxThrust { get; init; } = 0;
    public required float MaxAngular { get; init; } = 0;
    public required float MaxBreaking { get; init; } = 0;

    public required AiControllerDef Ai { get; init; }

    public required DatabaseReference<AnimatedSprite> Sprite { get; init; }
    
    public AttachmentPointDef[] Weapons { get; init; } = [];
}