using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Can you advise me how can I in my Unity Patasy game create single menu management script which creates text in label version of game, manage menu music, settings of soundtrack management.
public class MainMenu : MonoBehaviour {
    public void PlayGame () {
        Debug.Log("start");
        /*sfx_manager.sfx_instance.Audio.PlayOneShot(sfx_manager.sfx_instance.Click);*/
        /*yield return new WaitForSeconds(0.5f);*/
        wait();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);   
    }
    public void QuitGame () {
        Debug.Log("exit");
        Application.Quit();
    }
    IEnumerator wait() {
        yield return new WaitForSeconds((float)2);
            
    }    

}
