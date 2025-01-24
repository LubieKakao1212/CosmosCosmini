using Base.Def.Weapon;
using CosmosCosmini.Core.Def;
using Custom2d_Engine.Physics;
using Custom2d_Engine.Scenes;
using JustLoaded.Content;

namespace Base.Weapons;

public class AttachmentPointDef {

    public required Vector2Def LocalPosition { get; init; }

    public required DatabaseReference<WeaponDef> Weapon { get; init; }

    public float Direction { get; init; } = 0;

    public WeaponInstance InstantiateWeapon(PhysicsBodyObject owner) {
        return Weapon.Value.InstantiateWeapon(owner, new AttachmentPoint(this));
    }
}