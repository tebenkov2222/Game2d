using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanSettings : MonoBehaviour
{
    public GameObject Debugging;
    // Start is called before the first frame update
    private void Awake()
    {
        Debugging.SetActive(PlayerPrefs.GetInt("Debugging") == 1);
    }
}
