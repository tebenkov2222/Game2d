using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SodesPlayer : MonoBehaviour
{
    public int mode = 1;
    public GameObject player;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (mode == 1) player.GetComponent<MovePlayer>()._DownSide = true;
        if (mode == 2) player.GetComponent<MovePlayer>()._RightSide = true;
        if (mode == 3) player.GetComponent<MovePlayer>()._LeftSide = true;
        if (collision.gameObject.tag == "StopBlock")
        {
            if (mode == 2) player.GetComponent<MovePlayer>()._RightBlc = true;
            if (mode == 3) player.GetComponent<MovePlayer>()._LeftBlc = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (mode == 1) player.GetComponent<MovePlayer>()._DownSide = false;
        if (mode == 2) player.GetComponent<MovePlayer>()._RightSide = false;
        if (mode == 3) player.GetComponent<MovePlayer>()._LeftSide = false;
        if (collision.gameObject.tag == "StopBlock")
        {
            if (mode == 2) player.GetComponent<MovePlayer>()._RightBlc = false;
            if (mode == 3) player.GetComponent<MovePlayer>()._LeftBlc = false;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
