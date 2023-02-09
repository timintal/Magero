using _Game.Flow;
using JetBrains.Annotations;
using Magero.UIFramework;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GamePlayScreen : UIScreen
{
    [SerializeField] private Button _backButton;
    [SerializeField] Slider _healthBar;

    public Slider HealthBar => _healthBar;

    private GameFSM _gameFSM;

    [Inject, UsedImplicitly]
    public void SetDependencies(GameFSM gameFsm)
    {
        _gameFSM = gameFsm;
    }
    
    void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    void OnDisable()
    {
        _backButton.onClick.RemoveAllListeners();

    }

    private void OnBackButtonClicked()
    {
        _gameFSM.GoTo<UnloadGameplayState>();
    }
}
