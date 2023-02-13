using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class PassiveIncomeView : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _incomeText;
    [SerializeField] Button _claimButton;
    
    [Inject] private PassiveIncomeService _passiveIncomeService;
    
    private void Start()
    {
        _claimButton.onClick.AddListener(() =>
        {
            _passiveIncomeService.ClaimIncome();
            UpdateView();
        });
        StartCoroutine(UpdateViewCoroutine());
    }
    
    IEnumerator UpdateViewCoroutine()
    {
        var wait = new WaitForSeconds(1f);
        while (true)
        {
            UpdateView();
            yield return wait;
        }
    }

    private void UpdateView()
    {
        _incomeText.text = $"<sprite name=\"Coin\"> {_passiveIncomeService.GetPassiveIncome().ToString()}";
    }
}
