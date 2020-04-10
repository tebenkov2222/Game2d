using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject StartCanvas, UnderCanvas;
    public void LoadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Under()
    {
        StartCanvas.SetActive(true);
        Destroy(UnderCanvas);
    }
}
