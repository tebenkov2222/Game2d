using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationWoman : MonoBehaviour
{
    Animator anim;
    #region FromAnimator
    private void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
    }
    public void StartBowAtack(bool State)
    {

    }
    public void BowAtack()
    {

    }
    public void EndAnimationSwordIdle()
    {
        this.GetComponent<MovePlayer>()._SwordIdle = false;
    }
    public void EndBowAtack()
    {
        anim.SetBool("AtackBow", false);
    }
    public void EndBowAtackSitDown()
    {
        anim.SetBool("AtackBowSitDown", false);
    }
    public void EndBowAtackJumped(){anim.SetBool("AtackBowJumped", false);}
    public void SwordAtack(){this.GetComponent<PlayerController>()._AtackMob = true;}
    public void endSwordAtack()
    {
        anim.SetInteger("State", 0);
        //ATACK VOID
    }
    public void EndHandAtack()
    {
        anim.SetBool("AtackHandUp", false);
    }
    public void EndDamage()
    {
        anim.SetBool("Damage", false);
        //DAMAGE VOID
    }
    #endregion
    #region Set

    #endregion

}
