using System.ComponentModel;
using _Game.Data;
using Magero.UIFramework;
using VContainer;

public class GameplayCheats 
{
    [Inject] private readonly PlayerData _playerData;
    [Inject] private UIFrame _uiFrame;
    [Inject] private Contexts _contexts;
   
    // [Category("Economy")] public int CoinsToSet { get; set; }
    //
    // [Category("Economy")]
    // public void SetCoins()
    // {
    //     _playerData.SetCoins(CoinsToSet);
    // }
    //
    [Category("Gameplay")]
    public void Win()
    {
        var ent = _contexts.game.CreateEntity();
        ent.AddLevelFinished(true);
        
    }

    [Category("Gameplay")]
    public void Lose()
    {
        var ent = _contexts.game.CreateEntity();
        ent.AddLevelFinished(false);
    }
    
	    
    
    
    
    
}