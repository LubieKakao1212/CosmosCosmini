using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Graphics;
using Custom2d_Engine.Rendering.Sprites;
using JustLoaded.Content;
using JustLoaded.Loading;

namespace Base.Def.Weapon;

[Def("proj", SearchDir = "ship")]
[CreateDb("projectile")]
public class ProjectileDef {
    
    public required DatabaseReference<AnimatedSprite> Sprite { get; init; }

}