using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager {



    public IEnumerator SpawnEnemyLoop(int waveIndex){

        // Which generation of game are we on
        int lifeIndex = stats.timesDied;

        // Reset enemy spawn counters
        foreach(WaveEnemyData enemy in waveData[waveIndex].enemy)
            enemy.totalSpawned = enemy.spawnCounter;
        
        yield return new WaitForSeconds(1);
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        
        // Spawn enemies
        while (!AllEnemiesSpawned(waveIndex)){

            int nextEnemyIndex = GetNextEnemyIndex(waveIndex);

            // Spawn enemy at random spawn point
            SpawnEnemy( waveData[waveIndex].enemy[nextEnemyIndex], 
                        spawnPoints[Random.Range (0, spawnPoints.Length)].transform);

            // Decriment spawn counter
            waveData[waveIndex].enemy[nextEnemyIndex].totalSpawned --;

            // Spawn delay
            yield return new WaitForSeconds(waveData[waveIndex].enemy[nextEnemyIndex].spawnDelay);

            // Check if player is dead
            if (PlayerHasDied(lifeIndex)) yield break;
        }


        // Wait for all enemies to be defeated
        while (!AllEnemiesDefeated()){

            if (PlayerHasDied(lifeIndex)) yield break;

            yield return new WaitForSeconds(1);

        }


        if (PlayerHasDied(lifeIndex)) yield break;

        NextLevel();

        yield return null;
    }




    // Checks to see if the player has died during SpawnEnemyLoop
    bool PlayerHasDied(int lifeIndex){

        // Check if the logged death counter doesn't allign
        // with the current death counter, & end the wave if true
        if (stats.timesDied!=lifeIndex) return true;
        else return false;
    }


    // Check to see if all enemies have been spawned
    bool AllEnemiesSpawned(int waveIndex){

        // Check for enemy whos counter is not yet zero
        foreach(WaveEnemyData enemy in waveData[waveIndex].enemy)
            if (enemy.totalSpawned!=0) return false;
        
        return true;
    }

    // Check to see if all enemies have been defeated
    bool AllEnemiesDefeated(){

        if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0) return false;
        else return true;
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
    void SpawnEnemy(WaveEnemyData enemy, Transform spawnPoint){

        // Debug.Log("Spawning Enemy: " + enemy.enemyPrefab.name);

        GameObject newEnemy = Instantiate(enemy.enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity); // Spawn the player in position
        newEnemy.name = enemy.enemyPrefab.name;

        newEnemy.transform.position = spawnPoint.transform.position;

        // Set fire speed
        float fireSpeed = Random.Range(enemy.shootFrequencyMin, enemy.shootFrequencyMax);
        newEnemy.GetComponent<EnemyProjectileEmitter>().shootFrequency = fireSpeed;
    }
}
