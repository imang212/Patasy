using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject shop_table;
    private bool canDoAction = true;

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) && canDoAction == true){
            if(GameIsPaused){ 
                Resume();
            }
            else{
                Pause();
            }
        }
    }
    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause() {
        shop_table.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void GetToMenu() {
        Debug.Log("menu");
        Destroy(GameObject.Find("LoadSave"));
        Destroy(GameObject.Find("PlayerName"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }
    public void QuitGame () {
        Debug.Log("exit");
        Application.Quit();
    }
    public void EscDisable() {
        canDoAction = false;
    }
    public void EscEnable() {
        canDoAction = true;
    }
}
