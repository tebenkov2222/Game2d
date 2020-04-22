using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SodesPlayer : MonoBehaviour
{
    public LayerMask lm;
    public int mode = 1;
    public GameObject player;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (mode == 2) player.GetComponent<PlayerController>()._RightSide = true;
        if (mode == 3) player.GetComponent<PlayerController>()._LeftSide = true;
        if (collision.gameObject.tag == "StopBlock")
        {
            if (mode == 2) player.GetComponent<PlayerController>()._RightBlc = true;
            if (mode == 3) player.GetComponent<PlayerController>()._LeftBlc = true;
        }
    }
    
    private bool S()
    {
        List<Collider2D> arr = new List<Collider2D>(Physics2D.OverlapPointAll(this.transform.position, lm));
        return arr.Count != 0;
    }
    private void Update()
    {
        if (mode == 4) player.GetComponent<PlayerController>()._JumpBlock = S();
        if (mode == 1) player.GetComponent<PlayerController>()._DownSide  = S();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (mode == 2) player.GetComponent<PlayerController>()._RightSide = false;
        if (mode == 3) player.GetComponent<PlayerController>()._LeftSide = false;
        if (collision.gameObject.tag == "StopBlock")
        {
            if (mode == 2) player.GetComponent<PlayerController>()._RightBlc = false;
            if (mode == 3) player.GetComponent<PlayerController>()._LeftBlc = false;
        }
    }
}
