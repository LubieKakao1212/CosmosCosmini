using CosmosCosmini.Entities.Def;
using Custom2d_Engine.Scenes;
using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Logger;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace CosmosCosmini.Entities;

public class EntityManager {

    public IEntityControls Controls { get; }
    public ILogger Logger { get; }

    private readonly Dictionary<EntityDef, EntityPool> _pools = new();
    private readonly HashSet<Entity> _activeEntities = new();
    // private readonly Stack<(Entity, EntityOperation)> _currentlyOperatedOn = new();
    
    private readonly World _world;
    private readonly IReadOnlyMasterDatabase _mdb;
    private readonly Hierarchy _hierarchy;
    
    public EntityManager(ModLoaderSystem modLoader) {
        _mdb = modLoader.MasterDb;
        var game = modLoader.GetRequiredAttachment<CosmosGame>();

        Logger = modLoader.GetRequiredAttachment<ILogger>();
        _world = game.PhysicsWorld;
        _hierarchy = game.GameHierarchy;
        Controls = new EntityControls(game.Input);

        foreach (var def in _mdb.GetDatabase<EntityDef>().GetContentValues<EntityDef>()) {
            if (def.UsePool) {
                //TODO Unhardcode initial capacity
                _pools.Add(def, new EntityPool(256, def));
            }
        }
    }

    public Entity? MakeEntity(ContentKey defId) {
        var def = _mdb.GetDatabase<EntityDef>().GetContent<EntityDef>(defId);

        if (def == null) {
            Logger.Error($"No such entity definition {defId}");
            return null;
        }

        return MakeEntity(def);
    }

    public Entity MakeEntity(EntityDef def) {
        if (_pools.TryGetValue(def, out var pool)) {
            return pool.GetOrCreate(CreateEntity);
        }
        return CreateEntity(def);
    }
    
    private Entity CreateEntity(EntityDef def) {
        var entity = def.Instantiate(_world, this);
        return entity;
    }

    public void SpawnEntity(Entity entity) {
        if(!_activeEntities.Add(entity)) {
            Logger.Error("Entity is already spawned");
            return;
        }
        entity.Spawn();
        entity.DoAddToScene(_hierarchy);
    }

    public void DespawnEntity(Entity entity)
    {
        if (!_activeEntities.Contains(entity)) {
            Logger.Error("Dude it ain't there!, This will cause an invalid state"); //TODO replace with throw
            Logger.LogTrace(LogLevel.Error, LogFilter.Always);
            return;
        }
        
        entity.DoRemoveFromScene();
        entity.Despawn();
        if (!_activeEntities.Remove(entity)) 
        {
            Logger.Error("Dude it ain't there! But it was there!, This will cause an invalid state"); //TODO replace with throw
            Logger.LogTrace(LogLevel.Error, LogFilter.Always);
            return;
        }
    }
    
    public void ReturnEntity(Entity entity) {
        if (_activeEntities.Contains(entity)) {
            Logger.Error("Cannot return an active entity, despawn it first!!!"); //TODO replace with throw
            Logger.LogTrace(LogLevel.Error, LogFilter.Always);
            return;
        }
        
        if (_pools.TryGetValue(entity.EntityDef, out var pool)) {
            pool.Return(entity);
        }
        else {
            _world.Remove(entity.PhysicsBody);
        }
    }
    
    public Entity GetFirstEntity(Func<Entity, bool> predicate)
    {
        return _activeEntities.First(predicate);
    }
    
    public Entity? GetFirstEntityOrNull(Func<Entity, bool> predicate)
    {
        return _activeEntities.FirstOrDefault(predicate);
    }

    public Entity? GetBestEntity(Func<Entity, Entity?, bool> predicate)
    {
        return _activeEntities.Aggregate<Entity, Entity?>(null, (accumulate, entity) => predicate(entity, accumulate) ? entity : accumulate);
    }
}