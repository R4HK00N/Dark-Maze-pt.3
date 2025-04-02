using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] spawnpoints;
    public List<GameObject> monsters = new List<GameObject>();
    public List<AudioSource> monsterAudio = new List<AudioSource>();
    public int enemyAmount;

    public void SpawnEnemies()
    {
        for(int i = 0; i < enemyAmount; i++)
        {
            monsters.Add(Instantiate(enemyPrefab, spawnpoints[Random.Range(0, spawnpoints.Length)].transform.position, Quaternion.identity));
        }

        foreach (GameObject monster in monsters)
            monsterAudio.Add(monster.GetComponent<AudioSource>());
    }
    public void Update()
    {
        foreach(GameObject monster in monsters)
        {
            if (!monster.GetComponent<JumpscareBehavior>().canJumpScare)
            {
                foreach(AudioSource audio in monsterAudio)
                {
                    audio.Stop();
                }
            }
        }
    }
}
