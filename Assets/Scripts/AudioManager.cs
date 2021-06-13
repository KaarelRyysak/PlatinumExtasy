using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;

public partial class AudioManager : MonoBehaviour
{

    [System.Serializable] public class AudioSound{
        public AudioClip clip; // 
        public string id; // 
        [Range(0f, 1f)] public int volume; // 
        [Range(0f, 1f)] public int pitch;  // 


        public AudioMixerGroup output;
        public GameObject soundObject;

        [HideInInspector] public AudioSource source;

        // List copying
        public static AudioSound DeepCopy(AudioSound old) {
            AudioSound copy = new AudioSound();
            copy.id     = old.id;
            copy.volume = old.volume;
            copy.pitch  = old.pitch;
            copy.clip = old.clip;
            copy.output = old.output;
            copy.soundObject = old.soundObject;

            return copy;
        }
    }

    public void PlaySound(AudioSound soundInstance){

        if (!soundInstance.output){
            Debug.LogWarning("WARNING: Audio has no mixer output set! It will may not play as intended.", soundInstance.soundObject);
        }

        // If no object was given, then target the audio system as the source
        if (soundInstance.soundObject == null)
            soundInstance.soundObject = gameObject;
        
        
        // create copy of the instance
        AudioSound newInstance = AudioSound.DeepCopy(soundInstance);


        PlaySoundStart(newInstance);
    
    }


    private void PlaySoundStart(AudioSound sound){

        GameObject soundObject = new GameObject();
        soundObject.transform.SetParent(sound.soundObject.transform);   // Set new sound object as child
        soundObject.name = sound.id;        // Sets the object name - Mostly pointless but good for debugging
        sound.soundObject = soundObject;    // Assing object reference back to sound class

        // Assign an AudioSource to the sound instance & initialize it with our settings
        sound.source        = soundObject.AddComponent<AudioSource>();
        if (sound.output)
            sound.source.outputAudioMixerGroup = sound.output;              // Set output
        sound.source.clip   = sound.clip;
        sound.source.volume = sound.volume+1;
        sound.source.pitch  = sound.pitch+1;

        sound.source.Play();

        StartCoroutine(DestroySound(sound));
    }


    IEnumerator DestroySound(AudioSound sound){
        
        // Play until SFX is complete
        while (sound.source.isPlaying)
            yield return 0; // wait for next frame

        // Destroy the sound object
        Destroy(sound.soundObject);

        yield break;
    }
}