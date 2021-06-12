using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int hp = 5;              // Remaining HP
    public int enemiesKilled = 0;   // Number of enemies Defeated
    public float timeSurvived = 0;  // How long the player has survived for

    void Update(){

        timeSurvived += Time.deltaTime;
    
    }
}
