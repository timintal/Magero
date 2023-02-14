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
    
    [Category("Progress")] public int ExpToAdd { get; set; }
    [Category("Progress"), UsedImplicitly]
    public void AddExp()
    {
        _playerData.PlayerExp += ExpToAdd;
    }
    
    [Category("Economy"), UsedImplicitly]
    public void AddCoins100()
    {
        _playerData.Coins += 100;
    }
    
    [Category("Economy"), UsedImplicitly]
    public void AddCoins1000()
    {
        _playerData.Coins += 1000;
    }
}