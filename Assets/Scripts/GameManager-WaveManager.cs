using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager {



    public IEnumerator SpawnEnemyLoop(int waveIndex){

        // Reset enemy spawn counters
        foreach(WaveEnemyData enemy in waveData[waveIndex].enemy)
            enemy.totalSpawned = enemy.spawnCounter;
        
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        
        while (AllEnemiesSpawned(waveIndex)){

            int nextEnemyIndex = GetNextEnemyIndex(waveIndex);

            // Spawn enemy at random spawn point
            SpawnEnemy( waveData[waveIndex].enemy[nextEnemyIndex].enemyPrefab, 
                        spawnPoints[Random.Range (0, spawnPoints.Length)].transform);

            // Spawn delay
            yield return new WaitForSeconds(waveData[waveIndex].enemy[nextEnemyIndex].spawnDelay);
        }

        NextLevel();
    }


    // Check to see if all enemies have been spawned
    bool AllEnemiesSpawned(int waveIndex){

        // Check for enemy whos counter is not yet zero
        foreach(WaveEnemyData enemy in waveData[waveIndex].enemy)
            if (enemy.totalSpawned!=0) return false;
        
        return true;
    }


    // Check to see if all enemies have been spawned
    int GetNextEnemyIndex(int waveIndex){

        // Get list of enemies whos counters are not 0
        List<int> enemyIndexList = new List<int>();
        foreach(WaveEnemyData enemy in waveData[waveIndex].enemy)
            if (enemy.totalSpawned!=0)
                enemyIndexList.Add(waveData[waveIndex].enemy.IndexOf(enemy));

        // Randomly select index to spawn
        return enemyIndexList[Random.Range (0, enemyIndexList.Count)];
    }


    // Spawns enemy at random spawn point
    void SpawnEnemy(GameObject enemy, Transform spawnPoint){

    }
}
