using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRoomSettingsScript : MonoBehaviour
{
    public GameObject[] Rooms;
    public GameObject Player;
    private int ThisRoom = 0;
    private void Awake()
    {
        Player = GameObject.Find("Player");
        Player.transform.position = GameObject.Find("Start").gameObject.transform.position;
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
        Player.transform.position = GameObject.Find("Start").gameObject.transform.position;
        Player.GetComponent<SpriteRenderer>().enabled = true;
        Player.GetComponent<PlayerController>().ActivePlayer = true;
        Player.GetComponent<Rigidbody2D>().isKinematic = false;

    }
}
