using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {
    public GameObject wolf, bat, jupiter;
    public PlayerMovement player_stats;
    public int positionx = 10;
    private int random_x_pos, random_y_pos;
    private string[] enemy_names;
    private Vector3[] enemy_positions;
    private List<string> actual_names = new List<string>();
    private List<Vector3> actual_positions = new List<Vector3>();
  
    void Start() {
        wolf.transform.localScale = new Vector3((float)1.5*40,(float)1.5*40,(float)1);
        bat.transform.localScale = new Vector3((float)3.010724*40,(float)3.010724*40,(float)1);
        jupiter.transform.localScale = new Vector3((float)1.452373*40,(float)1.452373*40,(float)1);
        enemy_names = new string[positionx+1];
        enemy_positions = new Vector3[positionx+1];
        player_stats = GameObject.Find("Player").GetComponent<PlayerMovement>();

        if(player_stats.GetScore() == 0){
            for (int i=0; i<positionx; i++) {
                random_x_pos = Random.Range(-300,2303);
                random_y_pos = Random.Range(0, 634*2);
                int x = Random.Range(0, 2);
                switch (x) {
                    case 0:
                        GameObject wolf_create = Instantiate(wolf, new Vector3(random_x_pos,random_y_pos,1), Quaternion.identity);
                        wolf_create.name = "wolf"+i.ToString();
                        enemy_names[i] = "wolf"+i.ToString(); 
                        enemy_positions[i] = GameObject.Find("wolf"+i.ToString()).transform.position;
                        break;
                    case 1:
                        GameObject bat_create = Instantiate(bat, new Vector3(random_x_pos,random_y_pos,1), Quaternion.identity);
                        bat_create.name = "bat"+i.ToString();
                        enemy_names[i] = "bat"+i.ToString(); 
                        enemy_positions[i] = GameObject.Find("bat"+i.ToString()).transform.position;
                        break;
                }
                if(i == 29){
                    GameObject jup_create = Instantiate(jupiter, new Vector3(random_x_pos,random_y_pos,1), Quaternion.identity);
                    jup_create.name = "jupiter"+i.ToString();
                    enemy_names[i+1] = "jupiter"+i.ToString(); 
                    enemy_positions[i+1] = GameObject.Find("jupiter"+i.ToString()).transform.position;
                }
            }
        }
        wolf.transform.localScale = new Vector3((float)1.5,(float)1.5,(float)1);
        bat.transform.localScale = new Vector3((float)3.010724,(float)3.010724,(float)1);
        jupiter.transform.localScale = new Vector3((float)1.452373,(float)1.452373,(float)1);
    }     

    public void Detect_actual_enemy_names_and_positions() {
        for(int i=0;i<enemy_names.Length;i++){
            if(GameObject.Find(enemy_names[i]) != null){
                actual_names.Add(enemy_names[i]);
                actual_positions.Add(GameObject.Find(enemy_names[i]).transform.position);
            }
            else{
                Debug.Log("enemy not finded: "+enemy_names[i]);
            }
        }  
    }

    public string[] Return_actual_enemy_names() {
        string[] enemy_list_names = new string[actual_names.Count];
        for(int i=0;i<actual_names.Count;i++){
            enemy_list_names[i] = actual_names[i];
        }
        actual_names.Clear(); 
        return enemy_list_names;
    }

    public string[] Return_actual_enemy_positions() {
        string[] enemy_string_positions = new string[actual_positions.Count];
        for(int i=0;i<actual_positions.Count;i++){
            string position = actual_positions[i].ToString();
            enemy_string_positions[i] = position;
        }
        actual_positions.Clear(); 
        return enemy_string_positions;
    }

    public void Generate_new_enemies_by_count(string[] enemies, Vector3[] positions) {
        enemy_names = enemies;
        enemy_positions = positions;
        wolf.transform.localScale = new Vector3((float)1.5*40,(float)1.5*40,(float)1);
        bat.transform.localScale = new Vector3((float)3.010724*40,(float)3.010724*40,(float)1);
        jupiter.transform.localScale = new Vector3((float)1.452373*40,(float)1.452373*40,(float)1);
        for(int i=0;i<enemies.Length;i++){
            if(enemies[i].Contains("wolf")){
                GameObject wolf_create = Instantiate(wolf, positions[i], Quaternion.identity);
                wolf_create.name = "wolf"+i.ToString();
                enemy_names[i] = "wolf"+i.ToString(); 
                enemy_positions[i] = GameObject.Find("wolf"+i.ToString()).transform.position;
            }
            if(enemies[i].Contains("bat")){
                GameObject bat_create = Instantiate(bat, positions[i], Quaternion.identity);
                bat_create.name = "bat"+i.ToString();
                enemy_names[i] = "bat"+i.ToString(); 
                enemy_positions[i] = GameObject.Find("bat"+i.ToString()).transform.position;
            }
            if(enemies[i].Contains("jupiter")){
                GameObject jup_create = Instantiate(jupiter, positions[i], Quaternion.identity);
                jup_create.name = "jupiter"+i.ToString();
                enemy_names[i] = "jupiter"+i.ToString(); 
                enemy_positions[i] = GameObject.Find("jupiter"+i.ToString()).transform.position;
            }
        }
        Debug.Log("Now generated " + enemy_names.Length + " enemies");
        wolf.transform.localScale = new Vector3((float)1.5,(float)1.5,(float)1);
        bat.transform.localScale = new Vector3((float)3.010724,(float)3.010724,(float)1);
        jupiter.transform.localScale = new Vector3((float)1.452373,(float)1.452373,(float)1);
    }

    public void Generate_new_enemies() {
        wolf.transform.localScale = new Vector3((float)1.5*40,(float)1.5*40,(float)1);
        bat.transform.localScale = new Vector3((float)3.010724*40,(float)3.010724*40,(float)1);
        jupiter.transform.localScale = new Vector3((float)1.452373*40,(float)1.452373*40,(float)1);
        if(Return_actual_enemy_names().Length == 0){
            for (int i=0; i<positionx; i++){
                random_x_pos = Random.Range(-300,2303);
                random_y_pos = Random.Range(0, 634*2);
                int x = Random.Range(0, 2);
                switch (x) {
                    case 0:
                        GameObject wolf_create = Instantiate(wolf, new Vector3(random_x_pos,random_y_pos,1), Quaternion.identity);
                        wolf_create.name = "wolf"+i.ToString();
                        enemy_names[i] = "wolf"+i.ToString(); 
                        enemy_positions[i] = GameObject.Find("wolf"+i.ToString()).transform.position;
                        break;
                    case 1:
                        GameObject bat_create = Instantiate(bat, new Vector3(random_x_pos,random_y_pos,1), Quaternion.identity);
                        bat_create.name = "bat"+i.ToString();
                        enemy_names[i] = "bat"+i.ToString(); 
                        enemy_positions[i] = GameObject.Find("bat"+i.ToString()).transform.position;
                        break;
                }
                if(i == 29){
                    GameObject jup_create = Instantiate(jupiter, new Vector3(random_x_pos,random_y_pos,1), Quaternion.identity);
                    jup_create.name = "jupiter"+i.ToString();
                    enemy_names[i+1] = "jupiter"+i.ToString(); 
                    enemy_positions[i+1] = GameObject.Find("jupiter"+i.ToString()).transform.position;
                }
            }
        }
        wolf.transform.localScale = new Vector3((float)1.5,(float)1.5,(float)1);
        bat.transform.localScale = new Vector3((float)3.010724,(float)3.010724,(float)1);
        jupiter.transform.localScale = new Vector3((float)1.452373,(float)1.452373,(float)1);
    }

    public void Delete_all_enemies() {
        for(int i=0;i<enemy_names.Length;i++){
            if(GameObject.Find(enemy_names[i]) != null){
                Destroy(GameObject.Find(enemy_names[i]));
            }
            else{
                Debug.Log("enemy not finded: "+enemy_names[i]);
            }
        }
    }

    public int Return_enemy_count() {
        int count = 0;
        for(int i=0;i<enemy_names.Length;i++){
            if(GameObject.Find(enemy_names[i]) != null){
                count++;
            }
        }
        return count-1;
    }
}



