using DG.Tweening;
using UnityEngine;

public class ResetDoTween : MonoBehaviour, IPoolReset
{
    [SerializeField] private DOTweenAnimation[] _animations;
    public void ResetForReuse()
    {
        foreach (var doTweenAnimation in _animations)
        {
            doTweenAnimation.DORestart();
        }
    }
}