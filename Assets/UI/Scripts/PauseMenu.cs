using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject pauseScreen;
    [SerializeField]
    Webcam webcam1;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.P)){
            pause();
        }
    }
    public void pause(){
        Time.timeScale=0;
            webcam1.stop();
            pauseScreen.SetActive(true);
    }
    public void resume(){
        webcam1.resume();
        pauseScreen.SetActive(false);
    }
    public void menu(){
        if(PlayerPrefs.GetString("GerarRelatório")=="sim"){
        FindObjectOfType<Pattern>().GerarRelatório();
        }
        webcam1.stop();
        SceneManager.LoadScene("Menu");
        
    }
}
