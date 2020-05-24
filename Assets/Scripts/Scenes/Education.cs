using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Education : MonoBehaviour
{
    public string name;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Education"))
        {
            this.gameObject.SetActive(false);
        }
    }
    public void Yes()
    {
        PlayerPrefs.SetInt("Education", 1);
        SceneManager.LoadScene(name);
    }
    public void No()
    {
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
