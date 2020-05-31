using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Android;
using System.Collections;
public class MenuScript : MonoBehaviour
{
    public GameObject StartCanvas, UnderCanvas, KeyCodeCanvas, MenuSettings;
    public Text txt;
    public void MenuSettigsCheck()
    {
        MenuSettings.SetActive(!MenuSettings.activeSelf);
    }
    public void LoadScene(string str)
    {
        StartCoroutine(TeleportCoroutine(str));
    }
    private  IEnumerator TeleportCoroutine(string str)
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(str);
        StopAllCoroutines();
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
        if (GameObject.Find("Player"))
        {
            Destroy(GameObject.Find("Player"));
        }
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
