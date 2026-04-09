using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadSave : MonoBehaviour {
    private string loadname;
    
    public void WriteLoad(string name) {
        loadname = name;
    }

    public void Load_game() {
        StartCoroutine(wait());
        Time.timeScale = 1f;
    }

    IEnumerator wait() {
        yield return new WaitForSeconds((float)0.4);
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().LoadPlayer(loadname);
        Debug.Log("Game loaded: " + loadname);
        Destroy(this);
    }
}
