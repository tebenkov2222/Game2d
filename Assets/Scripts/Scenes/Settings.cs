using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Settings : MonoBehaviour
{
    public GameObject[] objects;
    public Vector2[] offsets;
    public GameObject 
        Sensetive,
        dropBox,
        Togle;
    public void Visible()
    {
        bool value = Togle.GetComponent<Toggle>().isOn;
        if (value) PlayerPrefs.SetInt("Visible", 1);
        else PlayerPrefs.SetInt("Visible", 0);
    }
    public void SensetiveSet()
    {
        float value = Sensetive.GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("Sensetive", value);
    }
    public void SetStandartValueSesAndDropBox()
    {
        if (PlayerPrefs.HasKey("Joystick")) dropBox.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("Joystick");
        if (PlayerPrefs.HasKey("Sensetive")) Sensetive.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Sensetive");
        if (PlayerPrefs.HasKey("Visible"))
        {
            Togle.GetComponent<Toggle>().isOn = (PlayerPrefs.GetInt("Visible") == 1);
        }
    }
    public void ChangeJoystik()
    {
        int value = dropBox.GetComponent<Dropdown>().value;
        PlayerPrefs.SetInt("Joystick", value);
    }
    public void Save()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            SaveObj(objects[i].name, objects[i].GetComponent<RectTransform>().anchoredPosition, objects[i].transform.localScale / offsets[i]);
        }
    }
    public void Load(GameObject[] objs)
    {
        if (PlayerPrefs.HasKey("Saved"))
        {
            for (int i = 0; i < objs.Length; i++)
            {
                //Debug.Log(objs[i].name + " " + PosObj(objs[i].name) + " " + SclObj(objs[i].name));
                objs[i].GetComponent<RectTransform>().anchoredPosition = PosObj(objs[i].name);
                objs[i].GetComponent<RectTransform>().localScale = SclObj(objs[i].name);
            }
        }
    }
    public void LoadOnSettings()
    {
        if (PlayerPrefs.HasKey("Saved"))
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].GetComponent<RectTransform>().anchoredPosition = PosObj(objects[i].name);
                objects[i].transform.localScale = SclObj(objects[i].name) * offsets[i];
            }
        }
    }
    public Vector3 SclObj(string name)
    {
        if (PlayerPrefs.HasKey("Saved")) return new Vector3(PlayerPrefs.GetFloat(name + ".S.x"), PlayerPrefs.GetFloat(name + ".S.y"), 1);
        else return Vector2.zero;
    }
    public Vector2 PosObj(string name)
    {
        if (PlayerPrefs.HasKey("Saved"))  return new Vector2(PlayerPrefs.GetFloat(name + ".P.x"), PlayerPrefs.GetFloat(name + ".P.y"));
        else return Vector2.zero;
    }
    void SaveObj(string name, Vector2 pos, Vector2 scl)
    {
        PlayerPrefs.SetInt("Saved", 1);
        PlayerPrefs.SetFloat(name + ".P.x", pos.x);
        PlayerPrefs.SetFloat(name + ".P.y", pos.y);

        PlayerPrefs.SetFloat(name + ".S.x", scl.x);
        PlayerPrefs.SetFloat(name + ".S.y", scl.y);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
