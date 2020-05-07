using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagScript : MonoBehaviour
{
    [SerializeField] GameObject firePrefab, Spawmer;
    Animator anim;
    MobsController Controller;
    public int Damage = 0;
    private bool
        endAnimAtack = true,
        Damagebool = false;
    private int
        State = 0;
    public float
        timeLastAtackPlayer = 0,
        timePlayerAtack = 2,
        speedFire = 2;
    private MobsController.StateMobs StateMag
    {
        get { return (MobsController.StateMobs)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }
    private void Awake()
    {
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
        if (State != (int)MobsController.StateMobs.Atack) StateMag = (MobsController.StateMobs)State;
    }
    private void CheckAtackMode()
    {
        if (State == (int)MobsController.StateMobs.Atack)
        {
            StateMag = MobsController.StateMobs.Atack;
            if (Time.time - timeLastAtackPlayer > timePlayerAtack)
            {
                AtackVoid();
                timeLastAtackPlayer = Time.time;
                endAnimAtack = false;
            }
        }
    }

    #region Atack
    public void DamageVoid()
    {
        Damagebool = true;
    }
    public void endAnimationAttack()
    {
        StateMag = MobsController.StateMobs.Idle;
        endAnimAtack = true;
    }
    private bool rightMove()
    {
        return (this.transform.rotation.eulerAngles.y == 180f);
    }
    private void AtackVoid()
    {
        if (Damagebool)
        {
            GameObject var = GameObject.Instantiate(firePrefab, Spawmer.transform.position, Quaternion.identity);
            if (rightMove())
            {
                var.GetComponent<SpriteRenderer>().flipX = true;
                var.GetComponent<Rigidbody2D>().velocity = new Vector2(1 * speedFire, 0);
            }
            else var.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * speedFire, 0);
        }
    }
    public void DeleteAPI()
    {
        Destroy(this.gameObject.GetComponent<Animator>());
        Destroy(this);
    }
    #endregion
}
