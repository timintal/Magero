using _Game.Flow;
using JetBrains.Annotations;
using Magero.UIFramework;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class LevelFinishedScreen : UIScreen
{
    [SerializeField] private Button _nextButton;
    private GameFSM _gameFSM;

    [Inject, UsedImplicitly]
    public void SetDependencies(GameFSM gameFsm)
    {
        _gameFSM = gameFsm;
    }
    
    void OnEnable()
    {
        _nextButton.onClick.AddListener(NextButtonClicked);
    }

    void OnDisable()
    {
        _nextButton.onClick.RemoveAllListeners();
    }

    private void NextButtonClicked()
    {
        _gameFSM.GoTo<UnloadGameplayState>();
    }
}