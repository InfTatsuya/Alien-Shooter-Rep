using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Image cooldownImage;

    [SerializeField] float highlightSize = 1.5f;
    [SerializeField] float highlightOffset = 200f;
    [SerializeField] float scaleSpeed = 20f;
    [SerializeField] RectTransform offsetPivot;

    private Ability ability;

    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    private Vector3 goalScale = Vector3.one;
    private Vector3 goalOffset = Vector3.zero;

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, goalScale, scaleSpeed * Time.deltaTime);
        offsetPivot.localPosition = Vector3.Lerp(offsetPivot.localPosition, goalOffset, scaleSpeed * Time.deltaTime);
    }

    public void SetScaleAmt(float amt)
    {
        goalScale = Vector3.one * (1 + (highlightSize - 1) * amt);
        goalOffset = Vector3.left * highlightOffset * amt;
    }

    public void Init(Ability newAbility)
    {
        ability = newAbility;
        iconImage.sprite = ability.AbilityIcon;
        cooldownImage.enabled = false;

        ability.onCooldownStarted += Ability_onCooldownStarted;
    }

    private void Ability_onCooldownStarted()
    {
        if (isOnCooldown) return;

        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        isOnCooldown = true;

        cooldownImage.enabled = true;
        cooldownImage.fillAmount = 1f;

        cooldownTimer = ability.CooldownDuration;
        
        while(cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownImage.fillAmount = cooldownTimer / ability.CooldownDuration;
            
            yield return null;
        }

        isOnCooldown = false;

        cooldownImage.fillAmount = 0f;
        cooldownImage.enabled = false;
    }

    public void ActivateAbility()
    {
        ability.ActivateAbility();
    }
}
