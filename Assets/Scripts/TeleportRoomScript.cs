using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TeleportRoomScript : MonoBehaviour
{
    public List<GameObject> Mobs;
    public Animator Animator;
    public GameObject Player, PlayerEmpty, TeleportSettings;
    public TeleportRoomSettingsScript TeleportRoomSettingsScript;
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

    private void Start()
    {
        TeleportRoomSettingsScript = TeleportRoomSettingsScript.Instance;
    }

    private void Update()
    {
        CheckAllMobs();
    }
    public void Teleported()
    {
        if (CheckAllMobs())
        {
            PlayerController.Instance._ActivePlayer = false;
            PlayerController.Instance.RB.velocity = Vector2.zero;
            PlayerController.Instance.RB.bodyType = RigidbodyType2D.Kinematic;
            Player = PlayerController.Instance.gameObject;
            if (Levels)
            {
                DontDestroyOnLoad(Player);
                Player.transform.position = PlayerEmpty.transform.position;
                Animator.Play("TeleportLevel", 0);

            }
            else
            {
                Player.transform.position = PlayerEmpty.transform.position;
                Animator.Play("TeleportRoom", 0);
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
        else TeleportRoomSettingsScript.NextRoom(); 
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
