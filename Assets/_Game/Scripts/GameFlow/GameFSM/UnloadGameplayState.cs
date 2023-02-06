using _Game.Flow;
using Cysharp.Threading.Tasks;
using Entitas.VisualDebugging.Unity;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class UnloadGameplayState : FSMState
{
    private readonly Camera _uiCamera;

    public UnloadGameplayState(Camera uiCamera)
    {
        _uiCamera = uiCamera;
    }
    
    internal override async void OnEnter()
    {
        await SceneManager.UnloadSceneAsync("TestLevel01");
        
        _uiCamera.GetComponent<UniversalAdditionalCameraData>().renderType = CameraRenderType.Base;
        
#if UNITY_EDITOR
        var contextObserverBehaviours = GameObject.FindObjectsOfType<ContextObserverBehaviour>();
        foreach (var observerBehaviour in contextObserverBehaviours)
        {
            Object.Destroy(observerBehaviour.gameObject);
        }
#endif
        
        _parentFSM.GoTo<MainMenuState>();
    }
}