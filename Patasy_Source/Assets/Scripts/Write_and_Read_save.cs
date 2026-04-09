using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class Write_and_Read_save : MonoBehaviour {
    public GameObject player;

    public GameObject save_table;
    public Button save_button;
    public Button delete_button;

    public TextMeshProUGUI if_save_text;

    private string Saves_Count;
    private string datetime;
    private DateTime theTime;
    private string savename;

    public Transform panel;
    private bool[] bool_buttons_selected;

    private bool saving_time;
    private bool deleting_time;

    void Start() {
        saving_time = true;
        deleting_time = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        show();
    }

    public void save() {
        if(saving_time){
            Saves_Count = (SaveSystem.Count_of_Saves()).ToString();
            theTime = DateTime.Now;
            datetime = theTime.ToString("yyyy-MM-dd\\THH-mm-ss\\Z");
            savename = "save" + datetime + "_" + Saves_Count;

            saving_time = false;

            player.GetComponent<PlayerMovement>().SavePlayer(savename);
            SaveSystem.ShowSaves();
            Delete_buttons(panel);
            StartCoroutine(if_save());
            StartCoroutine(wait()); 
        }
    }
    public void show() {
        string saves = SaveSystem.ShowSaves();
        string[] saves_list = saves.Split(char.Parse("\n"));
        string save_names = SaveSystem.Return_save_names();
        string[] names_list = save_names.Split(char.Parse("\n"));
        for(int i=1; i<saves_list.Length; i++){
            CreateButton(saves_list[i], names_list[i], panel,new Vector3(0,0,0), new Vector2(350,30));
        }
        Create_button_booleans();
        Debug.Log("Seznam showed");
    }

    public void CreateButton(string b_name, string button_text, Transform panel ,Vector3 position, Vector2 size) {
        GameObject clicking_rect = new GameObject();
        
        GameObject button = new GameObject();
        button.name = button_text;
        button.transform.parent = panel;
        button.AddComponent<RectTransform>();
        button.AddComponent<Button>();
        button.transform.position = position;
        button.GetComponent<RectTransform>().sizeDelta = size;

        GameObject button_action = GameObject.Find(button_text);
        button.GetComponent<Button>().onClick.AddListener(() => Select(button_action));

        clicking_rect.transform.parent =  button.GetComponent<RectTransform>();

        GameObject textGO = new GameObject();
        textGO.transform.parent = button.GetComponent<Transform>();
        TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI>();
        text.GetComponent<RectTransform>().sizeDelta = size;
        text.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,-2,0);
        text.fontSize = 12;
        text.color = Color.green;
        text.text = b_name;
        text.alignment = TextAlignmentOptions.TopGeoAligned;

        clicking_rect.name = "rect";
        clicking_rect.AddComponent<Image>();
        //clicking_rect.GetComponent<Image>().color = new Color (0, 0, 0, 200);
        clicking_rect.AddComponent<RectTransform>();
        clicking_rect.GetComponent<RectTransform>().sizeDelta = size;

        button.GetComponent<Button>().targetGraphic = clicking_rect.GetComponent<Image>();
        ColorBlock theColor = button.GetComponent<Button>().colors;
        theColor.normalColor = new Color (0, 0, 20, 100);
        theColor.highlightedColor = new Color (0, 0, 0, 200);
        //theColor.selectedColor = new Color (0, 0, 50, 200);
        button.GetComponent<Button>().colors = theColor;
    }

    public void Delete_buttons(Transform transform) {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
         }
         new WaitForSeconds(2);
    }

    public void Delete_selected_button() {
      if(deleting_time){
        string saves = SaveSystem.Return_save_names();
        string[] saves_list = saves.Split(char.Parse("\n"));
        deleting_time = false;
        for(int i=1;i<saves_list.Length;i++){
            if(bool_buttons_selected[i] == true){
                foreach (Transform child in panel) {
                    SaveSystem.Delete_file(saves_list[i]);
                    Debug.Log("Save_deleted: "+saves_list[i]);
                    }
                }
        }
        Delete_buttons(panel);
        StartCoroutine(if_delete());
        StartCoroutine(wait());  
      }  
    }

    public void Select(GameObject button) {
        string saves = SaveSystem.Return_save_names();
        string[] saves_list = saves.Split(char.Parse("\n"));
        foreach(var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[]){
            if(gameObj.name == button.name){
                ColorBlock barva = button.GetComponent<Button>().colors;
                barva.normalColor = new Color (0, 0, 100, 200);
                button.GetComponent<Button>().colors = barva;
                Debug.Log("Button colored " + button.name);
                Color image = GameObject.Find(button.name + "/rect").GetComponent<Image>().color = new Color (0, 0, 0, 100);
            }
            else if(gameObj.name != button.name){
                for(int i=1;i<saves_list.Length;i++){
                    if(gameObj.name == saves_list[i] && button.name != saves_list[i]){
                     ColorBlock barva2 = gameObj.GetComponent<Button>().colors;
                     barva2.normalColor = new Color (0, 0, 20, 100);
                     gameObj.GetComponent<Button>().colors = barva2;
                     Color image = GameObject.Find(saves_list[i] + "/rect").GetComponent<Image>().color = new Color (0, 0, 10, 50);
                     bool_buttons_selected[i] = false;
                    }    
                }   
            } 
        }
        for(int i=1;i<saves_list.Length;i++){
            if(saves_list[i] == button.name){
                bool_buttons_selected[i] = true;
            }
        }
    }

    public bool[] Create_button_booleans(){
        string saves = SaveSystem.Return_save_names();
        string[] saves_list = saves.Split(char.Parse("\n"));
        for(int i=1;i<saves_list.Length;i++){
            bool_buttons_selected = new bool[i+1];

        }
        for(int i=1;i<saves_list.Length;i++){
            bool_buttons_selected[i] = false;
        }
        return bool_buttons_selected;
    }

    IEnumerator wait(){
        saving_time = false;
        deleting_time = false;
        yield return new WaitForSecondsRealtime(2);
        show();
        saving_time = true;
        deleting_time = true;
    }
    IEnumerator if_save(){
        if_save_text.text = "Game saved";
        yield return new WaitForSecondsRealtime(3);
        if_save_text.text = "";
    }
    IEnumerator if_delete(){
        if_save_text.text = "Game deleted";
        yield return new WaitForSecondsRealtime(3);
        if_save_text.text = "";
    }
}
