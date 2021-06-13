using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{

    public TextMeshProUGUI textHP;
    public TextMeshProUGUI textScore;
    public Transform webChargeContainer;
    public GameObject webChargePrefab;
    public TextMeshProUGUI serverText;

    private int webLastAmount = 0;

    GameManager GameManager;
    WebShooter WebShooter;
    GameObject player;

    void Start(){
        GameManager = FindObjectOfType<GameManager>();
        if (!GameManager) Debug.LogError("Error: GameManager could not be located!",this);


    }
    

    void Update(){

        if (player){


            // Update player HUD
            textHP.text  = "HP: " + GameManager.stats.hp;
            textScore.text  = "Score: " + GameManager.stats.score;

            // Update Web
            if (webLastAmount != WebShooter.remainingBursts){

                // Destroy all children
                foreach (Transform child in webChargeContainer)
                    GameObject.Destroy(child.gameObject);

                // Create web elements
                for(int i = 0; i < WebShooter.remainingBursts; i++) {

                    GameObject newCharge = Instantiate(webChargePrefab, new Vector3(0f,0f,0f), Quaternion.identity);
                    newCharge.transform.SetParent(webChargeContainer);
                    newCharge.transform.localScale = new Vector3(1f,1f,1f);
                    newCharge.transform.localPosition = new Vector3(0f,0f,0f);

                }         

                webLastAmount = WebShooter.remainingBursts;
            }

        
        }else{
            // Get player object
            player= GameObject.Find("Player");

            // If player was found - Get the web shooter
            if (player) WebShooter = player.GetComponent<WebShooter>();
        }
    }

    public void SetServerText(string text)
    {
        serverText.text = text;

    }

    public void ClearServerText()
    {
        serverText.text = "";
    }
}
