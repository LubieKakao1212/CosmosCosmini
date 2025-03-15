using CosmosCosmini.Entities.Def;
using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Logger;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace CosmosCosmini.Entities;

public class EntityManager {

    public IEntityControls Controls { get; }

    private HashSet<Entity> _entities = new();
    public ILogger Logger { get; }

    private readonly World _world;
    
    private readonly IReadOnlyMasterDatabase _mdb;
    
    public EntityManager(ModLoaderSystem modLoader) {
        _mdb = modLoader.MasterDb;
        var game = modLoader.GetRequiredAttachment<CosmosGame>();

        Logger = modLoader.GetRequiredAttachment<ILogger>();
        _world = game.PhysicsWorld;
        Controls = new EntityControls(game.Input);
    }


    public Entity? CreateEntity(ContentKey defId, Vector2 position) {
        var def = _mdb.GetDatabase<EntityDef>().GetContent<EntityDef>(defId);

        if (def == null) {
            Logger.Error($"No such entity definition {defId}");
            return null;
        }

        return CreateEntity(def, position);
    }
    
    
    public Entity CreateEntity(EntityDef def, Vector2 position) {
        var entity = def.Instantiate(_world, this);
        entity.Transform.GlobalPosition = position;

        _entities.Add(entity);
        entity.OnDespawn += RemoveEntity;
        return entity;
    }

    private void RemoveEntity(Entity ent)
    {
        if (!_entities.Remove(ent))
        {
            Logger.Error("Dude it ain't there!");
        }
    }

    public Entity GetFirstEntity(Func<Entity, bool> predicate)
    {
        return _entities.First(predicate);
    }
    
    public Entity? GetFirstEntityOrNull(Func<Entity, bool> predicate)
    {
        return _entities.FirstOrDefault(predicate);
    }

    public Entity? GetBestEntity(Func<Entity, Entity?, bool> predicate)
    {
        return _entities.Aggregate<Entity, Entity?>(null, (accumulate, entity) => predicate(entity, accumulate) ? entity : accumulate);
    }
}