using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public partial class GameManager : MonoBehaviour
{

    // Player Stats
    [System.Serializable] public class PlayerStats{
        public int hp = 5000;           // Remaining HP
        public int hpMax = 5;           // Max HP value 
        public int enemiesKilled = 0;   // Number of enemies Defeated
        public float timeSurvived = 0;  // How long the player has survived for
        public float currentWave = 0;   // How long the player has survived for
        public float score = 0;
        public float highscore = 0;
        public int timesDied = 0;       // Times played flag
        public bool newHighscore;       // Highscore flag
    }
    public PlayerStats stats;


    // Screen Data
    [System.Serializable] public class ScreenData{
        public GameObject mainMenu;
        public GameObject playerHud;
        public GameObject gameOver;
        public GameObject leaderboard;
        public GameObject credits;
        public TextMeshProUGUI gameOverScore;
        public TextMeshProUGUI mainMenuScore;
        public TextMeshProUGUI platinumDeath;
    }
    public ScreenData screen;


    // Leader board Stats
    [System.Serializable] public class LeaderboardData{
        public int wave;
        public float time;
        public float score;
    }
    public List<LeaderboardData> leaderboard = new List<LeaderboardData>();


    // Wave System
    [System.Serializable] public class WaveEnemyData{
        public GameObject enemyPrefab;
        [Range(0, 50)] public int spawnCounter;    // Number spawned
        [Range(0, 50)] public int spawnDelay;      // Seconds between each spawn
        [Range(0.1f, 10f)] public float shootFrequencyMin = 1f;
        [Range(0.1f, 10f)] public float shootFrequencyMax = 2f;
        public int totalSpawned; // Total number that have been spawned thus far
    }
    [System.Serializable] public class WaveData{
        public GameObject levelPrefab;
        public List<WaveEnemyData> enemy = new List<WaveEnemyData>();
    }
    public List<WaveData> waveData = new List<WaveData>();


    // Additional Refs
    public GameObject playerPrefab;         // Prefab of the player
    public CameraMovement CameraMovement;
    private GameObject currentPlayer;   
    private GameObject currentLevel;
    private bool playing = false;           // True when the player is spawned in
    private int currentLevelIndex = 0;

    // Initialization
    void Awake(){

        // Disable camera movement on startup
        DisablePlayerCamera();

        GetMainMenu();
    }






    //=================//
    // GAME MANAGEMENT //
    //=================//

    void Update(){
        if (playing){

            // Check for player death
            if (stats.hp<=0) EndGame();

            // Time survived
            stats.timeSurvived += Time.deltaTime;
            
        }
    }

    void FixedUpdate(){
            // Score calculation
            stats.score += (int)(Time.fixedDeltaTime * 100);

    }

    public void HideAllScreens(){
        screen.playerHud.SetActive(false);
        screen.mainMenu.SetActive(false);
        screen.gameOver.SetActive(false);
        screen.credits.SetActive(false);
    }

    public void StartGame(){

        Debug.Log("Starting game!");
        Debug.Log("waveData.Count: "+waveData.Count);


        // Destroy any enemies left in scene
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) Destroy (enemy);

        // Initialize data
        stats.hp = stats.hpMax;
        stats.currentWave = 0;
        stats.timeSurvived = 0;
        stats.newHighscore = false;
        stats.score = 0;

        // Set HUD active and hide menu
        HideAllScreens();
        screen.playerHud.SetActive(true);
        
        StartLevel(0);

        playing = true;
    }

    public void EndGame(){
        
        Debug.Log("PLATINUM DEATH!");

        playing = false;

        Destroy(currentPlayer);
        DisablePlayerCamera();
        GetDeathScreen();
    }

    public void EndGameWin(){
        
        Debug.Log("YOU WIN!");

        playing = false;

        Destroy(currentPlayer);
        DisablePlayerCamera();
        GetDeathScreen();       // TODO: Set win screen
    }


    public void SpawnPlayer(){

        currentPlayer = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity); // Spawn the player in position
        currentPlayer.name = "Player";
    }

    public void ResetPlayerPosition(){
        GameObject spawnTarget = GameObject.Find("PlayerSpawnPoint"); // Search for spawn marker in word
        if (!spawnTarget) {
            Debug.LogError("Error: No spawn point could be found in the scene!",this); 
            return;
        }
        currentPlayer.transform.position = spawnTarget.transform.position;

        // Destroy any web left in the scene
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Web")) Destroy (enemy);
    }

    // Replace last level in the scene (if present) with the current level map
    public void StartLevel(int levelIndex){

        Debug.Log("BEGINNING LEVEL: "+levelIndex);

        // Set current level undex
        currentLevelIndex = levelIndex;

        // Destroy level if present
        if (currentLevel) Destroy(currentLevel);

        // Create new level
        GameObject newLevel = Instantiate(waveData[levelIndex].levelPrefab, new Vector3(0f,0f,0f), Quaternion.identity);
        newLevel.transform.localScale = new Vector3(1f,1f,1f);
        newLevel.transform.localPosition = new Vector3(0f,0f,0f);

        SpawnPlayer();
        ResetPlayerPosition();
        EnablePlayerCamera();

        StartCoroutine(SpawnEnemyLoop(levelIndex));
    }

    public void NextLevel(){

        Debug.Log("LEVEL COMPLETE!");

        // Checks to if all levels have been completed
        if (currentLevelIndex+1 == waveData.Count) EndGameWin(); // Win state
        else StartLevel(currentLevelIndex+1); // Next level 
    }

    public void EnablePlayerCamera(){

        if (currentPlayer){

            // Reposition Cam
            CameraMovement.transform.position = new Vector3(currentPlayer.transform.position.x,
                                                            currentPlayer.transform.position.y+7f,
                                                            currentPlayer.transform.position.z-7f);

            CameraMovement.enabled = true;
            CameraMovement.SetTarget(currentPlayer);
        }else{
            Debug.LogError("Error: No player present. CameraMovement can not be enabled at this time.", this);
            DisablePlayerCamera();
        }
    }

    public void DisablePlayerCamera(){

        CameraMovement.enabled = false;
    }






    //=================//
    // MENU MANAGEMENT //
    //=================//

    // Screen to be displayed on death
    public void GetDeathScreen(){
        HideAllScreens();
        screen.gameOver.SetActive(true);


        screen.gameOverScore.text = "";
        if (stats.score > stats.highscore){
            stats.newHighscore = true;
            screen.gameOverScore.text += "<color=#14FFRW>NEW HIGH SCORE!</color>" + "\n";
            screen.gameOverScore.text += "SCORE: " + stats.score + "\n";

            // Set new highscore
            stats.highscore = stats.score;

        }else{
            screen.gameOverScore.text += "SCORE: " + stats.score + "\n";
            screen.gameOverScore.text += "<color=#14FFRW>HIGHSCORE TO BEAT: </color>"+stats.highscore + "\n";
        }

        // Get timer time
        int minutes = Mathf.FloorToInt(stats.timeSurvived / 60f);
        int seconds = Mathf.FloorToInt(stats.timeSurvived - minutes * 60);
        string survivalTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        screen.gameOverScore.text += "SURVIVED FOR: " + survivalTime;
    }

    // Screen to be displayed on startup
    public void GetMainMenu(){
        HideAllScreens();
        screen.mainMenu.SetActive(true);

        screen.mainMenuScore.text = "Highscore: " + stats.highscore;
    }

    // Screen to be displayed on startup
    public void GetCredits(){
        HideAllScreens();
        screen.credits.SetActive(true);
    }
}
