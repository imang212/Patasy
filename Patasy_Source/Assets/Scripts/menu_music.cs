using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu_music : MonoBehaviour {
    public static menu_music menu_instance;
    public AudioSource Audio;
    private void Awake() {
        if(menu_instance != null && menu_instance != this) {
            Destroy(this.gameObject);
            return;
        }
        menu_instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start() {
        Audio = GetComponent<AudioSource>();
    }

}
