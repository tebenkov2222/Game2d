using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject StartCanvas, UnderCanvas, KeyCodeCanvas;
    public Text txt;
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
    public void KeyCodeActive()
    {
        KeyCodeCanvas.SetActive(true);
        StartCanvas.SetActive(false);
    }
    private void Start()
    {
        txt.text =  Application.version;
    }
}
