using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TeleportRoomScript : MonoBehaviour
{
    public List<GameObject> Mobs;
    public GameObject Player, PlayerEmpty, TeleportSettings;
    public string LevelName;
    public bool Levels = false;
    private int countMobs = 0;

    private void Awake()
    {
        GameObject[] vr = GameObject.FindGameObjectsWithTag("Sceleton");
        for (int i = 0; i < vr.Length; i++)
        {
            if (vr[i].gameObject.GetComponent<MagScript>() ||
            vr[i].gameObject.GetComponent<SceletonAPI>()) Mobs.Add(vr[i]);
        }
        vr = GameObject.FindGameObjectsWithTag("Strazh");
        for (int i = 0; i < vr.Length; i++)
        {
            if (vr[i].gameObject.GetComponent<MagScript>() ||
            vr[i].gameObject.GetComponent<SceletonAPI>()) Mobs.Add(vr[i]);
        }
        countMobs = Mobs.Count;
    }
    private void Update()
    {
        CheckAllMobs();
    }
    public void Teleported()
    {
        if (CheckAllMobs())
        {
            Player = GameObject.Find("Player");
            TeleportSettings = GameObject.Find("TeleportRoomSettings");
            if (Levels)
            {
                if (LevelName == "Menu")
                {
                    DestroyObject(Player);
                }
                else
                {
                    DontDestroyOnLoad(Player);
                }
                Player.transform.position = PlayerEmpty.transform.position;
                Player.GetComponent<PlayerController>()._ActivePlayer = false;
                Player.GetComponent<Rigidbody2D>().isKinematic = true;
                this.GetComponent<Animator>().Play("TeleportLevel", 0);

            }
            else
            {
                Player.transform.position = PlayerEmpty.transform.position;
                Player.GetComponent<PlayerController>()._ActivePlayer = false;
                Player.GetComponent<Rigidbody2D>().isKinematic = true;
                this.GetComponent<Animator>().Play("TeleportRoom", 0);
            }
        }
    }
    public void DisablePlayer()
    {
        Player.GetComponent<SpriteRenderer>().enabled = false;
    }
    public void TeleportEnd()
    {
        if (Levels) SceneManager.LoadScene(LevelName);
        else TeleportSettings.GetComponent<TeleportRoomSettingsScript>().NextRoom(); 
    }
    private bool CheckAllMobs()
    {
        Mobs.Clear();
        GameObject[] vr = GameObject.FindGameObjectsWithTag("Sceleton");
        for (int i = 0; i < vr.Length; i++)
        {
            if (vr[i].gameObject.GetComponent<MagScript>() ||
            vr[i].gameObject.GetComponent<SceletonAPI>()) Mobs.Add(vr[i]);
        }
        vr = GameObject.FindGameObjectsWithTag("Strazh");
        for (int i = 0; i < vr.Length; i++)
        {
            if (vr[i].gameObject.GetComponent<MagScript>() ||
            vr[i].gameObject.GetComponent<SceletonAPI>()) Mobs.Add(vr[i]);
        }
        countMobs = Mobs.Count;
        if (countMobs == 0) return true;
        return false;
    }
}
