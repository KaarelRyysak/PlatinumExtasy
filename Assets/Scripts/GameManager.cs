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
        public GameObject youDied;
        public TextMeshProUGUI youDiedHighscore;
        public TextMeshProUGUI youDiedYourScore;
        public TextMeshProUGUI youDiedLeaderboard;
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

    public GameObject playerPrefab;     // Prefab of the player
    private GameObject currentPlayer;   
    private GameObject currentLevel;

    public CameraMovement CameraMovement;

    public bool playing = false;

    void Awake(){

        // Disable camera movement on startup
        DisablePlayerCamera();

        GetMainMenu();
    }


    void Update(){
        if (playing){
            stats.timeSurvived += Time.deltaTime;
            if (stats.hp<=0) EndGame();
        }
    }

    public void StartGame(){

        // Initialize data
        stats.hp = stats.hpMax;
        stats.currentWave = 0;
        stats.timeSurvived = 0;
        stats.newHighscore = false;

        // Set hud active and hide menu
        screen.playerHud.SetActive(true);
        screen.mainMenu.SetActive(false);
        screen.youDied.SetActive(false);
        
        StartLevel(0);

        playing = true;
    }

    public void EndGame(){
        
        playing = false;

        Destroy(currentPlayer);
        DisablePlayerCamera();
        GetDeathScreen();

        if (stats.score > stats.highscore){
            stats.newHighscore = true;
        }

    }

    public void SpawnPlayer(){
        currentPlayer = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity); // Spawn the player in position
        currentPlayer.name = "Player";
    }

    // Screen to be displayed on death
    public void GetDeathScreen(){
        screen.mainMenu.SetActive(false);
        screen.playerHud.SetActive(false);
        screen.youDied.SetActive(true);
    }

    // Screen to be displayed on startup
    public void GetMainMenu(){
        screen.mainMenu.SetActive(true);
        screen.playerHud.SetActive(false);
        screen.youDied.SetActive(false);
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
