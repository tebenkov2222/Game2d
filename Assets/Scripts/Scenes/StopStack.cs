using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopStack : MonoBehaviour
{
    public bool isBack = true;
    public GameObject LastStack;
    public void BackMenu()
    {
        LastStack.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void NextMenu(GameObject next)
    {
        next.SetActive(true);
        this.gameObject.SetActive(false);
    }
    void Update()
    {
        if (isBack)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    BackMenu();
                }
            }
        }
    }
}
