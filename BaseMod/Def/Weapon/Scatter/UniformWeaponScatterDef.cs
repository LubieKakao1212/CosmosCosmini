using CosmosCosmini.Core.Serialization;

namespace Base.Def.Weapon.Scatter;

[SubType(typeof(WeaponScatterDef), "uniform")]
public class UniformWeaponScatterDef : WeaponScatterDef {

    public required float Angle { get; init; }

    public override float ScatterAngle() {
        return Random.Shared.NextSingle() * Angle - Angle / 2f;
    }
}