using Base.Def;
using Base.Weapons;
using CosmosCosmini.Scene;
using nkast.Aether.Physics2D.Dynamics;

namespace Base.Ship;

public class ShipObject : DefinedPhysicsObject {

    public ShipDef ShipDef { get; }

    public WeaponInstance[] Weapons { get; }
    
    public ShipObject(ShipDef shipDef, World world) : base(shipDef.Physics, world) {
        ShipDef = shipDef;
        Weapons = shipDef.Weapons.Select(point => point.InstantiateWeapon(this)).ToArray();
    }

}