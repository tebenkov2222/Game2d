using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject cnv, Pause;
    bool pause = false;
    public void PressPause() 
    {
        cnv.gameObject.SetActive(false);
        Pause.SetActive(true);
        pause = true;
    } 
    public void Resume()
    {
        pause = false;
        cnv.gameObject.SetActive(true);
        Pause.SetActive(false);
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
    private void Update()
    {
        if (pause || PlayerPrefs.GetInt("Death") == -1)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.0000001f, 0.1f);
        }
        else
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, 0.1f);
        }
    }
}
