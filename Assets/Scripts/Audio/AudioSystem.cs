using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;

public class AudioSystem : MonoBehaviour
{
    [System.Serializable] public class AudioSystemSound{
        public AudioClip clip; // 
        public string id; // 
        [Range(0f, 1f)] public int volume; // 
        [Range(0f, 1f)] public int pitch;  // 

        [HideInInspector] public GameObject soundObject;
        [HideInInspector] public AudioSource source;

        // List copying
        public static AudioSystemSound DeepCopy(AudioSystemSound old) {
            AudioSystemSound copy = new AudioSystemSound();
            copy.id     = old.id;
            copy.volume = old.volume;
            copy.pitch  = old.pitch;
            copy.clip = old.clip;

            return copy;
        }
    }
    List<AudioSystemSound> soundList = new List<AudioSystemSound>();
    AudioMixerGroup output;

    public void PlaySoundWithId(string id){
        Play(soundList.Find(sound => sound.id == id));
    }

    // Plays a random sound
    public void PlaySoundRandom(){
        Play(soundList[Random.Range(0, soundList.Count)]);
    }



    public void Play(AudioSystemSound soundInstance){

        
        // create copy of the instance
        AudioSystemSound newInstance = AudioSystemSound.DeepCopy(soundInstance);


        PlaySound(newInstance);
    
    }


    private void PlaySound(AudioSystemSound sound){

        GameObject soundObject = new GameObject();
        sound.soundObject = soundObject;                // Assing object reference back to sound class
        soundObject.transform.SetParent(gameObject.transform); // Set new sound object as child
        soundObject.name = sound.id;                    // Sets the object name - Mostly pointless but good for debugging

        // Assign an AudioSource to the sound instance & initialize it with our settings
        sound.source        = soundObject.AddComponent<AudioSource>();
        sound.source.outputAudioMixerGroup = output;    // Set output
        sound.source.clip   = sound.clip;
        sound.source.volume = sound.volume+1;
        sound.source.pitch  = sound.pitch+1;

        sound.source.Play();

        StartCoroutine(DestroySound(sound));
    }


    IEnumerator DestroySound(AudioSystemSound sound){
        
        // Play until SFX is complete
        while (sound.source.isPlaying)
            yield return 0; // wait for next frame

        // Destroy the sound object
        Destroy(sound.soundObject);

        yield break;
    }
}
