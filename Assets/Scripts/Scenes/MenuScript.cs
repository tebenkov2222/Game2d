using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Android;

public class MenuScript : MonoBehaviour
{
    public GameObject StartCanvas, UnderCanvas, KeyCodeCanvas;
    public Text txt;
    public void LoadScene(string str)
    {
        SceneManager.LoadScene(str);
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
        CheckPermision(Permission.ExternalStorageRead);
        CheckPermision(Permission.ExternalStorageWrite);
        CheckPermision(Permission.Camera);
        txt.text =  Application.version;
    }
    void CheckPermision(string permission)
    {
        if (Permission.HasUserAuthorizedPermission(permission))
        {
            // The user authorized use of the microphone.
        }
        else
        {
            // We do not have permission to use the microphone.
            // Ask for permission or proceed without the functionality enabled.
            Permission.RequestUserPermission(permission);
        }
    }
}
