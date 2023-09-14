using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAudio : MonoBehaviour //Class to play music, and make sure it does not get removed between scene changes as well as keeping only one instance of it.
    
{
    private static KeepAudio instance = null; //Setting the variable of the stored music

    public static KeepAudio Instance //Identifying if the music is playing
    {
        get { return instance; }
    }
    private void Awake() //Awake method plays once every time the scene changes
    {

        if (instance != null && instance != this) //If music is already playing, delete the new music soundtrack that is about to play
        {
            Destroy(this.gameObject);
            return;
        }
        else { instance = this;
        }
        DontDestroyOnLoad(this.gameObject); //Maintains the music throughout scene switching

    }
}
