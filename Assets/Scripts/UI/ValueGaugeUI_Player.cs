using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueGaugeUI_Player : MonoBehaviour
{
    [SerializeField] Image valueFill;
    [SerializeField] TextMeshProUGUI valueText;

    public void UpdateValueBar(float currentValue, float delta, float maxValue)
    {
        valueFill.fillAmount = currentValue / maxValue;
        valueText.text = $"{(int)currentValue} / {maxValue}";
    }
}
