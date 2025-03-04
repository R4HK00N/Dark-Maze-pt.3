using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] spawnpoints;
    public int enemyAmount;
    public void SpawnEnemies()
    {
        for(int i = 0; i < enemyAmount; i++)
        {
            Instantiate(enemyPrefab, spawnpoints[Random.Range(0, spawnpoints.Length)].transform.position, Quaternion.identity);
        }
    }
}
