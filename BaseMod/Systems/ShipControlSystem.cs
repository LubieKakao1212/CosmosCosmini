using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Base.Constants;
using Base.Def;
using Base.Ship;
using Base.Ship.Ai;
using CosmosCosmini;
using CosmosCosmini.Scene;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Ticking;
using JustLoaded.Content;
using JustLoaded.Core;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace Base.Systems;

public class ShipControlSystem : IGameSystem {
    
    private readonly Dictionary<ShipObject, AiController> _controllers = new();

    private readonly AutoTimeMachine _spawner;

    private readonly Hierarchy _gameHierarchy;
    
    [NotNull]
    private ShipCollection? Ships { get; set; }

    private readonly CosmosGame _game;
    private readonly ModLoaderSystem _modLoader;

    public ShipControlSystem(ModLoaderSystem modLoader) {
        _modLoader = modLoader;
        _gameHierarchy = modLoader.GetRequiredAttachment<CosmosGame>().GameHierarchy;
        _game = modLoader.GetRequiredAttachment<CosmosGame>();
        _spawner = new AutoTimeMachine(CreateEnemy, TimeSpan.FromSeconds(5));
    }

    public void InitHierarchy(Hierarchy gameHierarchy) {
        Ships = new ShipCollection(gameHierarchy);
        
        CreateShip(gameHierarchy, new ContentKey(BaseMod.ModId, "player"), FactionAlignment.Friendly, Vector2.Zero);
    }

    public void Update(GameTime gameTime, Hierarchy gameHierarchy) {
        foreach (var entry in _controllers) {
            entry.Value.DoAi(entry.Key, Ships, gameTime);
        }
        _spawner.Forward(gameTime.ElapsedGameTime);
    }
    
    public void SaveState() {
        throw new NotImplementedException();
    }

    public void LoadState() {
        throw new NotImplementedException();
    }

    private void CreateEnemy() {
        var angle = Random.Shared.NextSingle() * MathF.PI * 2f;
        
        CreateShip(_gameHierarchy, new ContentKey("base:enemy"), FactionAlignment.Hostile, 
            new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * 5f);
    }
    
    private void CreateShip(Hierarchy gameHierarchy, ContentKey defKey, FactionAlignment alignment, Vector2 position) {
        var key = BoundContentKey<ShipDef>.Make(defKey);
        var def = key.FetchContent(_modLoader.MasterDb)!;

        var ship = new ShipObject(def, _game.PhysicsWorld) {
            Transform = {
                GlobalPosition = position
            }
        };

        //TODO Move layers to fixture def
        foreach (var fixture in ship.PhysicsBody.FixtureList) {
            fixture.CollisionCategories = alignment switch {
                FactionAlignment.Friendly => Collisions.Cats.Friendly,
                FactionAlignment.Hostile => Collisions.Cats.Hostile,
                _ => 0
            };

            fixture.CollidesWith = alignment switch {
                FactionAlignment.Friendly => Collisions.CollidesWith.Friendly,
                FactionAlignment.Hostile => Collisions.CollidesWith.Hostile,
                _ => 0
            };
        }
        
        new AnimatedDrawableObject(def.Sprite.Value!) {
            Parent = ship
        };
        
        gameHierarchy.AddObject(ship);
        Ships.Add(alignment, ship);
        _controllers.Add(ship, def.Ai.Instantiate(alignment, _modLoader));
    }
}