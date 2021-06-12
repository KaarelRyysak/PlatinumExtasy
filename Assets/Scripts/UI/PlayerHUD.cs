using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{

    public TextMeshProUGUI textHP;
    public TextMeshProUGUI textScore;

    PlayerStats PlayerStats;

    void Start(){
        PlayerStats = FindObjectOfType<PlayerStats>();
        if (!PlayerStats) Debug.LogError("Error: PlayerStats could not be located!",this);
    }
    

    void Update(){

        // Update player HUD
        textHP.text  = "HP: " + PlayerStats.hp;
        textScore.text  = "Score: " + (int)(PlayerStats.timeSurvived * 10);

    }
}
