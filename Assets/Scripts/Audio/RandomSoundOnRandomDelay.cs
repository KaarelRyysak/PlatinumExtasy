using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundOnRandomDelay : MonoBehaviour
{

    [Range(0.1f, 10f)] float minDelay = 5;
    [Range(0.1f, 10f)] float maxDelay = 100;
    AudioSystem AudioSystem;

    void Start(){
        AudioSystem = FindObjectOfType<AudioSystem>();
        if (!AudioSystem) Debug.LogError("Error: AudioSystem could not be located!",this);
    }

    public IEnumerator RandomDelayLoop(int waveIndex){

        while(true){

            float delay = Random.Range(maxDelay, maxDelay);

            yield return new WaitForSeconds(delay);

            // AudioSystem

        }
    }
}
