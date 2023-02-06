using System.ComponentModel;
using _Game.Data;
using JetBrains.Annotations;
using Magero.UIFramework;
using VContainer;

public class GeneralCheats
{
    [Inject] private readonly PlayerData _playerData;
    [Inject] private UIFrame _uiFrame;
   
    [Category("Progress")] public int LevelToSet { get; set; }
    
    [Category("Progress"), UsedImplicitly]
    public void SetLevel()
    {
        _playerData.PlayerLevel = LevelToSet;
    }
}