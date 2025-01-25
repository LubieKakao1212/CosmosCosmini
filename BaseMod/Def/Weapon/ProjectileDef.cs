using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Graphics;
using JustLoaded.Content;
using JustLoaded.Loading;

namespace Base.Def.Weapon;

[Def("proj", SearchDir = "ship")]
[CreateDb("projectile")]
public class ProjectileDef {
    
    public required DatabaseReference<AnimatedSprite> Sprite { get; init; }

    public TimeSpan MaxLifespan { get; init; } = TimeSpan.FromSeconds(30);
}