using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnComponent : MonoBehaviour
{
    [SerializeField] GameObject[] objectToSpawn;
    [SerializeField] Transform spawnPoint;


    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public bool StartSpawn()
    {
        if (objectToSpawn.Length == 0) return false;

        if(anim != null)
        {
            anim.SetTrigger(StringCollector.spawnAnim);
        }
        else
        {
            SpawnImplementation();
        }

        return true;
    }

    private void SpawnImplementation()
    {
        int randomIndex = Random.Range(0, objectToSpawn.Length);

        GameObject newSpawn = Instantiate(objectToSpawn[randomIndex], spawnPoint.position, spawnPoint.rotation);
        
        ISpawnInterface spawnInterface = newSpawn.GetComponent<ISpawnInterface>();
        if(spawnInterface != null )
        {
            spawnInterface.SpawnBy(gameObject);
        }
    
    }
}
