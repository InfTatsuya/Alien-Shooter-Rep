using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarComponent : MonoBehaviour
{
    private static InGameUI inGameUI;

    [SerializeField] HealthBarUI_Enemy healthBarToSpawn;
    [SerializeField] Transform healthBarAttachPoint;

    private HealthComponent healthComponent;

    private void Start()
    {
        if(inGameUI == null)
        {
            inGameUI = FindObjectOfType<InGameUI>();
        }

        HealthBarUI_Enemy newHealthBar = Instantiate(healthBarToSpawn, inGameUI.transform);
        newHealthBar.Init(healthBarAttachPoint);

        healthComponent = GetComponent<HealthComponent>();
        healthComponent.onHealthChanged += newHealthBar.SetHealthFill;
        healthComponent.onHealthEmpty += newHealthBar.OnOwnerDead;
    }
}
