using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI_Enemy : MonoBehaviour
{
    [SerializeField] Image healthFill;

    private Transform attachPoint;

    public void Init(Transform attachPoint)
    {
        this.attachPoint = attachPoint;
    }

    public void SetHealthFill(float currentHealth, float delta, float maxHealth)
    {
        healthFill.fillAmount = currentHealth / maxHealth;
    }

    public void OnOwnerDead(GameObject killer)
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(attachPoint.position);
    }
}
