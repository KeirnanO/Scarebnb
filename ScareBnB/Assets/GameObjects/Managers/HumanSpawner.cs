using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HumanSpawner : MonoBehaviour
{
    public static HumanSpawner instance;

    private void Awake()
    {
        instance = this;
    }

    public Human regularHuman;
    public Transform spawnPoint;

    public int spawnCount = 3;
    public float spawnTime = 2f;
    public bool spawning = true;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            yield return new WaitForSeconds(spawnTime);

            SpawnHuman();
        }
    }

    public void SpawnHuman()
    {
        Instantiate(regularHuman, spawnPoint);        
    }
}
