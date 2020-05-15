using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobsController : MonoBehaviour
{
    #region public
    public LayerMask MaskDiy;
    [SerializeField] private float 
        Health = 5, 
        AtackRadius = 1f, 
        SearchRadius = 15, 
        MobsRadius = 20, 
        timeState = 2, 
        speedRun = 4, 
        speedWalk = 2, 
        heightFindPlayer = 6,
        SpeedRunMin = 2,
        SpeedRunMax = 3.5f,
        SpeedWalkMin = 1,
        SpeedWalkMax = 2;
    public GameObject
        eyeRaycast, 
        BackeyeRaycast, 
        FwdBttmBoolGO, 
        BttmBoolGO, 
        FwdWallBoolGO, 
        StateGO, 
        StateMobsGameObj;
    [SerializeField] LayerMask lmask;
    #endregion
    #region private

    #region States Mobs
    private int State = 0,
    StateMobsSprite = 0;
    public StateMobsGO StateSprite
    {
        get { return (StateMobsGO)StateMobsSprite; }
        set { StateMobsSprite = (int)value; }
    }
    public enum StateMobsGO
    {
        Exc,
        Qwe,
        None
    }
    public StateMobs StateMob
    {
        get { return (StateMobs)State; }
        set { State = (int)value; }
    }
    public enum StateMobs
    {
        Idle,
        Walk,
        Run,
        Atack,
        Death
    }
    #endregion
    [HideInInspector]
    #region Sodes Mobs
    public bool
        FBSodes = false,
        BSodes = false,
        FWSodes = false,
        rightMove = true;
    #endregion
    private GameObject Player;
    private float timefromState = 0,
        timePLayerSearch = 15,
        timeLastVisiblePlayer = -16;
    private Rigidbody2D Rb;
    private bool
        AtackAnim = false,
        Atack_Ready = false,
        playerFinded = false,
        endAnimAtack = true;
    private Vector3 LastVisiblePositionPlayer;
    #endregion
    void Start()
    {
        speedRun = Random.Range(SpeedRunMin, SpeedRunMax);
        speedWalk = Random.Range(SpeedWalkMin, SpeedWalkMax);
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
                    if (CheckAtackPlayer()) StateMob = StateMobs.Atack;
                    else StateMob = StateMobs.Idle;
                }
            }
            else { IdleState(); }


            GetPlayerInAtackCircle();
        }
        else
        {
            this.gameObject.layer = 12;
            StateMob = StateMobs.Death;
            for (int i = 0; i < 6; ++i) Destroy(transform.GetChild(i).gameObject);
            Destroy(this);
        }
    }
    #region Main
    public List<int> GetData(List <bool> list)
    {
        endAnimAtack = list[0];
        return new List<int> {
            State
        };
    }
    private void checkState()
    {
        if (!Raycast(eyeRaycast, true, !BackEye()) && playerFinded)
        {
            if (StateMobsGameObj.GetComponent<StateActive>().LastState != "Que")
            {
                StateMobsGameObj.GetComponent<StateActive>().EndVoid(new List<bool> { false, true, false });
                StateMobsGameObj.GetComponent<StateActive>().Questions();
            }
            else
            {
                StateMobsGameObj.GetComponent<StateActive>().EndVoid(new List<bool> { true, true, false });
            }
        }
    }
    private void StatePosition()
    {
        StateMobsGameObj.transform.position = StateGO.transform.position;
    }

    private void SetFindOtherMobs()
    {
        List<Collider2D> cols = new List<Collider2D>(Physics2D.OverlapCircleAll(this.gameObject.transform.position, MobsRadius));
        for (int i = 0; i < cols.Count; i++)
        {
            RaycastHit2D hit1 = Physics2D.Raycast(eyeRaycast.transform.position, (cols[i].gameObject.transform.position - eyeRaycast.transform.position), MobsRadius, lmask);
            RaycastHit2D hit2 = Physics2D.Raycast(BackeyeRaycast.transform.position, (cols[i].gameObject.transform.position - BackeyeRaycast.transform.position), MobsRadius, lmask);
            if (hit1.collider.gameObject == cols[i].gameObject || hit2.collider.gameObject == cols[i].gameObject)
                if (cols[i].gameObject.GetComponent<MobsController>()) cols[i].gameObject.GetComponent<MobsController>().PlayerFindOtherMobs(LastVisiblePositionPlayer);
        }
    }
    #region public void
    public void PlayerFindOtherMobs(Vector2 PlayerPosition)
    {
        LastVisiblePositionPlayer = PlayerPosition;
        timeLastVisiblePlayer = Time.timeSinceLevelLoad;
        playerFinded = true;
    }
    #endregion
    #region Variables
    private bool CheckLastPositionPlayer()
    {
        return Mathf.Abs(this.transform.position.x - LastVisiblePositionPlayer.x) > AtackRadius;
    }
    private bool CheckAtackPlayer()
    {
        return Mathf.Abs(this.transform.position.y - LastVisiblePositionPlayer.y) < AtackRadius;
    }
    private bool RightMove()
    {
        if (Vector2.Distance(BackeyeRaycast.transform.position, LastVisiblePositionPlayer) >
            Vector2.Distance(eyeRaycast.transform.position, LastVisiblePositionPlayer)) return true;
        else return false;
    }
    private bool BackEye()
    {
        if (Mathf.Abs(BackeyeRaycast.transform.position.x - LastVisiblePositionPlayer.x) <
            Mathf.Abs(eyeRaycast.transform.position.x - LastVisiblePositionPlayer.x)) return true;
        else return false;
    }
    private bool Death()
    {
        if (Health > 0) return false;
        return true;
    }
    #endregion

    #endregion
    #region SearchPlayer 
    /// <summary>
    /// получает данные с точек перед скелетом
    /// </summary>
    private void GetBoolFromEmpty()
    {
        FBSodes = FwdBttmBoolGO.GetComponent<EmptyCheck>().GetStateEmpty();
        BSodes = BttmBoolGO.GetComponent<EmptyCheck>().GetStateEmpty();
        FWSodes = FwdWallBoolGO.GetComponent<EmptyCheck>().GetStateEmpty();
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
    /// проверяет на видимость игрока в зоне На входе начальная поз. нужно ли обнулять время поиска и определение для поиска
    /// </summary>
    private bool Raycast(GameObject Start, bool timeRes, bool Eye)
    {
        RaycastHit2D hit = Physics2D.Raycast(Start.transform.position, (Player.transform.position - Start.transform.position), 40, lmask);
        if (hit.collider.gameObject.tag == "Player" && Eye)
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
        if (Raycast(eyeRaycast, true, !BackEye()) && (Player.transform.position.y - this.transform.position.y) < heightFindPlayer)
        {

            SetFindOtherMobs();
            playerFinded = true;
            if (StateMobsGameObj.GetComponent<StateActive>().LastState != "Exc")
            {
                StateMobsGameObj.GetComponent<StateActive>().EndVoid(new List<bool> { true, false, false });
                StateMobsGameObj.GetComponent<StateActive>().Exlamation();
            }
            else
            {
                StateMobsGameObj.GetComponent<StateActive>().EndVoid(new List<bool> { true, true, false });
            }
        }
        else
        {
            StateMobsGameObj.GetComponent<StateActive>().RenameLastState();
        }
        if (Raycast(BackeyeRaycast, false, true) && BackEye())
        {
            if (Player.GetComponent<PlayerController>()._Movd)
            {
                rightMove = !rightMove;
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }
    }
    #endregion
    #region State

    #region RunsToPLayerState
    private bool GoToPlayer()
    {
        if (CheckLastPositionPlayer() && endAnimAtack)
        {
            GetBoolFromEmpty();
            if (endAnimAtack && FBSodes && !FWSodes)
            {
                StateMob = StateMobs.Run;
                if (BSodes) GoForward(speedRun);
            }
            else
            {
                StateMob = StateMobs.Idle;
                if (BSodes) Rb.velocity = new Vector2(0, Rb.velocity.y);
            }
            return true;
        }
        return false;
    }
    #endregion
    #region Idle

    private void IdleState()
    {
        GetBoolFromEmpty();
        if (FBSodes && !FWSodes)
        {
            if (BSodes)
            {
                StateMob = StateMobs.Walk;
                GoForward(speedWalk);
            }
        }
        else
        {
            if (BSodes) Rb.velocity = new Vector2(0, Rb.velocity.y);
            if (StateMob != StateMobs.Idle)
            {
                StateMob = StateMobs.Idle;
                timefromState = Time.realtimeSinceStartup;
            }
            if (Time.realtimeSinceStartup - timefromState > timeState && StateMob == StateMobs.Idle)
            {
                StateMob = StateMobs.Walk;
                rightMove = !rightMove;
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }
    }
    private void GoForward(float speed)
    {
        StatePosition();
        if ((Raycast(eyeRaycast, true, !BackEye()) || !CheckLastPositionPlayer()));
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
            StateMobsGameObj.GetComponent<StateActive>().EndVoid(new List<bool> { true, true, true });
            StateMobsGameObj.GetComponent<StateActive>().Krit();
            Health -= damage * 0.3f;
        }
        else
        {
            Health -= damage;
        }
    }

}
