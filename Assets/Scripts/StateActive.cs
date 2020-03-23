using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateActive : MonoBehaviour
{
    Animator th;
    private void Start()
    {
        th = this.GetComponent<Animator>();
    }
    public bool Active = false;
    public void Questions()
    {
        if (!Active)
        {
            Active = true;
            th.SetBool("Que", true);
        }
    }
    public void Exlamation()
    {
        if (!Active)
        {
            Active = true;
            th.SetBool("Exc", true);
        }
    }
    public void Krit()
    {
        if (!Active)
        {
            Active = true;
            th.SetBool("Krit", true);
        }
    }
    public void EndVoid(List<bool>States)
    {
        if (States[0])th.SetBool("Que", false);
        if (States[1]) th.SetBool("Exc", false);
        if (States[2]) th.SetBool("Krit", false);
        if (States[0] || States[1] || States[2]) Active = false;
    }
}
