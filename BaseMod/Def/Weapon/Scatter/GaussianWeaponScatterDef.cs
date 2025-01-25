using CosmosCosmini.Core.Serialization;

namespace Base.Def.Weapon.Scatter;

[SubType(typeof(WeaponScatterDef), "gaussian")]
public class GaussianWeaponScatterDef : WeaponScatterDef {
    
    public required float StdDev { get; init; }

    private float? next = null;
    
    public override float ScatterAngle() {
        if (next != null) {
            var result = next.Value;
            next = null;
            return result;
        }
        else {
            var random = Random.Shared;
            // The method requires sampling from a uniform random of (0,1]
            // but Random.NextDouble() returns a sample of [0,1).
            var x1 = 1 - random.NextSingle();
            var x2 = 1 - random.NextSingle();

            var l1 = MathF.Sqrt(-2f * MathF.Log(x1));
            
            var y1 = l1 * MathF.Cos(2f * MathF.PI * x2);
            
            next = l1 * MathF.Sin(2f * MathF.PI * x2);
            next *= StdDev;
            return y1 * StdDev;
        }
    }
}