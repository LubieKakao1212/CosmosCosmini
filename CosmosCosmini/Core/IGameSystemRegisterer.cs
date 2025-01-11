using JustLoaded.Core;

namespace CosmosCosmini.Core;

public interface IGameSystemRegisterer {

    public void RegisterSystems(ModLoaderSystem modLoader, OrderedResolver<IGameSystem> systems);


}