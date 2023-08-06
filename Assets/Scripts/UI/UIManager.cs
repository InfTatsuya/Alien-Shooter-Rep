using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup gameplayCtrl;
    [SerializeField] CanvasGroup gameplayMenu;
    [SerializeField] CanvasGroup shopUI;

    private List<CanvasGroup> allChildren = new List<CanvasGroup>();

    private CanvasGroup currentActiveGroup;

    private void Start()
    {
        List<CanvasGroup> children = new List<CanvasGroup>();
        GetComponentsInChildren(true, children);

        foreach(var child in children)
        {
            if(child.transform.parent == transform)
            {
                allChildren.Add(child);
                SetGroupActive(child, false, false);
            }
        }

        if(allChildren.Count > 0)
        {
            SetCurrentActiveGroup(allChildren[0]);
        }
    }

    private void SetGroupActive(CanvasGroup child, bool interactable, bool visible)
    {
        child.interactable = interactable;
        child.blocksRaycasts = interactable;

        child.alpha = visible ? 1 : 0;  
    }

    private void SetCurrentActiveGroup(CanvasGroup group)
    {
        if(currentActiveGroup != null)
        {
            SetGroupActive(currentActiveGroup, false, false);
        }

        currentActiveGroup = group;
        SetGroupActive(currentActiveGroup, true, true);

    }

    public void SetGameplayControlEnabled(bool enabled)
    {
        SetCanvasGroupEnabled(gameplayCtrl, enabled);
    }

    public void SetGameMenuEnabled(bool enabled)
    {
        SetCanvasGroupEnabled(gameplayMenu, enabled);
    }

    private void SetCanvasGroupEnabled(CanvasGroup group, bool enabled)
    {
        group.interactable = enabled;
        group.blocksRaycasts = enabled;
    }

    public void SwitchToShopUI()
    {
        SetCurrentActiveGroup(shopUI);
        GameplayStatics.SetGamePaused(true);
    }

    public void SwitchToGameplayUI()
    {
        SetCurrentActiveGroup(gameplayCtrl);
        GameplayStatics.SetGamePaused(false);
    }
}
