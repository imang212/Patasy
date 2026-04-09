using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfx_manager : MonoBehaviour {
    public AudioSource Audio;
    public AudioClip Click;
    public static sfx_manager sfx_instance;
    
    public void Awake() {
        if(sfx_instance != null && sfx_instance != this) {
            Destroy(this.gameObject);
            return;
        }
        sfx_instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start() {
        Audio = GetComponent<AudioSource>();
        Audio.volume = (float)0.1;
    }

}
