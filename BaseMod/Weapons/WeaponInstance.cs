using Base.Def;
using Base.Def.Weapon;
using CosmosCosmini;
using Custom2d_Engine.Physics;
using Custom2d_Engine.Ticking;
using Custom2d_Engine.Ticking.Actions;

namespace Base.Weapons;

public abstract class WeaponInstance(PhysicsBodyObject ownerObject, AttachmentPoint attachmentPoint, TimeSpan cooldown) {

    protected readonly PhysicsBodyObject _ownerObject = ownerObject;
    protected TickMachineBase? _activeTicker;
    
    public virtual void StartShooting() {
        if (cooldown > TimeSpan.Zero) {
            _activeTicker = _ownerObject.AddAccurateRepeatingAction(DoShoot, cooldown);
        }
        else {
            _activeTicker = _ownerObject.AddSimpleRepeatingAction(DoShoot, TimeSpan.FromMicroseconds(1));
        }
    }
    
    protected abstract void DoShoot();

    public virtual void StopShooting() {
        if (_activeTicker != null) {
            _activeTicker.Dispose();
        }
        else {
            CosmosGame.Logger.Warning($"Weapon {GetType().Name} experienced unscheduled shutdown but it was never active");
        }
    }
    
    public virtual void Hit() { }
}

public abstract class WeaponInstance<TDef>(TDef def, PhysicsBodyObject ownerObject, AttachmentPoint attachmentPoint) : WeaponInstance(ownerObject, attachmentPoint, TimeSpan.FromSeconds(1f / def.Rps)) where TDef : WeaponDef {
    public TDef Def { get; set; } = def;

}
