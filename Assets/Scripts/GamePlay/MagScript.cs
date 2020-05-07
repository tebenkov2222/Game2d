using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagScript : MonoBehaviour
{
    #region public
    [SerializeField] private float Health = 5, AtackRadius = 15f, SearchRadius = 20, MobsRadius = 20, timeState = 2, speedRun = 4, speedWalk = 2, speedFire = 5, heightFindPlayer = 6;
    [SerializeField] private GameObject eyeRaycast, BackeyeRaycast, FwdBttmBoolGO, BttmBoolGO, FwdWallBoolGO, StateGO, StateMobs, firePrefab, Spawmer;
    [SerializeField] LayerMask lmask;
    [HideInInspector] public bool FBbool = false, Bbool = false, FWbool = false, Atack = false, Damagebool = false;
    #endregion
    #region private
    private GameObject Player;
    private Animator anim;
    private float timefromState = 0, timePLayerSearch = 15, timeLastVisiblePlayer = -16, timePLayerAtack = 1, timeLastAtackPlayer = 0;
    private Rigidbody2D Rb;
    private bool DeathBool = false, Run = false, Atack_Ready = false, playerFinded = false, State = false, rightMove = true;
    private Vector3 LastVisiblePositionPlayer;
    #endregion
    void Start()
    {
        speedRun = Random.Range(2, 2.5f);
        speedWalk = Random.Range(1, 2f);
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
                checkState();
                if (!RightMove())
                {
                    rightMove = !rightMove;
                    transform.Rotate(new Vector3(0, 180, 0));
                }
                if (!GoToPlayer())
                {
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.gameObject.GetComponent<Rigidbody2D>().velocity.y);
                    SetAnim(false, false, true, false);
                    
                    if (Time.time - timeLastAtackPlayer > timePLayerAtack)
                    {
                        timeLastAtackPlayer = Time.time;
                        AtackVoid();
                    }
                }
            }
            else { IdleState(); }


            GetPlayerInAtackCircle();
        }
        else
        {
            if (!DeathBool)
            {
                DeathBool = true;
                SetAnim(false, false, false, false);
                anim.SetBool("Death", true);
                for (int i = 0; i < 6; ++i) Destroy(transform.GetChild(i).gameObject);
                Destroy(this);
            }
        }
    }
    #region Main
    private void checkState()
    {
        if (!Raycast(eyeRaycast, true) && playerFinded)
        {
            if (StateMobs.GetComponent<StateActive>().LastState != "Que")
            {
                StateMobs.GetComponent<StateActive>().EndVoid(new List<bool> { false, true, false });
                StateMobs.GetComponent<StateActive>().Questions();
            }
            else
            {
                StateMobs.GetComponent<StateActive>().EndVoid(new List<bool> { true, true, false });
            }
        }
    }
    private void StatePosition()
    {
        StateMobs.transform.position = StateGO.transform.position;
    }
    public void PlayerFindOtherMobs(Vector2 PlayerPosition)
    {
        LastVisiblePositionPlayer = PlayerPosition;
        timeLastVisiblePlayer = Time.timeSinceLevelLoad;
        playerFinded = true;
    }

    private void SetFindOtherMobs()
    {
        List<Collider2D> cols = new List<Collider2D>(Physics2D.OverlapCircleAll(this.gameObject.transform.position, MobsRadius));
        for (int i = 0; i < cols.Count; i++)
        {
            RaycastHit2D hit1 = Physics2D.Raycast(eyeRaycast.transform.position, (cols[i].gameObject.transform.position - eyeRaycast.transform.position), MobsRadius, lmask);
            RaycastHit2D hit2 = Physics2D.Raycast(BackeyeRaycast.transform.position, (cols[i].gameObject.transform.position - BackeyeRaycast.transform.position), MobsRadius, lmask);
            if (hit1.collider.gameObject == cols[i].gameObject || hit2.collider.gameObject == cols[i].gameObject)
            {
                if (cols[i].tag == "Sceleton") if (cols[i].gameObject.GetComponent<SceletonAPI>()) cols[i].gameObject.GetComponent<SceletonAPI>().PlayerFindOtherMobs(LastVisiblePositionPlayer);
                if (cols[i].tag == "Strazh") if (cols[i].gameObject.GetComponent<MagScript>()) cols[i].gameObject.GetComponent<MagScript>().PlayerFindOtherMobs(LastVisiblePositionPlayer);
            }
        }
    }
    private bool RightMove()
    {
        if (Mathf.Abs(BackeyeRaycast.transform.position.x - LastVisiblePositionPlayer.x) >
            Mathf.Abs(eyeRaycast.transform.position.x - LastVisiblePositionPlayer.x)) return true;
        else return false;
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
        RaycastHit2D hit = Physics2D.Raycast(Start.transform.position, (Player.transform.position - Start.transform.position), 40, lmask);
        if (hit.collider.gameObject.tag == "Player")
        {
            LastVisiblePositionPlayer = hit.transform.position;
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
        if (Raycast(eyeRaycast, true) && (Player.transform.position.y - this.transform.position.y) < heightFindPlayer)
        {
            SetFindOtherMobs();
            playerFinded = true;
            if (StateMobs.GetComponent<StateActive>().LastState != "Exc")
            {
                StateMobs.GetComponent<StateActive>().EndVoid(new List<bool> { true, false, false });
                StateMobs.GetComponent<StateActive>().Exlamation();
            }
            else
            {
                StateMobs.GetComponent<StateActive>().EndVoid(new List<bool> { true, true, false });
            }
        }
        else
        {
            StateMobs.GetComponent<StateActive>().RenameLastState();
        }
        if (Raycast(BackeyeRaycast, false))
        {
            if (!Player.GetComponent<PlayerController>()._DownBool && Player.GetComponent<PlayerController>()._Movd)
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
        if (Vector2.Distance(eyeRaycast.gameObject.transform.position, Player.transform.position) > AtackRadius && !Atack)
        {
            GetBoolFromEmpty();
            if (FBbool && !FWbool)
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
    public void endAnimationAttack()
    {
        Damagebool = false;
    }
    public void StartAnimationAttack()
    {
        Damagebool = true;
    }
    private void AtackVoid()
    {
        if (Damagebool) { 
            GameObject var = GameObject.Instantiate(firePrefab, Spawmer.transform.position, Quaternion.identity);
            if (rightMove)
            {
                var.GetComponent<SpriteRenderer>().flipX = true;
                var.GetComponent<Rigidbody2D>().velocity = new Vector2(1 * speedFire, 0);
            }
            else var.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * speedFire, 0);
        }
    }
    #endregion
    #region Idle
    private void IdleState()
    {
        GetBoolFromEmpty();
        if (FBbool && !FWbool)
        {
            SetAnim(false, true, false, false);
            Atack = false;
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
        StatePosition();
        if (((Raycast(eyeRaycast, true) || this.transform.position.x != LastVisiblePositionPlayer.x) && playerFinded) || !playerFinded)
        {
            if (rightMove) Rb.velocity = new Vector2(1 * speed, Rb.velocity.y);
            else Rb.velocity = new Vector2(-1 * speed, Rb.velocity.y);
        }
    }
    #endregion
    #endregion
    public void GetDamage(float damage)
    {
        if (Random.Range(0, 30) == 1)
        {
            StatePosition();
            StateMobs.GetComponent<StateActive>().EndVoid(new List<bool> { true, true, true });
            StateMobs.GetComponent<StateActive>().Krit();
            Health -= damage * 0.3f;
        }
        else
        {
            Health -= damage;
        }
    }

}
