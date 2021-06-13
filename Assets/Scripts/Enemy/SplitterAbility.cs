using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterAbility : MonoBehaviour
{
    [SerializeField] int splitsLeft = 3;
    float splitDelay = 5;
    float splitCountdown;
    //bool hasSplit = false;
    GameObject SplitObject;

    void Start()
    {
        SplitObject = gameObject;
        splitCountdown = splitDelay;
        ScaleByRemainingSplits();
    }

    void Update()
    {
        if (splitsLeft > 0)
        {
            splitCountdown -= Time.deltaTime;
            if (splitCountdown <= 0f)
            {
                Split();
                splitCountdown = splitDelay;
                //hasSplit = true;
            }
        }
        
    }

    void Setup(int _splitsLeft)
    {
        splitsLeft = _splitsLeft;
        SplitObject = gameObject;
        ScaleByRemainingSplits();
    }

    void ScaleByRemainingSplits()
    {
        transform.localScale = Vector3.one * (1f + 0.5f * splitsLeft);
    }

    void Split()
    {
        splitsLeft -= 1;
        ScaleByRemainingSplits();
        Vector3 splitOffset = new Vector3(Random.Range(-.5f, .5f), 0, Random.Range(-.5f, .5f));
        GameObject other = Instantiate(SplitObject, transform.position + splitOffset, Quaternion.identity, transform.parent);
        other.GetComponent<SplitterAbility>().Setup(splitsLeft);

    }
}
