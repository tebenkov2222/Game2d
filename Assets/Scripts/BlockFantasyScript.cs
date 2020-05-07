using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFantasyScript : MonoBehaviour
{
    GameObject Player;
    public GameObject Empty;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool pl = false;
        List<Collider2D> cols = new List<Collider2D>(Physics2D.OverlapBoxAll(Empty.transform.position, Empty.transform.localScale, 0));
        for (int i = 0; i < cols.Count; ++i)
        {
            if (cols[i].gameObject.name == "Player")
            {
                pl = true;
            }
        }
        if (Player.GetComponent<MovePlayer>()._elevatorState || pl)
        {
            Physics2D.IgnoreLayerCollision(10, 15, true);
        }
        if (Player.GetComponent<Rigidbody2D>().velocity.y > 0.01f ) Physics2D.IgnoreLayerCollision(10, 15, true);
        else
        {
            if (!Player.GetComponent<MovePlayer>()._DownBool && !Player.GetComponent<MovePlayer>()._elevatorState && !Player.GetComponent<MovePlayer>()._JumpState)
            {
                if (Player.GetComponent<Rigidbody2D>().velocity.y <= 0.01f) Physics2D.IgnoreLayerCollision(10, 15, false);
            }
        }
    }
}
