using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundtrackManager : MonoBehaviour {
    [SerializeField] Slider volumeSlider;
    public AudioSource soundtrack;

    void Start() {
        Load();
    }
    public void ChangeVolume(AudioSource AudioListener) {
        AudioListener.volume = volumeSlider.value;
        Save();
    }
    private void Load() {
        volumeSlider.value = soundtrack.volume;
    }
    private void Save() {
        soundtrack.volume = volumeSlider.value;
    }
}
