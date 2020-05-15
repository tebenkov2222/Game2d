using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float Damage = 5f;
    public GameObject tip;
    public LayerMask lm, lmAtack;
    public bool Active = true, Vall = false;
    private void Awake()
    {
        this.GetComponent<Rigidbody2D>().centerOfMass = -tip.transform.localPosition;
    }
    private void FixedUpdate()
    { 
        if (Active)
        {
            List<Collider2D> arr = new List<Collider2D>(Physics2D.OverlapBoxAll(tip.transform.position, tip.transform.lossyScale, 0));
            for (int i = 0; i < arr.Capacity; i++)
            {
                if (arr[i].gameObject.tag == "Level")
                {
                    this.gameObject.layer = 10;
                    Active = false;
                    Vall = true;
                    this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                }
                if (arr[i].gameObject.GetComponent<MobsController>())
                {
                    Active = false;
                    this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    this.GetComponent<BoxCollider2D>().isTrigger = true;
                    this.GetComponent<FixedJoint2D>().enabled = true;
                    this.GetComponent<FixedJoint2D>().connectedBody = arr[i].GetComponent<Rigidbody2D>();
                    arr[i].gameObject.GetComponent<MobsController>().GetDamage(Damage);
                }

            }
        }
        if (!Vall && !Active && this.GetComponent<FixedJoint2D>()) if( !this.GetComponent<FixedJoint2D>().connectedBody.gameObject.GetComponent<MobsController>()) Break();
    }
    public void Break()
    {
        Debug.Log("NON");
        this.gameObject.layer = 10;
        this.GetComponent<BoxCollider2D>().isTrigger = false;
        this.GetComponent<Rigidbody2D>().gravityScale = 7;
        Destroy(this.GetComponent<FixedJoint2D>());
    }
}
