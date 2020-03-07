using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceletonAPI : MonoBehaviour
{
    #region public
    public float Health = 5, AtackRadius = 15, timeState = 2, speedGo = 2, timePLayerSearch = 15, timeLastVisiblePlayer = -15;
    public GameObject Player, eyeRaycast, BackeyeRaycast, FB, B, FW, HITCOL;
    [HideInInspector] public bool FBbool = false, Bbool = false, FWbool = false;
    #endregion
    #region private
    private Animator anim;
    private float timefromState = 0;
    private Rigidbody2D Rb;
    private bool Atack = false, Run = false, Atack_Ready = false, playerFinded = false, State = false, rightMove = true;
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
        if (!Death())
        {
            if (Atack_Ready) checkEye();
            playerFinded = TimeActiveSearch();
            if (!playerFinded) IdleState();
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
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }
    }
    #endregion
    #region AtackPlayer

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
            GoForward();
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
    private void GoForward()
    {
        if (rightMove) Rb.velocity = new Vector2(1 * speedGo, Rb.velocity.y);
        else Rb.velocity = new Vector2(-1 * speedGo, Rb.velocity.y);
    }
    #endregion
    #endregion

}
