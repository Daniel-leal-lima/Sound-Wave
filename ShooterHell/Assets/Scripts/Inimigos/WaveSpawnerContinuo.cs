using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnerContinuo : MonoBehaviour
{
    public bool repeat;
    public bool waiting;
    public List<Enemy> enemies = new List<Enemy>();
    public int currWave;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public Transform[] spawnLocations;
    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;


    // Update is called once per frame
    void FixedUpdate()
    {
            if (spawnTimer <= 0)
            {
                //spawn an enemy
                if (enemiesToSpawn.Count > 0)
                {
                    Instantiate(enemiesToSpawn[0], spawnLocations[Random.Range(0, spawnLocations.Length)].position, Quaternion.identity); // spawn first enemy in our list
                    enemiesToSpawn.RemoveAt(0); // and remove it
                    spawnTimer = spawnInterval;
                }
                else
                {
                    waveTimer = 0; // if no enemies remain, end wave
                }
            }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
        if ((repeat) && (!waiting))
        {
            if (GameObject.FindGameObjectWithTag("Inimigo")==null)
            {
                waveDuration = Random.Range(10, 20);
                GenerateWave();
            }
        }
        
    }

    public void GenerateWave()
    {
        waveValue = currWave * 10;
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
        waveTimer = waveDuration; // wave duration is read only
    }

    public void KillWave()
    {
        GameObject[] listaInimigos = GameObject.FindGameObjectsWithTag("Inimigo");
        foreach(GameObject obj in listaInimigos)
        {
            Destroy(obj);
        }
    }

    public void Resume()
    {
        waiting = false;
    }
    public void GenerateEnemies()
    {
        // Create a temporary list of enemies to generate
        // 
        // in a loop grab a random enemy 
        // see if we can afford it
        // if we can, add it to our list, and deduct the cost.

        // repeat... 

        //  -> if we have no points left, leave the loop

        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if (waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if (waveValue <= 0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}
