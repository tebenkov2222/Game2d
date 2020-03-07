using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceletonAPI : MonoBehaviour
{
    #region public
    public float AtackRadius = 15, DamageRadius = 1f, timeState = 2, speedGo = 2, timePLayerSearch = 15, timeLastVisiblePlayer = -15, Health = 5f;
    public GameObject Player, eyeRaycast, BackeyeRaycast, FB, B, FW, HITCOL;
    public Animator anim;
    [HideInInspector] public bool FBbool = false, Bbool = false, FWbool = false;
    #endregion
    #region private
    private Vector3 LastPositionVisiblePlayer = new Vector3();
    private float timefromState = 0;
    private Rigidbody2D Rb;
    private bool Atack_Ready = false, playerFinded = false, State = false, Run = false, Atack = false, Walk = false, rightMove = true;
    #endregion

    #region Main
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player");
        GetBoolFromEmpty();
        GetPlayerInAtackCircle();
    }
    void Update()
    {
        if (!Death())
        {
            if (Atack_Ready) checkEye();
            playerFinded = TimeActiveSearch();
            if (!playerFinded) IdleState();
            /*else
            {
                if (Raycast(eyeRaycast, true))
                {
                }
                else GoToLastPOint();
            }*/
            GetPlayerInAtackCircle();
        }
        else Destroy(this.gameObject);
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
    /// ищет в сфере радиусом AtackRadius игрока 
    /// </summary>
    private void GetPlayerInAtackCircle()
    {
        Player = GameObject.Find("Player");
        List<Collider2D> cols = new List<Collider2D>(Physics2D.OverlapCircleAll(this.gameObject.transform.position, AtackRadius));
        if (cols.LastIndexOf(Player.GetComponent<Collider2D>()) != -1)
        {
            Atack_Ready = true;
        }
        else
        {
            Atack_Ready = false;
        }
    }
    private bool Death()
    {
        if (Health > 0) return false;
        return true; ;
    }
    /// <summary>
    /// проверяет на видимость игрока в зоне На входе ГО с которого происходит проверка и булева на обнуление времени последнего обнаружения игрока
    /// </summary>
    private bool Raycast(GameObject Start, bool timeRes)
    {
        RaycastHit2D hit = Physics2D.Raycast(Start.transform.position, (Player.transform.position - Start.transform.position));
        Debug.DrawRay(Start.transform.position, (Player.transform.position - Start.transform.position), Color.red);
        if (hit.collider.gameObject.tag == "Player")
        {
            LastPositionVisiblePlayer = hit.collider.gameObject.transform.position;
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
        if (Time.timeSinceLevelLoad - timeLastVisiblePlayer > timePLayerSearch) return false;
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
    #region AtackPlayer
    private void GoToPlayer()
    {
        /*GetBoolFromEmpty();
        if (FBbool && !FWbool)
        {
            anim.SetBool("Walk", true);
            GoForward();
            State = false;
        }
        else
        {
            anim.SetBool("Walk", false);
            if (!State)
            {
                timefromState = Time.realtimeSinceStartup;
                State = true;
            }
            if (Time.realtimeSinceStartup - timefromState > timeState && State)
            {
                rightMove = !rightMove;
                State = false;
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }*/
    }
    void GoToLastPOint()
    {

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
    #region Idle
    private void IdleState()
    {
        GetBoolFromEmpty();
        if (FBbool && !FWbool)
        {
            SetAnim(false, true, false, false);
            GoForward();
        }
        else
        {
            anim.SetBool("Walk", false);
            if (!State)
            {
                timefromState = Time.realtimeSinceStartup;
                State = true;
            }
            if (Time.realtimeSinceStartup - timefromState > timeState && State)
            {
                rightMove = !rightMove;
                State = false;
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }

        GetBoolFromEmpty();
        if (FBbool && !FWbool && Run)
        {
            SetAnim(false, true, false, false);
            GoForward();
            State = false;
        }
        else
        {
            anim.SetBool("Walk", false);
            if (!Atack)
            {
                State = true;
            }
            if (Time.realtimeSinceStartup - timefromState > timeState && State)
            {
                rightMove = !rightMove;
                State = false;
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }
    }
    private void GoForward()
    {
        if (rightMove) Rb.velocity = new Vector2(1 * speedGo, Rb.velocity.y);
        else Rb.velocity = new Vector2(-1 * speedGo, Rb.velocity.y);
    }
    #endregion
    #endregion
    #region Переменные
    /// <summary>
    /// расстояние до плеера
    /// </summary>
    float DistanceToPlayer(GameObject Start)
    {
        return Vector3.Distance(Start.transform.position, Player.transform.position);
    }
    /// <summary>
    /// если расстояние до плеера больше радиуса дамага, то Т
    /// вход радиус
    /// </summary>
    /// <returns></returns>
    bool CheckDistanceToPlayer(float radius)
    {
        if (DistanceToPlayer(this.gameObject) > radius) return true;
        return false;
    }
    #endregion
}
