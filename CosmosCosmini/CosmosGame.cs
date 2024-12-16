using System.Diagnostics.CodeAnalysis;
using CosmosCosmini.JustLoadedEx;
using Custom2d_Engine.Input;
using Custom2d_Engine.Rendering;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Ticking;
using JustLoaded.Core;
using JustLoaded.Core.Discovery;
using JustLoaded.Filesystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
    [NotNull] public ModLoaderSystem? ModLoaderSystem { get; private set; }
    
    public CosmosGame() { 
        _graphicsManager = new GraphicsDeviceManager(this);
        _graphicsManager.GraphicsProfile = GraphicsProfile.HiDef;
        _graphicsManager.HardwareModeSwitch = false;
        Content.RootDirectory = "Content";
    }

    protected override void LoadContent() {
        base.LoadContent();
        
        RenderPipeline = new RenderPipeline();
        PhysicsWorld = new World();
        GlobalTickManager = new TickManager();
        
        GameScene = new Hierarchy(GlobalTickManager);
        GameCamera = new Camera { ViewSize = 10 };
        GameScene.AddObject(GameCamera);
        
        Ui = new Hierarchy(GlobalTickManager);
        Ui.AddObject(UiCamera);
        
        Input = new InputManager(Window);
        
        RenderPipeline.Init(GraphicsDevice);
        Effects.Init(Content);
        
        CreateMls();

        InitMods();
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