using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    float destroyTime = 5f; 

    public void Start () { 

        Destroy(gameObject, destroyTime); // Destory after seconds
    }
}
