using System.Diagnostics.CodeAnalysis;
using CosmosCosmini.Graphics;
using CosmosCosmini.JustLoadedEx;
using CosmosCosmini.Scene;
using Custom2d_Engine.Input;
using Custom2d_Engine.Rendering;
using Custom2d_Engine.Rendering.Sprites;
using Custom2d_Engine.Rendering.Sprites.Atlas;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Ticking;
using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Core.Discovery;
using JustLoaded.Filesystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;

namespace CosmosCosmini;

public class CosmosGame : Game {

    private GraphicsDeviceManager _graphicsManager;
    
    //TODO Use MLS attachment system
    [NotNull] internal static CosmosGame? Game { get; private set; }
    //TODO Use MLS attachment system
    [NotNull] internal static IFilesystem? Filesystem { get; set; }

    [NotNull] public InputManager? Input { get; private set; }
    [NotNull] public World? PhysicsWorld { get; private set; }
    [NotNull] public TickManager? GlobalTickManager { get; private set; }
    
    [NotNull] public Hierarchy? GameScene { get; private set; }
    [NotNull] public Camera? GameCamera { get; private set; }
    [NotNull] public Hierarchy? Ui { get; private set; }
    public Camera UiCamera { get; } = new();
    
    [NotNull] public RenderPipeline? RenderPipeline { get; private set; }
    [NotNull] public SpriteAtlas<Vector4>? SpriteAtlas { get; private set; }
    [NotNull] public ModLoaderSystem? ModLoaderSystem { get; private set; }
    
    public CosmosGame() {
        Game = this;
        _graphicsManager = new GraphicsDeviceManager(this);
        _graphicsManager.GraphicsProfile = GraphicsProfile.HiDef;
        _graphicsManager.HardwareModeSwitch = false;
        Content.RootDirectory = "Content";
    }

    protected override void LoadContent() {
        base.LoadContent();
        
        RenderPipeline = new RenderPipeline();
        SpriteAtlas = new SpriteAtlas<Vector4>(GraphicsDevice);
        PhysicsWorld = new World();
        GlobalTickManager = new TickManager();
        
        GameScene = new Hierarchy(GlobalTickManager);
        GameCamera = new Camera { ViewSize = 10, AspectRatio = 16f/9f };
        GameScene.AddObject(GameCamera);
        
        Ui = new Hierarchy(GlobalTickManager);
        Ui.AddObject(UiCamera);
        
        Input = new InputManager(Window);
        
        RenderPipeline.Init(GraphicsDevice);
        Effects.Init(Content);
        
        CreateMls();

        InitMods();
        RenderPipeline.SpriteAtlas = SpriteAtlas.AtlasTextures!;
        
        var s = BoundContentKey<AnimatedSprite>.Make(new ContentKey("base:player"))
            .FetchContent(masterDb: ModLoaderSystem.MasterDb)!;

        var player = new AnimatedDrawableObject(s);
        
        GameScene.AddObject(player);
    }

    protected override void Update(GameTime gameTime) {
        base.Update(gameTime);
        
        Input.UpdateState();
        PhysicsWorld.Step(gameTime.ElapsedGameTime);
        
        GameScene.Update(gameTime);
        Ui.Update(gameTime);
        
        GlobalTickManager.Forward(gameTime.ElapsedGameTime);
    } 

    protected override void Draw(GameTime gameTime) {
        base.Draw(gameTime);

        foreach (var (key, value) in ModLoaderSystem.MasterDb.GetDatabase<AnimatedSprite>().GetContentEntries<AnimatedSprite>()) {
            value.Update(gameTime.ElapsedGameTime);
        }
        
        GraphicsDevice.Clear(Color.Aqua);
        
        RenderPipeline.RenderScene(GameScene, GameCamera);
        RenderPipeline.RenderScene(Ui, UiCamera);
    }
    
    private void CreateMls() {
        var modsFileSystem = new PhysicalFilesystem("mods".AsPath());
        ModLoaderSystem = new ModLoaderSystem.Builder(
            new LoggingModProvider(
                new FilesystemModProvider(
                    modsFileSystem
                    ).WithMods(CoreMod.Construct(modsFileSystem))
                )
            ) {
                ModFilter = IdentityModFilter.Instance
            }.Build();
    }

    private void InitMods() {
        try {
            ModLoaderSystem.DiscoverMods();
            ModLoaderSystem.ResolveDependencies();
            ModLoaderSystem.InitMods();
            ModLoaderSystem.Load();
        }
        catch {
            //TODO
            throw;
        }
    }
}