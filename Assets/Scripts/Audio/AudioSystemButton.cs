using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioSystemButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    public AudioManager.AudioSound onPointerEnter;
    public AudioManager.AudioSound onClick;

    private Button button;
    private AudioManager AudioManager;

    void Awake()
    {
        // Add listener to set up button    
        button = GetComponent<Button>();

        // Add event triggers if sufficent audio has been set
        if (onClick != null) button.onClick.AddListener(OnClick);
    }


    void Start()
    {
        // Link the button to the AudioManager
        AudioManager = FindObjectOfType<AudioManager>();
        if (!AudioManager) Debug.LogError("ERROR: AudioManager could not be found on this object! Please add one!",this);
    }


    //Do this when the UI object is entered. (MOUSE SPECIFIC)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onPointerEnter.clip != null) AudioManager.PlaySound(onPointerEnter);

    }


    //Do this when the UI object is clicked.  (MOUSE & GAMEPAD & KEYBOARD)
    public void OnClick()
    {
        if (onClick.clip != null) AudioManager.PlaySound(onClick);
    }


    //Do this when the selectable UI object is selected.  (MOUSE & GAMEPAD SPECIFIC)
    public void OnSelect(BaseEventData eventData) 
    {
        if (onPointerEnter.clip != null) AudioManager.PlaySound(onPointerEnter);
    }
}