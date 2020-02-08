using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCheck : MonoBehaviour
{
    public List<Collider2D> cols;
    public bool GetStateEmpty()
    {
        cols = new List<Collider2D>(Physics2D.OverlapPointAll(this.gameObject.transform.position));
        for (int i = 0; i < cols.Count; ++i)
        {
            if (cols[i].gameObject.tag == "Level")
            {
                return true;
            }
        }
        return false;
    }
}
