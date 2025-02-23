using Base.Entities.Behaviors.Def;
using Custom2d_Engine.Ticking;
using Microsoft.Xna.Framework;

namespace Base.Entities.Behaviors;


public class HealthBehaviour(HealthBehaviourDef def, Entity entity) : EntityBehavior<HealthBehaviourDef>(def, entity)
{
    private int _currentHP = def.MaxHp;
    private int _maxHP = def.MaxHp;

    public event Action HealthDepleted =  delegate{};
    private bool _isAlive = true;

    public void ReceiveDamage(int amt)
    {
        _currentHP -= amt;
        if (_currentHP <= 0 && _isAlive)
        {
            _isAlive = false;
            HealthDepleted?.Invoke();
        }
        else if(_currentHP > _maxHP)
        {
            _currentHP = _maxHP;
        }
        Console.WriteLine("cur_hp = " + _currentHP.ToString());
    }

    public override void Construct()
    {
        base.Construct();
        entity.AddAccurateRepeatingAction(() => { ReceiveDamage(15); }, TimeSpan.FromSeconds(2.5f));
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
    }
}