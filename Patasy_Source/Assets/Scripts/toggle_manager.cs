using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggle_manager : MonoBehaviour {
    public Toggle sound;
    public GameObject walk1;
    public GameObject walk2;
    // Update is called once per frame
    void Update() {
        if(sound.isOn){
            walk1.SetActive(true);
            walk1.SetActive(true);
        }
        else{
            walk1.SetActive(false);
            walk1.SetActive(false);
        }
    }
}
