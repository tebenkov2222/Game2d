using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PWomanApi : MonoBehaviour
{
    public int PlayerChange;
    private GameObject _GOPlayer;
    private PlayerController Controller;
    private Animator anim;
    public Vector2 VectorJumpVall
        , AtackCol = new Vector2(0.5f, 1f);
    public float forceDamage = 200,timeLoad = 2, timeRunner = 0.2f, timeAtackRes = 0.5f, radiusAtack = 2f, DamageRunner = 10f, Damage = 5f, _speedstandart = 20, _speedSitDown = 10, Force = 2, _maxJump = 1060;
    private float _speed, NormalGravityScale = 7, _speedRun = 1100,
        _timeRun = 0, timeUnderJump = 0, timeBlock = 0;
    [HideInInspector] public bool _Die = false, _JumpBool = false, _AtackBool = false, _LeftBool = false, _RightBool = false, _DownBool = false, _StopBlock = false, MoveActive = false, movel = false;
    public bool _DamageMobs = false, _JumpBlock = false, _RightSide = false, _LeftSide = false, _DownSide = false, _RightBlc = false, _LeftBlc = false, _Runner = false, _Vector = false, rightMove = true;[HideInInspector]
    void Start()
    {
        anim = this.GetComponent<Animator>();
        Controller = this.GetComponent<PlayerController>();
        _GOPlayer = this.gameObject;
        _speed = _speedstandart;
        _GOPlayer.GetComponent<Rigidbody2D>().freezeRotation = true;
    }
    void FixedUpdate()
    {

        if (_Die) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        MoveActive = checkMove();
        //если нажат прыжок
        if (_JumpBlock)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("JumpUp", false);
            anim.SetBool("JumpDown", false);
        }
        else
        {
            if (_GOPlayer.GetComponent<Rigidbody2D>().velocity.y < 4)
            {
                anim.SetBool("Run", false);
                anim.SetBool("Jump", true);
                anim.SetBool("JumpUp", false);
                anim.SetBool("JumpDown", true);
            }
        }
        if (_JumpBool)
        {
            // если снизу есть спрайт
            if (_DownSide)
            {
                if (Time.time - timeUnderJump > 0.5f)
                {
                    timeUnderJump = Time.time;
                    anim.SetBool("Run", false);
                    anim.SetBool("Jump", true);
                    anim.SetBool("JumpUp", true);
                    _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1) * _maxJump);
                }
            }
            else
            {
                // если справа или слева есть waypoint block 
                if ((_RightBlc || _LeftBlc))
                {
                    if ((Time.time - timeBlock > 0.5f))
                    {
                        timeBlock = Time.time;
                        // АНИМАЦИЯ блока
                        _GOPlayer.GetComponent<Rigidbody2D>().gravityScale = 0;
                        _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                        _StopBlock = true;
                    }
                }
                else
                {
                    _GOPlayer.GetComponent<Rigidbody2D>().gravityScale = NormalGravityScale;
                }
                if ((_LeftSide || _StopBlock) || (_RightSide || _StopBlock))
                {
                    _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, _GOPlayer.GetComponent<Rigidbody2D>().velocity.y);
                }
                // если справа блок и нажата левая кнопка
                if ((_RightSide || _StopBlock) && _LeftBool && (Time.time - timeUnderJump > 0.2f))
                {
                    
                    timeUnderJump = Time.time;
                    anim.SetBool("Run", false);
                    anim.SetBool("JumpDown", true);
                    movel = true;
                    _StopBlock = false;
                    _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(-VectorJumpVall.x, VectorJumpVall.y) * _maxJump);
                    rightMove = true;
                }
                // если слева блок и нажата правая кнопка 
                if ((_LeftSide || _StopBlock) && _RightBool && (Time.time - timeUnderJump > 0.2f))
                {
                    timeUnderJump = Time.time;
                    anim.SetBool("Run", false);
                    anim.SetBool("JumpDown", false);
                    movel = true;
                    _StopBlock = false;
                    _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(-VectorJumpVall.x, VectorJumpVall.y) * _maxJump);
                    rightMove = false;
                }
                
            }
        }
        else
        {
            _StopBlock = false;
            _GOPlayer.GetComponent<Rigidbody2D>().gravityScale = NormalGravityScale;
        }
        if ((_LeftBool || _RightBool)&& _DownSide && ! _Runner) anim.SetBool("Run", true);
        else anim.SetBool("Run", false);
        // если не бежит
        if (!_Runner && !Controller._AnimationAtack)
        {
            // если нажата левая кнопка и слева свободно
            if (_LeftBool && !_LeftSide)
            {
                movel = true;
                _Vector = true;
                _GOPlayer.GetComponent<SpriteRenderer>().flipX = true;
                Controller.LeftAtackRegion();
                _GOPlayer.transform.position += new Vector3(-0.01f * _speed, 0, 0);
                rightMove = false;
            }
            // если нажата правая кнопка и справа свободно
            if (_RightBool && !_RightSide)
            {
                movel = true;
                _Vector = false;
                _GOPlayer.GetComponent<SpriteRenderer>().flipX = false;
                Controller.RightAtackRegion();
                _GOPlayer.transform.position += new Vector3(0.01f * _speed, 0, 0);
                rightMove = true;
            }
        }
        // если снизу спрайт
        if (_DownSide) movel = false;
        // если плеер в воздухе и 
        if (!_DownSide && !_LeftBool && !_RightBool && !_RightSide && !_LeftSide && movel)
        {
            anim.SetBool("Run", false);
            movel = false;
            if (rightMove) _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector3(_speed * Time.deltaTime / 4, _GOPlayer.GetComponent<Rigidbody2D>().velocity.y, 0);
            else _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector3(-_speed * Time.deltaTime/ 4, _GOPlayer.GetComponent<Rigidbody2D>().velocity.y, 0);
        }

        if (_DownBool) // если в присяди
        {
            _speed = _speedSitDown;
        }
        else
        {
            _speed = _speedstandart;
        }
        if (_Runner) // если бег
        {
            Controller.TrailRunner(true);
            _GOPlayer.GetComponent<Rigidbody2D>().gravityScale = 0;
            _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            AtackVoidRunner();
            if (!_Vector) // вправо
            {
                _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f * Force, 0) * _speedRun);
            }
            else // влево
            {
                _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1f * Force, 0) * _speedRun);
            }
            if (Time.time - _timeRun > timeRunner) // если прошло больше, чем время рывка
            {

                _Runner = false;
                _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                _GOPlayer.GetComponent<Rigidbody2D>().isKinematic = true;
                _GOPlayer.GetComponent<Rigidbody2D>().isKinematic = false;
                _GOPlayer.GetComponent<Rigidbody2D>().gravityScale = NormalGravityScale;
            }
        }
        if ( Controller.TrailEnabled()) // если трейл включен
        {
            if (Time.time - _timeRun > timeRunner + 0.2f) // если прошло больше, чем нажатие на кнопку и время трейла
            {
                Controller.TrailRunner(false);
            }
        }
        if (_AtackBool) // если нажата кнопка атаковать
        {
            AtackAnimation();
        }
        if (_DamageMobs)
        {
            AtackVoid(true);
        }
        GetData();
    }
    void AtackAnimation()
    {
        Controller._AnimationAtack = true;
        int n = Random.Range(0, 3);
        if (n == 0) anim.SetBool("SwordAtack", true);
        if (n == 1) anim.SetBool("SwordAtackSitDown", true);
        if (n == 2) anim.SetBool("SwordAtackForward", true);
    }
    /// <summary>
    /// провека на передвижение плеера
    /// </summary>
    /// <returns></returns>
    private bool checkMove()
    {
        if (_LeftBool || _RightBool)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #region UI
    void GetData()
    {
        List<bool> Databool = Controller.GetAllDatabool();
        _AtackBool = Databool[0];
        _JumpBool = Databool[1];
        _LeftBool = Databool[2];
        _RightBool = Databool[3];
        if (Databool[4])
        {
            _timeRun = Time.time;
            _Runner = true;
        }
        _DownBool = Databool[5];
        if (Databool[6])
        {
            List<float> Datafloat = Controller.GetAllDatafloat();
            float Health = Datafloat[0];
            if (Health <= 0) _Die = true;
        }//анимация смерти
        _DownSide = Databool[7];
        _RightSide = Databool[8];
        _LeftSide = Databool[9];
        _RightBlc = Databool[10];
        _LeftBlc = Databool[11];
        _JumpBlock = Databool[12];
        _DamageMobs = Databool[13];


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
                if (ColAttack[i].gameObject.tag == "Sceleton") if (ColAttack[i].gameObject.GetComponent<SceletonAPI>()) ColAttack[i].gameObject.GetComponent<SceletonAPI>().GetDamage(DamageRunner);
                if (ColAttack[i].gameObject.tag == "Strazh") if (ColAttack[i].gameObject.GetComponent<MagScript>()) ColAttack[i].gameObject.GetComponent<MagScript>().GetDamage(DamageRunner);
                
            }
        }
    }
    private void AtackVoid(bool Force)
    {
        _AtackBool = false;
        List<Collider2D> ColAttack = Controller.CheckAtackRegion();
        for (int i = 0; i < ColAttack.Count; ++i)
        {
            if (ColAttack[i].GetComponent<TeleportRoomScript>()) ColAttack[i].GetComponent<TeleportRoomScript>().Teleported();
            if (ColAttack[i].GetComponent<Fire>()) Destroy(ColAttack[i].gameObject);
            if (ColAttack[i].gameObject.layer == 11 || ColAttack[i].gameObject.layer == 12)
            {
                if (ColAttack[i].gameObject.tag == "Sceleton")if (ColAttack[i].gameObject.GetComponent<SceletonAPI>()) ColAttack[i].gameObject.GetComponent<SceletonAPI>().GetDamage(Damage);
                if (ColAttack[i].gameObject.tag == "Strazh") if (ColAttack[i].gameObject.GetComponent<MagScript>()) ColAttack[i].gameObject.GetComponent<MagScript>().GetDamage(Damage);
                if(Force) ColAttack[i].gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(ColAttack[i].gameObject.transform.position.x - this.transform.position.x, 1f) * forceDamage);
            }
        }
    }
    /// <summary>
    /// загрузка спрайта
    /// </summary>
    // перезагрузка левэла
}
