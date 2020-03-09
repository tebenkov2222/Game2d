using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceletonAPI : MonoBehaviour
{
    #region public
    public float Health = 5, AtackRadius = 0.5f, SearchRadius = 15, timeState = 2, speedRun = 5, speedGo = 2, Damage = 1;
    public GameObject Player, eyeRaycast, BackeyeRaycast, FB, B, FW, HITCOL, AtackRegion;
    public bool Atack = false, Damagebool = false;
    [HideInInspector] public bool FBbool = false, Bbool = false, FWbool = false;
    #endregion
    #region private
    private Animator anim;
    private float timefromState = 0, timePLayerSearch = 15, timeLastVisiblePlayer = -16, timePLayerAtack = 1, timeLastAtackPlayer = 0;
    private Rigidbody2D Rb;
    private bool  Run = false, Atack_Ready = false, playerFinded = false, State = false, rightMove = true;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player");
        GetBoolFromEmpty();
        GetPlayerInAtackCircle();
    }

    // Update is called once per frame
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
        FBbool = FB.GetComponent<EmptyCheck>().GetStateEmpty();
        Bbool = B.GetComponent<EmptyCheck>().GetStateEmpty();
        FWbool = FW.GetComponent<EmptyCheck>().GetStateEmpty();
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
        RaycastHit2D hit = Physics2D.Raycast(Start.transform.position, (Player.transform.position - Start.transform.position));
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
        playerFinded = Raycast(eyeRaycast, true);
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
        if (Vector3.Distance(this.gameObject.transform.position, Player.transform.position) > AtackRadius && !Atack)
        {
            GetBoolFromEmpty();
            if (FBbool && !FWbool)
            {
                Debug.Log("RUN");
                SetAnim(true, false, false, false);
                GoForward(speedRun);
            }
            else
            {
                Rb.isKinematic = true;
                Rb.isKinematic = false;
                Rb.velocity = new Vector2(0, Rb.velocity.y);
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
                    ColAttack[i].gameObject.GetComponent<MovePlayer>().Health -= Damage;
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
            GoForward(speedGo);
            State = false;
        }
        else
        {
            Rb.velocity = new Vector2(0, Rb.velocity.y);    
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
        if (rightMove) Rb.velocity = new Vector2(1 * speed, Rb.velocity.y);
        else Rb.velocity = new Vector2(-1 * speed, Rb.velocity.y);
    }
    #endregion
    #endregion

}
