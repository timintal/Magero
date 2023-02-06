using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] private Image _weaponIcon;
    [SerializeField] private TextMeshProUGUI _label;

    [SerializeField] private Button _selectionButton;

    private Action OnSelectedAction;

    public void Init(Sprite icon, string label, Action onSelected)
    {
        _weaponIcon.sprite = icon;
        _label.text = label;
        OnSelectedAction = onSelected;
    }
    
    void OnEnable()
    {
        _selectionButton.onClick.AddListener(() => OnSelectedAction?.Invoke());
    }

    void OnDisable()
    {
        _selectionButton.onClick.RemoveAllListeners();
    }
}