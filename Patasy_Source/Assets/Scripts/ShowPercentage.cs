using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowPercentage : MonoBehaviour {
    [SerializeField] Slider volumeSlider;
    public TextMeshProUGUI volumePercentage;

    void Update() {
        volumePercentage.text = ((Mathf.RoundToInt((float)(volumeSlider.value*100)))*2).ToString()+"%";
    }
}
