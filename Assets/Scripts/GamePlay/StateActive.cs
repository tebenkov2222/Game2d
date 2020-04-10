using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateActive : MonoBehaviour
{
    public string LastState;
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
            LastState = "Que";
        }
    }
    public void Exlamation()
    {
        if (!Active)
        {
            Active = true;
            th.SetBool("Exc", true);
            LastState = "Exc";
        }
    }
    public void Krit()
    {
        if (!Active)
        {
            Active = true;
            th.Play("StateKrit", 0);
            th.SetBool("Krit", true);
        }
    }
    public void EndKrit()
    {
        th.SetBool("Krit", false);
        Active = false;
    }
    public void RenameLastState()
    {
        LastState = "";
    }
    public void EndVoid(List<bool>States)
    {
        if (States[0]) th.SetBool("Que", false);
        if (States[1]) th.SetBool("Exc", false);
        if (States[2]) th.SetBool("Krit", false);
        if (States[0] || States[1] || States[2]) Active = false;
    }
}
