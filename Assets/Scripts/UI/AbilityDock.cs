using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityDock : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] AbilityComponent abilityComponent;
    [SerializeField] RectTransform root;
    [SerializeField] VerticalLayoutGroup verticalLayoutGroup;
    [SerializeField] AbilityUI abilityUIPrefab;

    [SerializeField] float scaleRange = 200f;

    [SerializeField] float highlightSize = 1.5f;
    [SerializeField] float scaleSpeed = 20f;

    private Vector3 goalScale = Vector3.one;

    private List<AbilityUI> abilityUIs = new List<AbilityUI>();

    private PointerEventData touchData;
    private AbilityUI highlightedAbilityUI;

    private void Awake()
    {
        abilityComponent.onNewAbilityAdded += AbilityComponent_onNewAbilityAdded;
    }

    private void AbilityComponent_onNewAbilityAdded(Ability newAbility)
    {
        AbilityUI newAbilityUI = Instantiate(abilityUIPrefab, root);
        newAbilityUI.Init(newAbility);

        abilityUIs.Add(newAbilityUI);
    }

    private void Update()
    {
        if(touchData != null)
        {
            GetUIUnderPointer(touchData, out highlightedAbilityUI);

            ArrageScale(touchData);
        }

        transform.localScale = Vector3.Lerp(transform.localScale, goalScale, scaleSpeed * Time.deltaTime);
    }

    private void ArrageScale(PointerEventData touchData)
    {
        if (scaleRange == 0) return;

        float touchVerticalPos = touchData.position.y;

        foreach(var abilityUI in abilityUIs)
        {
            float abilityUIVerticalPos = abilityUI.transform.position.y;
            float distance = Mathf.Abs(abilityUIVerticalPos - touchVerticalPos);

            if(distance > scaleRange)
            {
                abilityUI.SetScaleAmt(0f);
                continue;
            }

            float scaleAmt = (scaleRange - distance) / scaleRange;
            abilityUI.SetScaleAmt(scaleAmt);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touchData = eventData;

        goalScale = Vector3.one * highlightSize;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(highlightedAbilityUI != null)
        {
            highlightedAbilityUI.ActivateAbility();
        }

        touchData = null;

        ResetScale();
        goalScale = Vector3.one;
    }

    private void ResetScale()
    {
        foreach(var abilityUI in abilityUIs)
        {
            abilityUI.SetScaleAmt(0f);
        }
    }

    private bool GetUIUnderPointer(PointerEventData eventData, out AbilityUI abilityUI)
    {
        List<RaycastResult> findUIs = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, findUIs);

        abilityUI = null;

        foreach(var result in findUIs)
        {
            abilityUI = result.gameObject.GetComponent<AbilityUI>();

            if(abilityUI != null)
            {
                return true;
            }
        }

        return false;
    }
}
