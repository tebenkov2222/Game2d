using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyCodeScripts : MonoBehaviour
{
    public Text txt;
    public GameObject CanvasStart;
    public void CheckBtn()
    {
        if (txt.text == "Debug.Log")
        {
            PlayerPrefs.SetInt("Debugging", 1);
        }
        else if (txt.text == "Debug.Log.False") PlayerPrefs.SetInt("Debugging", 0);
        txt.text = "";
        CanvasStart.SetActive(true);
        this.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Debugging"))PlayerPrefs.SetInt("Debugging", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
