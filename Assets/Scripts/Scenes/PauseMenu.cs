using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject cnv, Pause;
    public void PressPause() 
    {
        cnv.gameObject.SetActive(false);
        Pause.SetActive(true);
        //Time.timeScale = 0f;
    } 
    public void Resume()
    {
        cnv.gameObject.SetActive(true);
        Pause.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
