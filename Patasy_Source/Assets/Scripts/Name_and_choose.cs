using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Name_and_choose : MonoBehaviour {   
    private bool name_right;
    public TextMeshProUGUI error;
    public GameObject InputField;
    public GameObject set_name;
    public GameObject music;
    public TextMeshProUGUI name_field;
    string tojmeno;
  
    void Start() {
        var name_input = InputField.GetComponent<TMP_InputField>();
        var name = new TMP_InputField.SubmitEvent();
        //name.AddListener(SubmitName);
        //name_input.onEndEdit = name;
        name_input.onEndEdit.AddListener(SubmitName);
        
    }
    
    private void SubmitName(string jmeno) {
         if (jmeno.Length < 3){
            name_right = false;
            error.text = "Too short name, min 3 characters."; 
         }
         else{
            name_right = true;   
            error.text = "";
         } 
     }

    public void Start_game() {
        if(name_right==true){
            Debug.Log("start");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            tojmeno = name_field.text.ToString();
            set_name.GetComponent<ChangeName>().WriteName(tojmeno);
            set_name.GetComponent<ChangeName>().change();
            Destroy(music);
        }
        else{
            Debug.Log(error.text);
        }
    }
    
    void Awake() {
        DontDestroyOnLoad(this.set_name);
    }
}




