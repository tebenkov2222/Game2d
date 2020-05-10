using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopStack : MonoBehaviour
{
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
}
