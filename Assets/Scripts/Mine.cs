using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Mine : MonoBehaviour
{
    public float speed = 0.5f, damage = 5;
    public GameObject PointBoom;
    private bool Damage = false;
    void Update()
    {
        List<Collider2D> ColAttack = new List<Collider2D>(Physics2D.OverlapBoxAll(this.transform.position, new Vector2(6, 1.6f), 0));
        for (int i = 0; i < ColAttack.Count; ++i)
        {
            if (ColAttack[i].gameObject.name == "Player")
            {
                Damage = true;
            }
        }
        if (Damage)
        {
            this.GetComponent<Animator>().Play("MineAtack"); 
            if (this.GetComponent<Light2D>().intensity < 5)
            {
                this.GetComponent<Light2D>().enabled = true;
                this.GetComponent<Light2D>().intensity += speed * Time.deltaTime;
            }
        }
    }
    public void StartAnim()
    {
        List<Collider2D> ColAttack = new List<Collider2D>(Physics2D.OverlapBoxAll(this.transform.position, new Vector2(6, 1.6f), 0));
        for (int i = 0; i < ColAttack.Count; ++i)
        {
            if (ColAttack[i].gameObject.name == "Player")
            {
                ColAttack[i].gameObject.GetComponent<PlayerController>().GetDamage(damage);
            }
        }
        this.GetComponentInChildren<Exposion>().Boom();
        this.GetComponent<Light2D>().intensity = 0;
    }
    public void endAnim()
    {
        Destroy(this.gameObject);
        this.GetComponent<Light2D>().enabled = false;
    }
}
