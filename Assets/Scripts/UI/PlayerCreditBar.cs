using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCreditBar : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] Button shopButton;
    [SerializeField] TextMeshProUGUI creditText;
    [SerializeField] CreditComponent creditComponent;

    private void Start()
    {
        shopButton.onClick.AddListener(OpenShop);

        creditComponent.onCreditChanged += CreditComponent_onCreditChanged;
        UpdateCredit(creditComponent.Credits);
    }

    private void CreditComponent_onCreditChanged(int newCredits)
    {
        UpdateCredit(newCredits);
    }

    private void UpdateCredit(int newCredits)
    {
        creditText.text = $"${newCredits}";
    }

    private void OpenShop()
    {
        uiManager.SwitchToShopUI();
    }
}
