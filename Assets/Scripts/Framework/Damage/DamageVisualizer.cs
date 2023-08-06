using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVisualizer : MonoBehaviour
{
    [SerializeField] Renderer mesh;
    [SerializeField] Color damageColor;
    [SerializeField] float blinkSpeed = 0.1f;
    [SerializeField] string emissionColorPropertyName = "_Addition";
    [SerializeField] HealthComponent healthComponent;

    private Color defaultColor;

    private void Start()
    {
        Material mat = mesh.material;
        mesh.material = new Material(mat);

        defaultColor = mesh.material.GetColor(emissionColorPropertyName);
        healthComponent.onTakeDamage += HealthComponent_onTakeDamage;
    }

    protected virtual void HealthComponent_onTakeDamage(float currentHealth, float delta, float maxHealth, GameObject instigator)
    {
        Color currentColor = mesh.material.GetColor(emissionColorPropertyName);

        if(Mathf.Abs((currentColor - defaultColor).grayscale) <= 0.1f)
        {
            mesh.material.SetColor(emissionColorPropertyName, damageColor);
        }
    }

    private void Update()
    {
        Color currentColor = mesh.material.GetColor(emissionColorPropertyName);
        Color newColor = Color.Lerp(currentColor, defaultColor, blinkSpeed * Time.deltaTime);
        mesh.material.SetColor(emissionColorPropertyName, newColor);
    }
}
