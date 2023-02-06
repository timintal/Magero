using _Game.Flow;
using JetBrains.Annotations;
using Magero.UIFramework.Components.NavBar;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MapScreen : NavBarScreen
{
    [SerializeField] private Button _playButton;
    private GameFSM _fsm;

    [Inject, UsedImplicitly]
    public void SetDependencies(GameFSM fsm)
    {
        _fsm = fsm;
    }
    
    void OnEnable()
    {
        _playButton.onClick.AddListener(PlayButtonClicked);
    }

    void OnDisable()
    {
        _playButton.onClick.RemoveAllListeners();
    }

    private void PlayButtonClicked()
    {
        _fsm.GoTo<GameplayState>();
    }
}
