using JustLoaded.Core;
using JustLoaded.Core.Loading;
using JustLoaded.Util;

namespace CosmosCosmini.Core;

using static CoreMod.Keys.Phase;

public static class LoadingPhaseExtensions {

    public static OrderedResolver<ILoadingPhase>.Registration AssetLoadPhase(this OrderedResolver<ILoadingPhase>.Registration reg) {
        return reg.WithOrder(RegisterDb, Order.After)
            .WithOrder(AssetLoadEnd, Order.Before);
    }
    
    
}