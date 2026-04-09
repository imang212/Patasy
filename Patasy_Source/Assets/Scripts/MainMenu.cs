using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
