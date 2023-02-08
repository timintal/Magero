using System;
using EasyTweens;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradableParamView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameLabel;
    [FormerlySerializedAs("_value")] [SerializeField] private TextMeshProUGUI _currentValue;
    [SerializeField] private TextMeshProUGUI _priceLabel;
    [SerializeField] private TextMeshProUGUI _upgradeValue;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TweenAnimation _upgradeAnimation;

    [SerializeField] private Transform _normalRoot;
    [SerializeField] Transform _maxedRoot;

    private Action _onUpgradeAction;
    public void Init(string paramName, string currValue, string upgradeValue, int price, Action onUpgrade, bool isMaxedOut = false)
    {
        _nameLabel.text = paramName;
        _currentValue.text = currValue;
        _upgradeValue.text = upgradeValue;

        _priceLabel.text = $"<sprite name=\"Coin\"> {price.ToString()} UPGRADE";
        
        _onUpgradeAction = onUpgrade;
        
        _normalRoot.gameObject.SetActive(!isMaxedOut);
        _maxedRoot.gameObject.SetActive(isMaxedOut);
    }

    public void Clear()
    {
        _onUpgradeAction = null;
        _nameLabel.text = "";
        _currentValue.text = "";
        _upgradeValue.text = "";
    }

    void OnEnable()
    {
        _upgradeButton.onClick.AddListener(() =>
        {
            if (_upgradeAnimation != null)
            {
                _upgradeAnimation.PlayForward();
            }

            _onUpgradeAction?.Invoke();
        });
    }

    void OnDisable()
    {
        _upgradeButton.onClick.RemoveAllListeners();
    }    
}