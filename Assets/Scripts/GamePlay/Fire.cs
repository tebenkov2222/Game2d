using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    bool Damage = false;
    [SerializeField] LayerMask lmask;
    [SerializeField] float Radius;
    void Update()
    {
        List<Collider2D> cols = new List<Collider2D>(Physics2D.OverlapCircleAll(this.gameObject.transform.position, Radius, lmask));
        for (int i = 0; i < cols.Count; ++i)
        {
            if (cols[i].gameObject.name == "Player" && !Damage)
            {
                Damage = true;
                cols[i].gameObject.GetComponentInParent<PlayerController>().GetDamage(3f);
                Destroy();
            }
            if (cols[i].gameObject.layer != 10)
            {
                Destroy();
            }
        }
    }
    public void Destroy()
    {
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.GetComponent<Animator>().SetBool("Boom", true);
    }
    public void EndAnim()
    {
        Destroy(this.gameObject);
    }
}
