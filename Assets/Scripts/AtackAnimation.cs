using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackAnimation : MonoBehaviour
{
    public bool AtackAnim = false;
    private void Update()
    {
        if (AtackAnim)
        {
            this.GetComponent<Animator>().SetBool("Atack", true);
        }
        else
        {
            this.GetComponent<Animator>().SetBool("Atack", false);
        }
    }
    public void Atack()
    {
        this.GetComponent<Animator>().Play("Atack");
        AtackAnim = true;
    }
    void Atackend()
    {
        AtackAnim = false;
    }
}
