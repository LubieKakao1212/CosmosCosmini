using System.Diagnostics.CodeAnalysis;
using CosmosCosmini.Graphics;
using CosmosCosmini.JustLoadedEx;
using CosmosCosmini.Scene;
using Custom2d_Engine.Input;
using Custom2d_Engine.Rendering;
using Custom2d_Engine.Rendering.Sprites.Atlas;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Ticking;
using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Core.Discovery;
using JustLoaded.Filesystem;
using JustLoaded.Logger;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;

namespace CosmosCosmini;

public class CosmosGame : Game {

    private GraphicsDeviceManager _graphicsManager;
    
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
        
    [NotNull] private AsyncLogger? Logger { get; set; }
    
    public CosmosGame() {
        _graphicsManager = new GraphicsDeviceManager(this);
        _graphicsManager.GraphicsProfile = GraphicsProfile.HiDef;
        _graphicsManager.HardwareModeSwitch = false;
        _graphicsManager.PreferredBackBufferWidth = 16 * 16;
        _graphicsManager.PreferredBackBufferHeight = 9 * 16;
        Content.RootDirectory = "Content";
    }

    protected override void LoadContent() {
        base.LoadContent();
        
        CreateLogger();
        
        RenderPipeline = new RenderPipeline();
        SpriteAtlas = new SpriteAtlas<Vector4>(GraphicsDevice);
        PhysicsWorld = new World();
        GlobalTickManager = new TickManager();
        
        GameScene = new Hierarchy(GlobalTickManager);
        GameCamera = new Camera { ViewSize = 1f, AspectRatio = 16f/9f };
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

    private void CreateLogger() {
        Logger = new AsyncLogger(
            new ConsoleLogModule(),
            new StreamLogModule(File.Open("log.log", FileMode.Create, FileAccess.Write, FileShare.Read))
            );
        Logger.Start();
    }
    
    private void CreateMls() {
        var modsFileSystem = new PhysicalFilesystem("mods".AsPath());
        ModLoaderSystem = new ModLoaderSystem.Builder(
                new FilesystemModProvider(modsFileSystem)
                    .WithMods(CoreMod.Construct(modsFileSystem))
                    .Verbose(Logger)) {
                ModFilter = IdentityModFilter.Instance
            }.Build()
            .AddAttachment(this)
            .AddAttachment<ILogger>(Logger);
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

    protected override void OnExiting(object sender, ExitingEventArgs args) {
        ((ILogger)Logger).Info("Shutting down, Goodbye, See you again soon : )");
        Logger.Dispose();
        base.OnExiting(sender, args);
    }
}