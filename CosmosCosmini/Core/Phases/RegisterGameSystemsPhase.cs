using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Core.Loading;
using JustLoaded.Loading;

namespace CosmosCosmini.Core.Phases;

public class RegisterGameSystemsPhase : EntrypointLoadingPhase<IGameSystemRegisterer> {
    
    private OrderedResolver<IGameSystem>? _systems;

    protected override void Setup(ModLoaderSystem modLoader) {
        _systems = new OrderedResolver<IGameSystem>();
    }

    protected override void HandleEntrypointFor(Mod mod, IGameSystemRegisterer entrypoint, ModLoaderSystem modLoader) {
        var systems = _systems!;
        entrypoint.RegisterSystems(modLoader, systems);
    }

    protected override void Finish(ModLoaderSystem modLoader) {
        var db = (ArrayDatabase<IGameSystem>)modLoader.MasterDb.GetDatabase<IGameSystem>();
        
        db.Init(_systems!.Resolve());
        _systems = null;
    }
}