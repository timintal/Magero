using System;
using _Game.Data;
using EasyTweens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] private Image _weaponIcon;
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] TweenAnimation _selectionAnimation;
    [SerializeField] Image _lockIcon;

    [SerializeField] private Button _selectionButton;

    private Action _onSelectedAction;
    private WeaponSettings _settings;

    public WeaponType WeaponType => _settings.Type;

    public void Init(WeaponSettings settings, Action onSelected, bool locked = false)
    {
        _settings = settings;
        _weaponIcon.sprite = settings.WeaponSprite;
        _label.text = settings.WeaponName;

        if (_lockIcon != null)
            _lockIcon.gameObject.SetActive(locked);
        
        _selectionButton.interactable = !locked;
        
        _onSelectedAction = onSelected;
    }

    public void Clear()
    {
        _onSelectedAction = null;
        _weaponIcon.sprite = null;
        _label.text = "";
    }

    void OnEnable()
    {
        _selectionButton.onClick.AddListener(() =>
        {
            if (_selectionAnimation != null)
            {
                _selectionAnimation.PlayForward();
            }

            _onSelectedAction?.Invoke();
        });
    }

    void OnDisable()
    {
        _selectionButton.onClick.RemoveAllListeners();
    }

    public void SetSelected(bool selected)
    {
        if (_selectionAnimation != null)
        {
            if (selected && !_selectionAnimation.IsInEndState)
            {
                _selectionAnimation.PlayForward();
            }
            else if (!selected && !_selectionAnimation.IsInStartState)
            {
                _selectionAnimation.PlayBackward();
            }
        }
    }
}