using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeName : MonoBehaviour {
    private string playername;
    public void WriteName(string name) { playername = name; }
    IEnumerator WaitTime1() {
        yield return new WaitForSeconds((float)0.2);
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().GetName(playername);
        Debug.Log("Name changed to: " + playername);
    }
    public void change() {
        StartCoroutine(WaitTime1());
        Time.timeScale = 1f;
    }
}
