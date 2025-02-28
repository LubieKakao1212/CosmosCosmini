using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Entities;
using CosmosCosmini.Entities.Behaviors;
using CosmosCosmini.Entities.Behaviors.Def;

namespace Base.Entities.Behaviors;

public class DeathBehaviour(DeathBehaviourDef def, Entity entity) : EntityBehavior<DeathBehaviourDef>(def, entity)
{
    public override void Construct()
    {
        base.Construct();
        HealthBehaviour? healthBehaviour = entity.GetOnlyBehavior<HealthBehaviour>();
        healthBehaviour.HealthDepleted += Death;
    }

    private void Death()
    {
        if (entity.CurrentHierarchy != null)
        {
            if (entity.Parent != null)
            {
                entity.Parent = null;
            }
            else
            {
                entity.CurrentHierarchy.RemoveObject(entity);
            }
        }
    }
}

[SubType(typeof(EntityBehaviorDef), "death-behaviour")]
public class DeathBehaviourDef : EntityBehaviorDef
{
    public override EntityBehavior Instantiate(Entity entity)
    {
        return new DeathBehaviour(this, entity);
    }
}