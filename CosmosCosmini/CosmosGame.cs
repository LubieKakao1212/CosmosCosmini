using System.Diagnostics.CodeAnalysis;
using CosmosCosmini.Graphics;
using CosmosCosmini.JustLoadedEx;
using Custom2d_Engine.Input;
using Custom2d_Engine.Rendering;
using Custom2d_Engine.Rendering.Sprites.Atlas;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Ticking;
using JustLoaded.Content.Database;
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

    public const int SpriteAtlasSize = 2048;
    
    [NotNull] public static ILogger Logger { get; private set; }

    [NotNull] public InputManager? Input { get; private set; }
    [NotNull] public World? PhysicsWorld { get; private set; }
    [NotNull] public TickManager? GlobalTickManager { get; private set; }
    
    [NotNull] public Hierarchy? GameHierarchy { get; private set; }
    [NotNull] public Camera? GameCamera { get; private set; }
    [NotNull] public Hierarchy? Ui { get; private set; }
    public Camera UiCamera { get; } = new();
    
    [NotNull] public RenderPipeline? RenderPipeline { get; private set; }
    [NotNull] public SpriteAtlas<Vector4>? SpriteAtlas { get; private set; }
    [NotNull] public ModLoaderSystem? ModLoaderSystem { get; private set; }
    
    [NotNull] private AsyncLogger? LoggerInstance { get; set; }
    
    public CosmosGame() {
        _graphicsManager = new GraphicsDeviceManager(this);
        _graphicsManager.GraphicsProfile = GraphicsProfile.HiDef;
        _graphicsManager.HardwareModeSwitch = false;
        _graphicsManager.PreferredBackBufferWidth = 16 * 64;
        _graphicsManager.PreferredBackBufferHeight = 9 * 64;
        Content.RootDirectory = "Content";
    }

    protected override void LoadContent() {
        base.LoadContent();
        
        CreateLogger();
        
        RenderPipeline = new RenderPipeline();
        SpriteAtlas = new SpriteAtlas<Vector4>(GraphicsDevice, SpriteAtlasSize);
        PhysicsWorld = new World(new Vector2(0, 0));
        GlobalTickManager = new TickManager();
        
        GameHierarchy = new Hierarchy(GlobalTickManager);
        GameCamera = new Camera { ViewSize = 4f, AspectRatio = 16f/9f };
        GameHierarchy.AddObject(GameCamera);
        
        Ui = new Hierarchy(GlobalTickManager);
        Ui.AddObject(UiCamera);
        
        Input = new InputManager(Window);
        
        RenderPipeline.Init(GraphicsDevice);
        Effects.Init(Content);
        
        CreateMls();

        InitMods();
        RenderPipeline.SpriteAtlas = SpriteAtlas.AtlasTextures!;
        
        foreach (var system in ((IContentDatabase<IGameSystem>)ModLoaderSystem.MasterDb.GetDatabase<IGameSystem>()).ContentValues) {
            system.InitHierarchy(GameHierarchy);
        }
    }

    protected override void Update(GameTime gameTime) {
        base.Update(gameTime);

        foreach (var system in ((IContentDatabase<IGameSystem>)ModLoaderSystem.MasterDb.GetDatabase<IGameSystem>()).ContentValues) {
            system.Update(gameTime, GameHierarchy);
        }
        
        Input.UpdateState();
        PhysicsWorld.Step(gameTime.ElapsedGameTime);
        
        GameHierarchy.Update(gameTime);
        Ui.Update(gameTime);
        
        GlobalTickManager.Forward(gameTime.ElapsedGameTime);
    } 

    protected override void Draw(GameTime gameTime) {
        base.Draw(gameTime);

        foreach (var (key, value) in ModLoaderSystem.MasterDb.GetDatabase<AnimatedSprite>().GetContentEntries<AnimatedSprite>()) {
            value.Update(gameTime.ElapsedGameTime);
        }
        
        GraphicsDevice.Clear(Color.Aqua);
        
        RenderPipeline.RenderScene(GameHierarchy, GameCamera);
        RenderPipeline.RenderScene(Ui, UiCamera);
    }

    private void CreateLogger() {
        LoggerInstance = new AsyncLogger(
            TimeSpan.FromTicks(2147483647),
            new ConsoleLogModule(),
            new StreamLogModule(File.Open("log.log", FileMode.Create, FileAccess.Write, FileShare.Read))
            );
        LoggerInstance.Start();
        Logger = LoggerInstance;
    }
    
    private void CreateMls() {
        var modsFileSystem = new PhysicalFilesystem("mods".AsPath());
        ModLoaderSystem = new ModLoaderSystem.Builder(
                new FilesystemModProvider(modsFileSystem)
                    .WithMods(CoreMod.Construct(modsFileSystem))
                    .Verbose(LoggerInstance)) {
                ModFilter = IdentityModFilter.Instance
            }.Build()
            .AddAttachment(this)
            .AddAttachment<ILogger>(LoggerInstance);
    }

    private void InitMods() {
        try {
            ModLoaderSystem.DiscoverMods();
            ModLoaderSystem.ResolveDependencies();
            ModLoaderSystem.InitMods();
            ModLoaderSystem.Load();
        }
        catch(Exception e) {
            //TODO
            ((ILogger)LoggerInstance).Info(e.StackTrace);
            throw;
        }
    }

    protected override void OnExiting(object sender, ExitingEventArgs args) {
        ((ILogger)LoggerInstance).Info("Shutting down, Goodbye, See you again soon : )");
        LoggerInstance.Dispose();
        base.OnExiting(sender, args);
    }
}