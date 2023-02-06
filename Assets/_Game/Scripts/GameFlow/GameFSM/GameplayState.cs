using _Game.Flow;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameplayState : FSMState
{
    private readonly Camera _uiCamera;

    public GameplayState(Camera uiCamera)
    {
        _uiCamera = uiCamera;
    }
    
    internal override async void OnEnter()
    {
        _uiCamera.GetComponent<UniversalAdditionalCameraData>().renderType = CameraRenderType.Overlay;

        _uiFrame.Open<GamePlayScreen>();

        var loadSceneAsync = SceneManager.LoadSceneAsync("TestLevel01", LoadSceneMode.Additive);
        loadSceneAsync.allowSceneActivation = true;
        await loadSceneAsync;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("TestLevel01"));

    }

    internal override void OnExit()
    {
        _uiFrame.Close<GamePlayScreen>();
        
    }
}