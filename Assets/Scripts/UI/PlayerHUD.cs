using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{

    public TextMeshProUGUI textHP;
    public TextMeshProUGUI textScore;

    GameManager GameManager;

    void Start(){
        GameManager = FindObjectOfType<GameManager>();
        if (!GameManager) Debug.LogError("Error: GameManager could not be located!",this);
    }
    

    void Update(){

        // Update player HUD
        textHP.text  = "HP: " + GameManager.stats.hp;
        textScore.text  = "Score: " + (int)(GameManager.stats.timeSurvived * 10);

    }
}
