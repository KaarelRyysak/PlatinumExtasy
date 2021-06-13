using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioSystem : MonoBehaviour
{

    public List<AudioManager.AudioSound> soundList = new List<AudioManager.AudioSound>();


    public void PlaySoundWithId(string id){
        FindObjectOfType<AudioManager>().PlaySound(soundList.Find(sound => sound.id == id));
    }

    // Plays a random sound
    public void PlaySoundRandom(){

        if (soundList.Count == 0){
            Debug.LogError("ERROR: soundList.Count==0. Unable to randomly select sound as there are no sounds! Please add sounds and try again.", this);
            return;
        }

        FindObjectOfType<AudioManager>().PlaySound(soundList[Random.Range(0, soundList.Count)]); 
    }
}
