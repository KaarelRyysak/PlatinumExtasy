using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GameManager : MonoBehaviour
{

    [System.Serializable] public class PlayerStats{
        public int hp = 5;              // Remaining HP
        public int hpMax = 5;           // Max HP value 
        public int enemiesKilled = 0;   // Number of enemies Defeated
        public float timeSurvived = 0;  // How long the player has survived for
        public float currentWave = 0;   // How long the player has survived for
        public float score = 0;
        public float highscore = 0;
        public bool newHighscore;       // Highscore flag
    }
    public PlayerStats stats;


    [System.Serializable] public class ScreenData{
        public GameObject mainMenu;
        public GameObject playerHud;
        public GameObject gameOver;
        public GameObject leaderboard;
        public TextMeshProUGUI gameOverScore;
        public TextMeshProUGUI mainMenuScore;
        // public TextMeshProUGUI gameOverHighscore;
        // public TextMeshProUGUI gameOverLeaderboard;
    }
    public ScreenData screen;


    [System.Serializable] public class LeaderboardData{
        public int wave;
        public float time;
        public float score;
    }
    public List<LeaderboardData> leaderboard = new List<LeaderboardData>();


    [System.Serializable] public class WaveData{
        GameObject levelPrefab;
    }
    public List<WaveData> waveData = new List<WaveData>();


    public GameObject playerPrefab;         // Prefab of the player
    public CameraMovement CameraMovement;
    private GameObject currentPlayer;   
    private GameObject currentLevel;
    private bool playing = false;           // True when the player is spawned in



    void Awake(){

        // Disable camera movement on startup
        DisablePlayerCamera();

        GetMainMenu();
    }


    void Update(){
        if (playing){

            // Check for player death
            if (stats.hp<=0) EndGame();

            // Time survived
            stats.timeSurvived += Time.deltaTime;
            
            // Score calculation
            stats.score += (int)(stats.timeSurvived * 10);
        }
    }

    public void StartGame(){

        // Initialize data
        stats.hp = stats.hpMax;
        stats.currentWave = 0;
        stats.timeSurvived = 0;
        stats.newHighscore = false;
        stats.score = 0;

        // Set hud active and hide menu
        screen.playerHud.SetActive(true);
        screen.mainMenu.SetActive(false);
        screen.gameOver.SetActive(false);
        
        StartLevel(0);

        playing = true;
    }

    public void EndGame(){
        
        playing = false;

        Destroy(currentPlayer);
        DisablePlayerCamera();
        GetDeathScreen();
    }

    public void SpawnPlayer(){
        currentPlayer = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity); // Spawn the player in position
        currentPlayer.name = "Player";
    }

    // Screen to be displayed on death
    public void GetDeathScreen(){
        screen.mainMenu.SetActive(false);
        screen.playerHud.SetActive(false);
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
        screen.mainMenu.SetActive(true);
        screen.playerHud.SetActive(false);
        screen.gameOver.SetActive(false);

        screen.mainMenuScore.text = "Highscore: " + stats.highscore;
    }

    public void ResetPlayerPosition(){
        GameObject spawnTarget = GameObject.Find("PlayerSpawnPoint"); // Search for spawn marker in word
        if (!spawnTarget) {
            Debug.LogError("Error: No spawn point could be found in the scene!",this); 
            return;
        }
        currentPlayer.transform.position = spawnTarget.transform.position;
    }

    public void StartLevel(int levelIndex){

        SpawnPlayer();
        ResetPlayerPosition();
        EnablePlayerCamera();
    }


    public void NextLevel(){

    }


    public void EnablePlayerCamera(){

        if (currentPlayer){
            CameraMovement.enabled = true;
            CameraMovement.objectToFollow = currentPlayer;
        }else{
            Debug.LogError("Error: No player present. CameraMovement can not be enabled at this time.", this);
            DisablePlayerCamera();
        }
    }

    public void DisablePlayerCamera(){

        CameraMovement.enabled = false;
    }
}
