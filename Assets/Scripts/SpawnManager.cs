using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float spawnRange = 100;
    public GameObject[] enemyPrefabs;
    public GameObject[] powerupPrefabs;
    private int waveNumber = 1;
    // Start is called before the first frame update
    void Start()
    {
        SpawnRandomEnemy(2);
    }

    // Update is called once per frame
    void Update()
    {
        int count = FindObjectsOfType<Enemy>().Length;
        if(count == 0)
        {
            waveNumber++;
            SpawnRandomEnemy(waveNumber);
            int random = Random.Range(0,2);
            Instantiate(powerupPrefabs[random],GenerateSpawnPosition(),Quaternion.identity);
        }
    }
    void SpawnRandomEnemy(int numberToSpawn)
    {
        for(int i = 0; i < numberToSpawn; i++)
        {
            int random = Random.Range(0,2);
            Instantiate(enemyPrefabs[random],GenerateSpawnPosition(),Quaternion.identity);
        }
    }
    Vector3 GenerateSpawnPosition()
    {
        float spawnX = Random.Range(-spawnRange,spawnRange);
        float spawnZ = Random.Range(-spawnRange,spawnRange);
        return  new Vector3 (spawnX,0.2f,spawnZ);

    }
}
