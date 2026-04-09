using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sroll_movement : MonoBehaviour {
    public Scrollbar slider;
    public ScrollRect scroll;
    public float min_value;
    public float value;

    public void AdjustValue() {
        if(slider.value > value){
            slider.value = value;
            scroll.StopMovement();
        }
    }
    public void AdjutMinValue() {
        if(slider.value < min_value){
            slider.value = min_value;
            scroll.StopMovement();
        }
    }  
}
