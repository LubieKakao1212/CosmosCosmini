using Base.Entities.Def;
using CosmosCosmini;
using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Logger;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace Base.Entities;

public class EntityManager {

    public IEntityControls Controls { get; }

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

        return entity;
    }
    
}