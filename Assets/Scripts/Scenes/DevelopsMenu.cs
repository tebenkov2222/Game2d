using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(PlayerPrefs.GetInt("Debugging") == 1);
    }

    public void DeletAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Debugging", 1);
    }
    public void DeleteUpgrades()
    {
        PlayerPrefs.DeleteKey("WomanUpgrade");
        PlayerPrefs.DeleteKey("MagUpgrade");
        PlayerPrefs.DeleteKey("Coin");
    }
    public void DeleteDriverSettings()
    {
        PlayerPrefs.DeleteKey("Visible");
        PlayerPrefs.DeleteKey("Joystick");
        PlayerPrefs.DeleteKey("Sensetive");
        PlayerPrefs.DeleteKey("Saved");
        PlayerPrefs.DeleteKey("WomanUpgrade");
    }
}
