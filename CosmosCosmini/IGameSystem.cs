using Custom2d_Engine.Scenes;
using JustLoaded.Core;
using Microsoft.Xna.Framework;

namespace CosmosCosmini;

public interface IGameSystem {

    public void InitHierarchy(Hierarchy gameHierarchy);
    
    public void Update(GameTime gameTime, Hierarchy gameHierarchy);
    
    //TODO
    public void SaveState();
    
    //TODO
    public void LoadState();
    
}