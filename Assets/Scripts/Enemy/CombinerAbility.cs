using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinerAbility : MonoBehaviour
{
    EnemyManager enemyManager;
    Dictionary<GameObject, float> connections;
    [SerializeField] GameObject combinedEnemy;
    GameObject linkedEnemy;
    float connectionCheckFrequency = 1;
    float combineTimeThreshold = 4;

    void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
    }

    private void Start()
    {
        connections = new Dictionary<GameObject, float>();
        InvokeRepeating("EvaluateAttachedEnemies", connectionCheckFrequency, connectionCheckFrequency);
    }

    void Update()
    {
        //if (Input.GetButtonDown("Jump"))
        //{
        //    CombineWithEnemy(GameObject.FindGameObjectWithTag("Enemy"));
        //}
    }

    void EvaluateClosestEnemy() //Doesn't work
    {
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> enemiesList = new List<GameObject>(enemies);
        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemiesList[i] == gameObject)
            {
                enemiesList.RemoveAt(i);
            }
        }
        float smallestDistance = int.MaxValue;
        GameObject nearestEnemy;
        for (int i = 0; i < enemiesList.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, enemiesList[i].transform.position);
            if (distance < smallestDistance)
            {

            }
        }
    }

    void EvaluateAttachedEnemies()
    {
        List<GameObject> attachedEnemies = enemyManager.GetAllAttachedEnemies();
        for (int i = 0; i < attachedEnemies.Count; i++)
        {
            if (connections.ContainsKey(attachedEnemies[i]))
            {
                connections[attachedEnemies[i]] += connectionCheckFrequency;
                print(connections[attachedEnemies[i]]);
            }
            else
            {
                connections.Add(attachedEnemies[i], 0);
            }

            if (connections[attachedEnemies[i]] >= combineTimeThreshold)
            {
                CombineWithEnemy(attachedEnemies[i]);
                attachedEnemies[i].GetComponent<CombinerAbility>().CancelInvoke();
                CancelInvoke("EvaluateAttachedEnemies");
                break;
            }
        }
    }

    void CombineWithEnemy(GameObject otherEnemy)
    {

        Vector3 otherEnemyPosition = otherEnemy.transform.position;

        Vector3 midpoint = new Vector3();
        midpoint.x = (otherEnemyPosition.x + transform.position.x) / 2;
        midpoint.y = transform.position.y;
        midpoint.z = (otherEnemyPosition.z + transform.position.z) / 2;

        Instantiate(combinedEnemy, midpoint, Quaternion.identity);
        GameObject.Destroy(otherEnemy);
        GameObject.Destroy(gameObject);
    }
}
