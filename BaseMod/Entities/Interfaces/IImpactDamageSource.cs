using nkast.Aether.Physics2D.Dynamics;

namespace Base.Entities.Interfaces;

public interface IImpactDamageSource {
    
    /// <param name="cause"></param>
    /// <param name="victim"></param>
    /// <param name="victimEntity"></param>
    /// <returns>Amount of damage dealt by this source</returns>
    int HandleImpact(Fixture cause, Fixture victim, Entity victimEntity);
    
}