using Base.Entities.Damage;

namespace Base.Entities.Interfaces;

public interface IHurtResponder {

    public void OnHurt(in DamageInstance instance);

}