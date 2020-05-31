using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;
#region Interfaces
interface Atack
{
    Vector2 StartPosition { get; set; }
    void Atack();
}
class ArrowAtack : Atack
{
    public Vector2 StartPosition { get; set; }
    public void Atack()
    {

    }
}
class ArrowAtackSit : Atack
{
    public Vector2 StartPosition { get; set; }
    public void Atack()
    {

    }
}
#endregion
public class MovePlayer : MonoBehaviour
{
    public SPlayers[] ScrObj;
    #region Player
    private GameObject _GOPlayer;
    private PlayerController Controller;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    #endregion
    [SerializeField] private GameObject Arrow, SpawnerArrow;
    [SerializeField] private LayerMask layerMask;
    [SerializeField]
    private float
        velocityJumpDown = 8f;
    public Vector2
        VectorJumpVall
        , AtackCol = new Vector2(0.5f, 1f);
    public float
        ForceArrow = 10,
        forceDamage = 200,
        timeLoad = 2,
        timeRunner = 0.2f,
        timeAtackRes = 0.5f,
        radiusAtack = 2f,
        DamageRunner = 10f,
        Damage = 5f,
        Damage_B = 5f,
        _speedstandart = 20,
        _speedSitDown = 10,
        _speedElevatorUp = 4,
        _speedElevatorDown = 8,
        Force = 2,
        _maxJump = 1060,
        atackradius;
    private float
        NormalGravityScale = 7,
        _speedRun = 1100,
        _timeRun = 0,
        timeUnderJump = 0,
        timeBlock = 0,
        _timeLastMove = 0;
    private int
        _Runnerbool = 0;
    [HideInInspector]
    public bool
    #region UI Buttons
        _AtackBool = false,
        _JumpBool = false,
        _LeftBool = false,
        _RightBool = false,
        _DownBool = false,
        _UpBool = false,
        _SitBool = false,
    #endregion
    #region Sides
        _DownSide = false,
        _RightSide = false,
        _LeftSide = false,
        _RightBlc = false,
        _LeftBlc = false,
        _JumpSide = false,
        _StopBlock = false,
        _elevator = false,
    #endregion
    #region States
        _ArrowSpawn = false,
        _AtackNow = false,
        _Life = true,
        _SwordIdle = false,
        _JumpState = false,
    _elevatorState = false;
    #endregion
    private string TagWeapons;
    void Awake()
    {
        #region Player
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        Controller = this.GetComponent<PlayerController>();
        _GOPlayer = this.gameObject;
        rb.freezeRotation = true;
        sprite = this.GetComponent<SpriteRenderer>();
        #endregion
        TagWeapons = Controller.GetTagWeapons();
        GetDataScrObj();
    }
    void GetDataScrObj() 
    {
        _speedstandart = ScrObj[PlayerPrefs.GetInt("WomanUpgrade")].GetSpeed();
        _speedSitDown = _speedstandart - 3;
        Damage = ScrObj[PlayerPrefs.GetInt("WomanUpgrade")].GetDamage_A();
        Controller.ArrowMax = ScrObj[PlayerPrefs.GetInt("WomanUpgrade")].GetArrows();
        Controller.HealthMax = ScrObj[PlayerPrefs.GetInt("WomanUpgrade")].GetHealth();
        Damage_B = ScrObj[PlayerPrefs.GetInt("WomanUpgrade")].GetDamage_B();
    }

    private void Start()
    {
        _timeLastMove = Time.time;
    }
    void FixedUpdate()
    {
        GetData();
        if (_Life)
        {
            CheckAllArrow();
            Move();
            MoveJumped();
            Jump();
            Atack();
            CheckIdle();
            StopBlock();
            JumpVall();
            Runner();
            Atack();
            CheckVerticleJoystik();
        }
        else {
            DontDestroyOnLoad(this.gameObject);
            if (PlayerPrefs.GetInt("Death") == 1)
            {
                Controller.Health = Controller.HealthMax;
                SceneManager.LoadScene("Finish");
            }
            else if (PlayerPrefs.GetInt("Death") == 0) PlayerPrefs.SetInt("Death", -1);
        }
    }
    #region Main
    private GameObject vr;
    private void Atack()
    {
        if (_AtackBool)
        {
            _AtackBool = false;
            if (TagWeapons == "Sword_Woman")
            {
                AtackAnimationSword();
            }
            else if (TagWeapons == "Arrow_Woman")
            {
                if (Controller.ArrowNow != 0) AtackAnimationArrow();
            }
        }
        if (_ArrowSpawn)
        {
            _ArrowSpawn = false;
            vr = Instantiate(Arrow, SpawnerArrow.transform.position, Quaternion   .identity);
            vr.GetComponent<Arrow>().Damage = Damage_B;
            if (this.GetComponent<SpriteRenderer>().flipX) vr.transform.Rotate(new Vector3(0, 180, 0));

        }
        if (_AtackNow)
        {
            _AtackNow = false;
            if (TagWeapons == "Sword_Woman") AtackVoid();
            else if (TagWeapons == "Arrow_Woman")
            {
                if (this.GetComponent<SpriteRenderer>().flipX) vr.GetComponent<Rigidbody2D>().AddForce(Vector2.left * ForceArrow);
                else vr.GetComponent<Rigidbody2D>().AddForce(Vector2.right * ForceArrow);
            }
        }
    }
   private void CheckVerticleJoystik()
    {
        if (_elevatorState)
        {
            Controller.Elevate();
        }
        if (_DownSide)
        {
            _elevatorState = false;
        }
        if (_elevator)
        {
            if (!_UpBool && !_DownBool && _elevatorState)
            {
                StateElevate = AnimElevate.Idle;
                rb.velocity = new Vector2(0, 0);
            }
            if (_UpBool)
            {
                Physics2D.IgnoreLayerCollision(10, 15, true);
                State = AnimState.Elevate;
                StateElevate = AnimElevate.Up;
                _elevatorState = true;
                rb.gravityScale = 0;
                rb.velocity = new Vector2(0, _speedElevatorUp);
            }
            if (_DownBool && !_DownSide)
            {
                _elevatorState = true;
                State = AnimState.Elevate;
                StateElevate = AnimElevate.Down;
                rb.velocity = new Vector2(0, -_speedElevatorDown);
                Physics2D.IgnoreLayerCollision(10, 15, true);
            }
            if (!_DownSide && _elevatorState)
            {
                rb.gravityScale = 0;
            }
            if (_LeftBool || _RightBool && _elevatorState)
            {
                State = AnimState.Idle;
                _elevatorState = false;
            }
        }
        if (_DownBool)
        {
            Physics2D.IgnoreLayerCollision(10, 15, true);
        }
    }
    private void CheckAllArrow()
    {
        List<Collider2D> ColAttack = Controller.CheckAtackRegion();
        for (int i = 0; i < ColAttack.Count; ++i)
        {
            if (ColAttack[i].GetComponent<Arrow>())
            {
                print(ColAttack[i].GetComponent<Arrow>().Active);
                if (!ColAttack[i].GetComponent<Arrow>().Active)
                {
                    Destroy(ColAttack[i].gameObject);
                    Controller.ArrowNow++;
                }
            }

        }
    }
    private void Runner()
    {
        if (_Runnerbool!= 0)
        {
            State = AnimState.Runner;
            Controller.TrailRunner(true);
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0, 0);
            AtackVoidRunner();
            if (!sprite.flipX) // вправо
            {
                _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f * Force, 0) * _speedRun);
            }
            else // влево
            {
                _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1f * Force, 0) * _speedRun);
            }
            if (Time.time - _timeRun > timeRunner) // если прошло больше, чем время рывка
            {
                State = AnimState.Idle;
                _Runnerbool = 0;
                rb.velocity = new Vector2(0, 0);
                rb.isKinematic = true;
                rb.isKinematic = false;
                rb.gravityScale = NormalGravityScale;
                Controller.TrailRunner(false);
            }
        }
    }
    private void Move()
    {
        if ( _DownSide && 
            _Runnerbool == 0 && 
            State != AnimState.AtackHand &&
            State != AnimState.AtackBow &&
            State != AnimState.AtackBowJumped &&
            State != AnimState.AtackBowSit &&
            State != AnimState.AtackSword &&
            State != AnimState.AtackSwordForward &&
            State != AnimState.AtackSwordSit)
        {
            if (!_RightSide && _RightBool)
            {
                State = AnimState.Run;
                MoveForward(Speed(), true);
            }
            if (!_LeftSide && _LeftBool)
            {
                State = AnimState.Run;
                MoveForward(Speed(), false);
            }
        }
    }
    private void CheckMovePlayer()
    {
        if (_AtackBool || _JumpBool || _LeftBool || _RightBool || _DownBool || _UpBool) _timeLastMove = Time.time;
    }
    private void MoveJumped()
    {
        if (!_DownSide && _Runnerbool == 0)
        {
            if (!_RightSide && _RightBool)
            {
                MoveForward(Speed(), true);
            }
            if (!_LeftSide && _LeftBool)
            {
                MoveForward(Speed(), false);
            }
        }
    }
    private void CheckIdle()
    {
        CheckMovePlayer();
        if (!_elevatorState)
        {
            if (
                _DownSide &&
                !_RightBool &&
                !_LeftBool &&
                State != AnimState.AtackBow &&
                State != AnimState.AtackBowJumped &&
                State != AnimState.AtackBowSit &&
                State != AnimState.AtackHand &&
                State != AnimState.AtackSword &&
                State != AnimState.AtackSwordForward &&
                State != AnimState.AtackSwordSit
                )
            {
                if (Time.time - _timeLastMove < 10) State = AnimState.Idle;
                else
                {
                    _SwordIdle = true;
                    State = AnimState.SwordIdle;
                }
            }
        }
    }
    private void Jump()
    {
        CheckStateJump();
        if (!_DownSide && !_RightBlc && !_LeftBlc && !_elevatorState)
        {
            State = AnimState.Jump;
        }
        if (_JumpSide && rb.velocity.y < velocityJumpDown && !_DownSide && !_elevatorState)
        {
            _JumpState = false;
            State = AnimState.Idle;
        }
        if (_JumpState && rb.velocity.y <= 0) _JumpState = false;
        if (!_SwordIdle && !_elevatorState && _JumpBool )
        {
            if (_DownSide)
            {
                State = AnimState.Jump;
                if (Time.time - timeUnderJump > 0.5f)
                {
                    _JumpState = true;
                    Physics2D.IgnoreLayerCollision(10, 15, true);
                    timeUnderJump = Time.time;
                    _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1) * _maxJump);
                }
            }
        }
    }
    private void StopBlock()
    {
        if (_JumpBool)
        {
            if ((_RightBlc || _LeftBlc))
            {
                if ((Time.time - timeBlock > 0.5f))
                {
                    timeBlock = Time.time;
                    State = AnimState.PlayerBlock;
                    rb.gravityScale = 0;
                    rb.velocity = new Vector2(0, 0);
                    _StopBlock = true;
                }
            }
            else rb.gravityScale = NormalGravityScale;
        }
        else
        {
            rb.gravityScale = NormalGravityScale;
        }
    }
    private void JumpVall()
    {
        if (_JumpBool && (_RightSide || _RightBlc || _LeftBlc) && _LeftBool && (Time.time - timeUnderJump > 0.2f))
        {
            timeUnderJump = Time.time;
            State = AnimState.Jump;
            StateJump = AnimJump.JumpUp;
            rb.velocity = new Vector2(0, 0);
            rb.isKinematic = true;
            rb.isKinematic = false;
            rb.AddForce(new Vector2(-VectorJumpVall.x, VectorJumpVall.y) * _maxJump);
        }
        if (_JumpBool && (_LeftSide || _LeftBlc || _RightBlc) && _RightBool && (Time.time - timeUnderJump > 0.2f))
        {
            timeUnderJump = Time.time;
            State = AnimState.Jump;
            StateJump = AnimJump.JumpUp;
            rb.isKinematic = true;
            rb.isKinematic = false;
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(VectorJumpVall.x, VectorJumpVall.y) * _maxJump);
        }
    }
    float Speed()
    {
        if (_SitBool) return _speedSitDown;
        else return _speedstandart;
    }

    #endregion



    #region UI
    void AtackAnimationArrow()
    {
        State = AnimState.AtackBowSit;
    }
    void AtackAnimationSword()
    {
        int n = Random.Range(0, 3);
        if (n == 0) State = AnimState.AtackSword;
        if (n == 1) State = AnimState.AtackSwordSit;
        if (n == 2) State = AnimState.AtackSwordForward;
    }
    private void CheckStateJump()
    {
        if (rb.velocity.y < velocityJumpDown) StateJump = AnimJump.JumpDown;
        else StateJump = AnimJump.JumpUp;
    }
    #region Enum
    public AnimState State
    {
        get { return (AnimState)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }
    private AnimElevate StateElevate
    {
        get { return (AnimElevate)anim.GetInteger("StateElevate"); }
        set { anim.SetInteger("StateElevate", (int)value); }
    }
    private AnimJump StateJump
    {
        get { return (AnimJump)anim.GetInteger("StateJump"); }
        set { anim.SetInteger("StateJump", (int)value); }
    }
    public enum AnimJump
    {
        JumpDown,
        JumpUp
    }
    public enum AnimElevate
    {
        Up,
        Idle,
        Down
    }
    public enum AnimState
    {
        Idle,
        SwordIdle,
        AtackBow,
        AtackBowSit,
        AtackBowJumped,
        AtackSword,
        AtackSwordSit,
        AtackSwordForward,
        AtackHand,
        Jump,
        Damage,
        TeleportStart,
        TeleportIdle,
        TeleportEnd,
        Run,
        Runner,
        PlayerBlock,
        Elevate
    }
    #endregion

    void GetData()
    {
        TagWeapons = Controller.GetTagWeapons();
        List<bool> Databool = Controller.GetAllDatabool();
        _AtackBool = Databool[0];
        _JumpBool = Databool[1];
        _LeftBool = Databool[2];
        _RightBool = Databool[3];
        _DownBool = Databool[4];
        _UpBool = Databool[5];
        if (Databool[6]) // Run 
        {
            _timeRun = Time.time;
            _Runnerbool = 1;
        }
        _SitBool = Databool[7];
        if (Databool[8]) // Damage
        {
            State = AnimState.Damage;
        }//анимация смерти
        _DownSide = Databool[9];
        _RightSide = Databool[10];
        _LeftSide = Databool[11];
        _RightBlc = Databool[12];
        _LeftBlc = Databool[13];
        _JumpSide = Databool[14];
        if (Databool[15]) // Atack
        {
            _AtackNow = true;
        }
        _Life = Databool[16];
        _elevator = Databool[17];
    }
    #endregion
    //атаковать
    private void AtackVoidRunner()
    {
        _AtackBool = false;
        List<Collider2D> ColAttack = Controller.CheckRunerAtackRegion();
        for (int i = 0; i < ColAttack.Count; ++i)
        {
            if (ColAttack[i].gameObject.layer == 11 || ColAttack[i].gameObject.layer == 12)
            {
                if (ColAttack[i].gameObject.GetComponent<MobsController>()) ColAttack[i].gameObject.GetComponent<MobsController>().GetDamage(DamageRunner);
            }
        }
    }
    private void AtackVoid()
    {
        List<Collider2D> ColAttack = Controller.CheckAtackRegion();
        for (int i = 0; i < ColAttack.Count; ++i)
        {
            RaycastHit2D hit1 = Physics2D.Raycast(this.transform.position, ColAttack[i].transform.position - this.transform.position, atackradius, layerMask);
            if (hit1.collider != null)
            {
                if (hit1.collider.gameObject == ColAttack[i].gameObject)
                {
                    if (ColAttack[i].GetComponent<TeleportRoomScript>()) ColAttack[i].GetComponent<TeleportRoomScript>().Teleported();
                    if (ColAttack[i].GetComponent<Fire>()) ColAttack[i].GetComponent<Fire>().Destroy();
                    if (ColAttack[i].gameObject.layer == 11 || ColAttack[i].gameObject.layer == 12)
                    {
                        if (ColAttack[i].gameObject.GetComponent<MobsController>()) ColAttack[i].gameObject.GetComponent<MobsController>().GetDamage(Damage);
                        ColAttack[i].gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(ColAttack[i].gameObject.transform.position.x - this.transform.position.x, 1f) * forceDamage);
                    }
                }
            }
        }

    }
    private void MoveForward(float speed, bool rightMove)
    {
        if (!_SwordIdle)
        {
            if (rightMove)
            {
                Controller.RightAtackRegion();
                sprite.flipX = false;
                _GOPlayer.transform.position += new Vector3(1 * speed * Time.deltaTime, 0, 0);
            }
            else
            {
                Controller.LeftAtackRegion();
                sprite.flipX = true;
                _GOPlayer.transform.position += new Vector3(-1 * speed * Time.deltaTime, 0, 0);
            }
        }
    }
    // перезагрузка левэла
}
