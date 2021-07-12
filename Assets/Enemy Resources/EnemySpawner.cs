using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] enemies;
    [SerializeField]
    Transform[] spawnPoints;
    // Start is called before the first frame update
   public void SpawnEnemy(){
       Vector3 pos = spawnPoints[Random.Range(0,spawnPoints.Length)].position;
       int enemyPos = Random.Range(0,2);
       GameObject enemy = Instantiate(enemies[enemyPos],pos,Quaternion.identity);
   }
}
