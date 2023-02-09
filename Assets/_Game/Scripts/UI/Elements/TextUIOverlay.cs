using EasyTweens;
using TMPro;
using UnityEngine;

public class TextUIOverlay : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    [SerializeField] private TweenAnimation _tweenAnimation;

    private float shownValue;

    public void ShowOverlayForValue(float value)
    {
        if (!Mathf.Approximately(shownValue, value))
        {
            _text.text = value.ToString("F1");
            shownValue = value;
        }

        _tweenAnimation.PlayForward();
    }
}