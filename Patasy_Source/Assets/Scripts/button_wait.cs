using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class button_wait : MonoBehaviour {
    // Update is called once per frame
    public void Button_waiting(Button button) {
        StartCoroutine(wait(button));
    }
    IEnumerator wait(Button button) {
        button.enabled = false;
        yield return new WaitForSeconds((float)2);
        button.enabled = true;
    }
    public void Update_possible_hp(TextMeshProUGUI hp_text) {
        hp_text.text = (GameObject.Find("Player").GetComponent<PlayerMovement>().return_health()).ToString();
    }
    public void Update_possible_damage(TextMeshProUGUI damage_text) {
        damage_text.text = GameObject.Find("Player").GetComponent<PlayerMovement>().return_increased_damage();
    }
}
