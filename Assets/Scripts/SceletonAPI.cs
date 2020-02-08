using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceletonAPI : MonoBehaviour
{
    #region public
    public float AtackRadius = 15, timeState = 2, speedGo = 2;
    public GameObject Player, eyeRaycast, FB, G, FW;
    [HideInInspector] public bool FBbool = false, Gbool = false, FWbool = false;
    #endregion
    #region private
    private float timefromState = 0;
    private Rigidbody2D Rb;
    private bool Atack_Ready = false, playerFinded = false, State = false, rightMove = true;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player");
        GetBoolFromEmpty();
        GetPlayerInAtackCircle();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerFinded) IdleState();
        GetPlayerInAtackCircle();
        if (Atack_Ready) Raycast();
    }
    /// <summary>
    /// получает данные с точек перед скелетом
    /// </summary>
    private void GetBoolFromEmpty()
    {
        FBbool = FB.GetComponent<EmptyCheck>().GetStateEmpty();
        Gbool = G.GetComponent<EmptyCheck>().GetStateEmpty();
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
    /// проверяет на видимость игрока в зоне
    /// </summary>
    private void Raycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(eyeRaycast.transform.position, (Player.transform.position -transform.position).normalized);
        Vector2 toplayer = Player.transform.position - transform.position;
        toplayer.Normalize();
        if (hit.collider.gameObject == Player) {
            playerFinded = true;
        }
    }
    #region State
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
    }
    private void GoForward()
    {
        if (rightMove) Rb.velocity = new Vector2(1 * speedGo, Rb.velocity.y);
        else Rb.velocity = new Vector2(-1 * speedGo, Rb.velocity.y);
    }
    #endregion
    #endregion

}
