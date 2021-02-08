using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SodesPlayer : MonoBehaviour
{
    public LayerMask lm;
    public int mode = 1;
    public PlayerController PlayerController;
    private bool StopBlockCheck()
    {
        List<Collider2D> arr = new List<Collider2D>(Physics2D.OverlapBoxAll(this.transform.position, this.transform.localScale, 0, lm));
        for (int i = 0; i < arr.Count; i++)
        {
            if (arr[i].tag == "StopBlock") return true;
        }
        return false;
    }

    private bool SBox()
    {
        List<Collider2D> arr = new List<Collider2D>(Physics2D.OverlapBoxAll(this.transform.position,this.transform.localScale,0 , lm));
        return arr.Count != 0;
    }
    private void Update()
    {
        if (mode == 1) PlayerController._DownSide = SBox();
        if (mode == 2) PlayerController._RightSide = SBox();
        if (mode == 3) PlayerController._LeftSide = SBox();
        if (mode == 4) PlayerController._JumpSide = SBox();
        if (mode == 5) PlayerController._LeftSideJump = SBox();
        if (mode == 6) PlayerController._RightSideJump = SBox();

        //StopBlock
        if (mode == 2) PlayerController._RightBlc = StopBlockCheck();
        if (mode == 3) PlayerController._LeftBlc = StopBlockCheck();

    }
}
