using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRoomSettingsScript : MonoBehaviour
{
    public static TeleportRoomSettingsScript Instance;
    public GameObject[] Rooms;
    public Parallax[] Parallaxes;
    public GameObject Player, UI, Wallpaper;
    private int ThisRoom = 0;
    public Vector3 OffsetWallpaper;
    private void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        UI = GameObject.Find("Main Camera");
        Player = GameObject.Find("Player");
        Player.transform.position = GameObject.Find("Start").gameObject.transform.position;
        Wallpaper.transform.position = GameObject.Find("Start").gameObject.transform.position + OffsetWallpaper;
    }
    public void NextRoom()
    {
        if (ThisRoom != Rooms.Length)
        {
            Rooms[ThisRoom].SetActive(false);
            ThisRoom++;
            Rooms[ThisRoom].SetActive(true);
            PlayerTeleportToRoom(Rooms[ThisRoom]);
            Rooms[ThisRoom].SetActive(true);
        }
    }
    void PlayerTeleportToRoom(GameObject Room)
    {
        UI. transform.position = new Vector2(GameObject.Find("Start").gameObject.transform.position.x, GameObject.Find("Start").gameObject.transform.position.y);
        Wallpaper.transform.position = GameObject.Find("Start").gameObject.transform.position + OffsetWallpaper;
        foreach (Parallax parallax in Parallaxes)
        {
            parallax.UpdateValues();
        }
        Player.transform.position = GameObject.Find("Start").gameObject.transform.position;
        Player.GetComponent<SpriteRenderer>().enabled = true;
        Player.GetComponent<PlayerController>()._ActivePlayer = true;
        Player.GetComponent<Rigidbody2D>().isKinematic = false;

    }
}
