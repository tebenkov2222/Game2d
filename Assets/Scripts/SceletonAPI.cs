using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceletonAPI : MonoBehaviour
{
    #region public
    [SerializeField]  private float Health = 5, AtackRadius = 1f, SearchRadius = 15, timeState = 2, speedRun = 4, speedWalk = 2, Damage = 1, heightFindPlayer = 6, heightState = 4;
    [SerializeField]  private GameObject eyeRaycast, BackeyeRaycast, FwdBttmBoolGO, BttmBoolGO, FwdWallBoolGO, AtackRegion;
    [SerializeField] LayerMask lmask;
    [HideInInspector] public bool FBbool = false, Bbool = false, FWbool = false, Atack = false, Damagebool = false;
    #endregion
    #region private
    private GameObject Player;
    private Animator anim;
    private float timefromState = 0, timePLayerSearch = 15, timeLastVisiblePlayer = -16, timePLayerAtack = 1, timeLastAtackPlayer = 0;
    private Rigidbody2D Rb;
    private bool  Run = false, Atack_Ready = false, playerFinded = false, State = false, rightMove = true;
    #endregion
    void Start()
    {
        anim = this.GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player");
        GetBoolFromEmpty();
        GetPlayerInAtackCircle();
    }
    void Update()
    {
        //если не умер
        if (!Death())
        {
            //если находится в зоне реагирования на плеера
            if (Atack_Ready) checkEye();
            playerFinded = TimeActiveSearch();
            //если 
            if (playerFinded)
            {
                if (Raycast(BackeyeRaycast, false))
                {
                    rightMove = !rightMove;
                    transform.Rotate(new Vector3(0, 180, 0));
                }
                if (!GoToPlayer())
                {
                    if (Time.time - timeLastAtackPlayer > timePLayerAtack)
                    {
                        this.GetComponent<Animator>().Play("SceletonAtack", 0);
                        timeLastAtackPlayer = Time.time;
                        SetAnim(false, false, true, false);
                        Atack = true;
                    }
                    else
                    {
                        SetAnim(false, false, false, true);
                    }
                    AtackVoid();
                }
            }
            else { IdleState(); }


            GetPlayerInAtackCircle();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #region Main
    private  bool CanRunToPlayer()
    {
        if (Player.transform.position.y - this.transform.position.y < heightState) return false;
        else return true;
    }
    private bool Death()
    {
        if (Health > 0) return false;
        return true; ;
    }
    #endregion
    #region SearchPlayer 
    /// <summary>
    /// получает данные с точек перед скелетом
    /// </summary>
    private void GetBoolFromEmpty()
    {
        FBbool = FwdBttmBoolGO.GetComponent<EmptyCheck>().GetStateEmpty();
        Bbool = BttmBoolGO.GetComponent<EmptyCheck>().GetStateEmpty();
        FWbool = FwdWallBoolGO.GetComponent<EmptyCheck>().GetStateEmpty();
    }
    /// <summary>
    /// ищет в сфере радиусом SearchRadius игрока 
    /// </summary>
    private void GetPlayerInAtackCircle()
    {
        Player = GameObject.Find("Player");
        List<Collider2D> cols = new List<Collider2D>(Physics2D.OverlapCircleAll(this.gameObject.transform.position, SearchRadius));
        if (cols.LastIndexOf(Player.GetComponent<Collider2D>()) != -1)
        {
            Atack_Ready = true;
        }
        else
        {
            Atack_Ready = false;
        }
    }
    /// <summary>
    /// проверяет на видимость игрока в зоне На входе ГО с которого происходит проверка
    /// </summary>
    private bool Raycast(GameObject Start, bool timeRes)
    {
        RaycastHit2D hit = Physics2D.Raycast(Start.transform.position, (Player.transform.position - Start.transform.position), lmask);
        Debug.DrawRay(Start.transform.position, (Player.transform.position - Start.transform.position), Color.red);
        if (hit.collider.gameObject.tag == "Player")
        {
            if (timeRes) timeLastVisiblePlayer = Time.timeSinceLevelLoad;
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// прошло ли время активности ожидания видимости плеера
    /// </summary>
    private bool TimeActiveSearch()
    {
        if (Time.timeSinceLevelLoad - timeLastVisiblePlayer > timePLayerSearch || timeLastVisiblePlayer < 0) return false;
        else return true;
    }
    /// <summary>
    /// проверка глаз и затылка
    /// </summary>
    private void checkEye()
    {
        playerFinded = Raycast(eyeRaycast, true) && (Player.transform.position.y - this.transform.position.y) < heightFindPlayer;
        if (Raycast(BackeyeRaycast, false))
        {
            if (!Player.GetComponent<MovePlayer>()._DownBool && Player.GetComponent<MovePlayer>().MoveActive)
            {
                rightMove = !rightMove;
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }
    }
    #endregion
    #region State
    void SetAnim(bool Runb, bool Walkb, bool Atackb, bool States)
    {
        anim.SetBool("Run", Runb);
        anim.SetBool("Walk", Walkb);
        anim.SetBool("Atack", Atackb);
        Atack = Atackb;
        Run = Runb;
        State = States;
    }
    
    #region RunsToPLayerState
    private bool GoToPlayer()
    {
        if (Vector3.Distance(eyeRaycast.gameObject.transform.position, Player.transform.position) > AtackRadius && !Atack)
        {
            GetBoolFromEmpty();
            if (FBbool && !FWbool )
            {
                SetAnim(true, false, false, false);
                if (Bbool) GoForward(speedRun);
            }
            else
            {
                SetAnim(false, false, false, true);
                if (Bbool) Rb.velocity = new Vector2(0, Rb.velocity.y);
            }
            return true;
        }
        return false;
    }
    #endregion
    #region Atack
    public void DamageVoid()
    {
        Damagebool = true;
    }
    public void endAnimationAttack()
    {
        Atack = false;
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
                    ColAttack[i].gameObject.GetComponent<MovePlayer>().GetDamage(Damage);
                }
            }
        }
    }
    #endregion
    #region Idle
    private void IdleState()
    {
        GetBoolFromEmpty();
        if (FBbool && !FWbool)
        {
            if (Bbool) GoForward(speedWalk);
            State = false;
        }
        else
        {
            if (Bbool) Rb.velocity = new Vector2(0, Rb.velocity.y);    
            if (!State)
            {
                SetAnim(false, false, false, true);
                timefromState = Time.realtimeSinceStartup;
            }
            if (Time.realtimeSinceStartup - timefromState > timeState && State)
            {
                SetAnim(false, true, false, false);
                rightMove = !rightMove;
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }
    }
    private void GoForward(float speed)
    {
        Debug.Log(this.gameObject.name + " Walk");
        if (rightMove) Rb.velocity = new Vector2(1 * speed, Rb.velocity.y);
        else Rb.velocity = new Vector2(-1 * speed, Rb.velocity.y);
    }
    #endregion
    #endregion
    public void GetDamage(float damage)
    {
        if (Random.Range(0, 50) == 1)
        {
            Health -= damage * 0.3f;
        }
        else
        {
            Health -= damage;
        }
    }

}
