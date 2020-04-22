using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MovePlayer : MonoBehaviour
{
    public int PlayerChange;
    public Text text;
    private GameObject _GOPlayer;
    //private ScriptableObject anim;
    public Vector2 _posPlayer, AtackCol = new Vector2(0.5f, 1f);
    public float Health = 5, timeLoad = 2, timeRunner = 0.2f,  timeAtackRes = 0.5f, radiusAtack = 2f, Damage = 5f, _speedstandart = 20, _speedSitDown = 10, Force = 2, _maxJump = 1060;
    private float  _speed, NormalGravityScale = 7, _speedRun = 1100,
        _timeRun = 0, procent = 1000;
    [HideInInspector] public bool _JumpBool = false, _AtackBool = false, _LeftBool = false, _RightBool = false, _DownBool = false, _StopBlock = false, MoveActive = false, movel = false;
    public bool _RightSide = false, _LeftSide = false, _DownSide = false, _RightBlc = false, _LeftBlc = false, _Runner = false, _Vector = false, rightMove = true; [HideInInspector]
    void Start()
    {
        if (PlayerChange == 0)
        {

        }
        else
        {
            if (PlayerChange == 1)
            {
                //anim = this.GetComponent<PlayerAnimationWoman>();
            }
        }
        _GOPlayer = this.gameObject;
        _speed = _speedstandart;
        _posPlayer = _GOPlayer.transform.position;
        _GOPlayer.GetComponent<Rigidbody2D>().freezeRotation = true;
    }
    void FixedUpdate()
    {
        if (Health <=0 ) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        setText();
        MoveActive = checkMove();
        _posPlayer = _GOPlayer.transform.position;
        //если нажат прыжок
        if (_JumpBool)
        {
            // если снизу есть спрайт
            if (_DownSide)
            {
                _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1) * _maxJump);
            }
            else
            {
                // если справа или слева есть waypoint block 
                if (_RightBlc || _LeftBlc)
                {
                    _GOPlayer.GetComponent<Rigidbody2D>().gravityScale = 0;
                    _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    _StopBlock = true;
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
                if ((_RightSide || _StopBlock )&& _LeftBool)
                {
                    movel = true;
                    _StopBlock = false;
                    _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.1f, 1f) * _maxJump);
                    rightMove = true;
                }
                // если слева блок и нажата правая кнопка 
                if ((_LeftSide || _StopBlock) && _RightBool)
                {
                    movel = true;
                    _StopBlock = false;
                    _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.1f, 1f) * _maxJump);
                    rightMove = false;
                }
            }
        }
        else
        {
            _StopBlock = false;
            _GOPlayer.GetComponent<Rigidbody2D>().gravityScale = NormalGravityScale;
        }
        // если не бежит
        if (!_Runner) 
        {
            // если нажата левая кнопка и слева свободно
            if (_LeftBool && !_LeftSide)
            {
                movel = true;
                _Vector = true;
                _GOPlayer.GetComponent<SpriteRenderer>().flipX = true;
                this.gameObject.GetComponentInChildren<AtackAnimation>().gameObject.transform.localPosition = new Vector2(-2, -0.5f);
                this.gameObject.GetComponentInChildren<AtackAnimation>().gameObject.GetComponent<SpriteRenderer>().flipX = true;
                _GOPlayer.transform.position += new Vector3(-0.01f * _speed, 0, 0);
                rightMove = false;
            }
            // если нажата правая кнопка и справа свободно
            if (_RightBool && !_RightSide)
            {
                movel = true;
                _Vector = false;
                _GOPlayer.GetComponent<SpriteRenderer>().flipX = false;
                this.gameObject.GetComponentInChildren<AtackAnimation>().gameObject.transform.localPosition = new Vector2(2, -0.5f);
                this.gameObject.GetComponentInChildren<AtackAnimation>().gameObject.GetComponent<SpriteRenderer>().flipX = false;
                _GOPlayer.transform.position += new Vector3(0.01f * _speed, 0, 0);
                rightMove = true;
            }
        }
        // если снизу спрайт
        if (_DownSide) movel = false;
        // если плеер в воздухе и 
        if (!_DownSide && !_LeftBool && !_RightBool && !_RightSide && !_LeftSide && movel)
        {
            movel = false;
            if (rightMove) _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector3(_speed/ 4, _GOPlayer.GetComponent<Rigidbody2D>().velocity.y, 0);
            else _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector3(-_speed / 4, _GOPlayer.GetComponent<Rigidbody2D>().velocity.y, 0);
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
            this.gameObject.GetComponent<TrailRenderer>().enabled = true;
            _GOPlayer.GetComponent<Rigidbody2D>().gravityScale = 0;
            _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            AtackVoid(this.gameObject, new Vector2(0.1f,0.1f));
            if (!_Vector) // вправо
            {
                _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f * Force,0) * _speedRun);
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
        if (this.gameObject.GetComponent<TrailRenderer>().enabled) // если трейл включен
        {
            if (Time.time - _timeRun > timeRunner+0.2f) // если прошло больше, чем нажатие на кнопку и время трейла
            {
                this.gameObject.GetComponent<TrailRenderer>().enabled = false;
            }
        }
        if (_AtackBool) // если нажата кнопка атаковать
        {
            AtackVoid(this.GetComponentInChildren<AtackAnimation>().gameObject, AtackCol);
            this.gameObject.GetComponentInChildren<AtackAnimation>().Atack();
        }

    }

    /// <summary>
    /// провека на передвижение плеера
    /// </summary>
    /// <returns></returns>
    private bool checkMove()
    {
        if (_LeftBool || _RightBool) return true;
        else return false;
    }
    #region UI
    public void Atack()
    {
        _AtackBool = !_AtackBool;
    }
    public void Jump()
    {
        _JumpBool = !_JumpBool;
    }
    public void MoveLeft()
    {
        _LeftBool = !_LeftBool;
    }
    public void MoveRight()
    {

        _RightBool = !_RightBool;
    }
    public void Run()
    {
        if (procent == 1000)
        {
            _timeRun = Time.time;
            _Runner = true;
            procent = 0;
        }
    }
    public void MoveSitDown()
    {
        _DownBool = !_DownBool;
    }
    #endregion
    //атаковать
    private void AtackVoid(GameObject Center, Vector2 Scale )
    {
        _AtackBool = false;
        List<Collider2D> ColAttack = new List<Collider2D>(Physics2D.OverlapBoxAll(Center.transform.position, Scale , 0));
        for (int i = 0; i < ColAttack.Count; ++i)
        {
            if (ColAttack[i].GetComponent<TeleportRoomScript>()) ColAttack[i].GetComponent<TeleportRoomScript>().Teleported();
            if (ColAttack[i].GetComponent<Fire>()) Destroy(ColAttack[i].gameObject);
            if (ColAttack[i].gameObject.layer == 11 || ColAttack[i].gameObject.layer == 12)
            {
                if (ColAttack[i].gameObject.tag == "Sceleton") ColAttack[i].gameObject.GetComponent<SceletonAPI>().GetDamage(Damage);
                if (ColAttack[i].gameObject.tag == "Strazh") ColAttack[i].gameObject.GetComponent<MagScript>().GetDamage(Damage);
                ColAttack[i].gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(ColAttack[i].gameObject.transform.position.x - this.transform.position.x, 0) * 200);
            }
        }
    }
    private void setText()
    {
        text.text = Health.ToString();
    }
    public void GetDamage(float damage)
    {
        if (Random.Range(0,50) == 1)
        {
            Health -= damage * 0.3f;
        }
        else
        {
            Health -= damage;
        }
    }
    /// <summary>
    /// загрузка спрайта
    /// </summary>
    // перезагрузка левэла
}
