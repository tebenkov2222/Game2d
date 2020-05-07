using System.Collections.Generic;
using UnityEngine;

public class SceletonAPI : MonoBehaviour
{
    [SerializeField] GameObject AtackRegion, Player;
    Animator anim;
    MobsController Controller;
    public int Damage = 0;
    private bool
        endAnimAtack = true,
        Damagebool = false;
    private int
        State = 0;
    public float
        timeLastAtackPlayer = -2,
        timePlayerAtack = 2;
    private  MobsController.StateMobs StateSceleton
    {
        get { return (MobsController.StateMobs)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }
    private void Awake()
    {
        Player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        Controller = this.GetComponent<MobsController>();
    }
    private void Update()
    {
        GetData();
        CheckAtackMode();
    }
    private void GetData()
    {
        List<int> Data = Controller.GetData(
            new List<bool>
            {
            endAnimAtack
            });
        State = Data[0];
        if (endAnimAtack && State != (int)MobsController.StateMobs.Atack) StateSceleton = (MobsController.StateMobs)State;
    }
    private void CheckAtackMode()
    {
        if (!endAnimAtack) StateSceleton = MobsController.StateMobs.Atack;
        if (State == (int)MobsController.StateMobs.Atack)
        {
            if (Time.time - timeLastAtackPlayer > timePlayerAtack)
            {
                Debug.Log("Time");
                StateSceleton = MobsController.StateMobs.Atack;
                timeLastAtackPlayer = Time.time;
                endAnimAtack = false;
            }
            else
            {
                StateSceleton = MobsController.StateMobs.Idle;
            }
            AtackVoid();
        }
    }

    #region Atack
    public void DamageVoid()
    {
        Damagebool = true;
    }
    public void endAnimationAttack()
    {
        StateSceleton = MobsController.StateMobs.Idle;
        endAnimAtack = true;
    }
    private void AtackVoid()
    {
        if (Damagebool)
        {
            Damagebool = false;
            List<Collider2D> ColAttack = new List<Collider2D>(Physics2D.OverlapBoxAll(AtackRegion.transform.position, AtackRegion.transform.localScale, 0));
            for (int i = 0; i < ColAttack.Count; ++i)
            {
                if (ColAttack[i].gameObject.name == "Player")
                {
                    Debug.Log("Damage");
                    ColAttack[i].gameObject.GetComponent<PlayerController>().GetDamage(Damage);
                    ColAttack[i].gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Player.transform.position.x - this.transform.position.x, 1f) * 300);
                    break;
                }
            }
        }
    }
    public void DeleteAPI()
    {
        Destroy(this.gameObject.GetComponent<Animator>());
        Destroy(this);
    }
    #endregion
}
