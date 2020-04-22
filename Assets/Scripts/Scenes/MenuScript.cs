using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject StartCanvas, UnderCanvas;
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
    private void Start()
    {
        txt.text =  Application.version;
    }
}
