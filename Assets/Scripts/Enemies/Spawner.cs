using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VFXSpec
{
    public ParticleSystem particleSystem;
    public float size;
}


public class Spawner : Enemy
{
    [SerializeField] VFXSpec[] deathVFXs;

    protected override void Dead()
    {
        foreach(VFXSpec vfx in deathVFXs)
        {
            ParticleSystem particleEffect = Instantiate(vfx.particleSystem);
            particleEffect.transform.position = transform.position;
            particleEffect.transform.localScale = Vector3.one * vfx.size;
        }
    }
}
