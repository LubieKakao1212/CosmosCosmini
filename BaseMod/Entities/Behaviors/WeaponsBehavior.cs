using Base.Weapons;
using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Entities;
using CosmosCosmini.Entities.Behaviors;
using CosmosCosmini.Entities.Behaviors.Def;

namespace Base.Entities.Behaviors;

public class WeaponsBehavior(WeaponsBehaviorDef def, Entity entity) : EntityBehavior<WeaponsBehaviorDef>(def, entity) {

    protected Dictionary<string, WeaponInstance> _weapons = new();
    
    public override void Construct() {
        base.Construct();

        foreach (var weaponEntry in Def.Weapons) {
            var weapon = weaponEntry.Value.InstantiateWeapon(this);
            
            _weapons.Add(weaponEntry.Key, weapon);
        }
    }

    public void StartShooting(string weapon) {
        _weapons[weapon].StartShooting();
    }

    public void StopShooting(string weapon) {
        _weapons[weapon].StopShooting();
    }
}

[SubType(typeof(EntityBehaviorDef), "weapons")]
public class WeaponsBehaviorDef : EntityBehaviorDef {

    public required Dictionary<string, AttachmentPointDef> Weapons { get; init; }

    public override EntityBehavior Instantiate(Entity entity) {
        return new WeaponsBehavior(this, entity);
    }
}