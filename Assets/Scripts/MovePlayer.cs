using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public GameObject _GOPlayer;
    public Vector3 _posPlayer;
    public float timeLoad = 2, timeAtackRes = 0.5f, radiusAtack = 2f, Damage = 5f, _speedstandart = 20, _speedSitDown = 10;
    private float _maxJump = 1060,
        _speed, NormalGravityScale = 7, _speedRun = 1100,
        _timeRun = 0, procent = 1000;
    [HideInInspector] public bool _JumpBool = false, _AtackBool = false, _LeftBool = false, _RightBool = false, _DownBool = false, _StopBlock = false, MoveActive = false, movel = false;
    public bool _RightSide = false, _LeftSide = false, _DownSide = false, _RightBlc = false, _LeftBlc = false, _Runner = false, _Vector = false, rightMove = true; [HideInInspector]
    void Start()
    {
        _speed = _speedstandart;
        _posPlayer = _GOPlayer.transform.position;
        _GOPlayer.GetComponent<Rigidbody2D>().freezeRotation = true;
    }
    void FixedUpdate()
    {
        
        LoadRun();
        MoveActive = checkMove();
        _posPlayer = _GOPlayer.transform.position;
        if (_JumpBool)
        {
            if (_DownSide)
            {
                _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1) * _maxJump);
            }
            else
            {
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
                if ((_RightSide || _StopBlock )&& _LeftBool)
                {
                    movel = true;
                    _StopBlock = false;
                    _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.1f, 1f) * _maxJump);
                    rightMove = true;
                }
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
        if (_LeftBool && !_LeftSide)
        {
            movel = true;
            _Vector = true;
            _GOPlayer.GetComponent<SpriteRenderer>().flipX = true;
            //_GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector3(-_speed, _GOPlayer.GetComponent<Rigidbody2D>().velocity.y, 0);
            _GOPlayer.transform.position += new Vector3(-0.01f * _speed, 0, 0);
            rightMove = false;
        }
        if (_RightBool && !_RightSide)
        {
            movel = true;
            _Vector = false;
            _GOPlayer.GetComponent<SpriteRenderer>().flipX = false;
            //_GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector3(_speed, _GOPlayer.GetComponent<Rigidbody2D>().velocity.y, 0);
            _GOPlayer.transform.position += new Vector3(0.01f * _speed, 0, 0);
            rightMove = true;
        }
        if (_DownSide) movel = false;
        if (!_DownSide && !_LeftBool && !_RightBool && !_RightSide && !_LeftSide && movel)
        {
            movel = false;
            if (rightMove) _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector3(_speed/ 4, _GOPlayer.GetComponent<Rigidbody2D>().velocity.y, 0);
            else _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector3(-_speed / 4, _GOPlayer.GetComponent<Rigidbody2D>().velocity.y, 0);
        }
        if (_DownBool)
        {
            _speed = _speedSitDown;
        }
        else
        {
            _speed = _speedstandart;
        }
        if (_Runner)
        {
            _GOPlayer.GetComponent<Rigidbody2D>().gravityScale = 0;
            _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            if (!_Vector)
            {
                _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f,0) * _speedRun);
            }
            else
            {
                _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1f, 0) * _speedRun);
            }
            if (Time.time - _timeRun > 0.2f)
            {
                _Runner = false;
                _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                _GOPlayer.GetComponent<Rigidbody2D>().gravityScale = NormalGravityScale;
            }
        }
        if (_AtackBool)
        {
            _AtackBool = false;
            List<Collider2D> ColAttack = new List<Collider2D>(Physics2D.OverlapCircleAll(this.gameObject.transform.position, radiusAtack));
            for(int i = 0; i < ColAttack.Count; ++i)
            {
                if (ColAttack[i].gameObject.layer == 11 || ColAttack[i].gameObject.layer == 12)
                {
                    ColAttack[i].gameObject.GetComponent<SceletonAPI>().Health -= Damage;
                }
            }
        }

    }
    
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

    public void LoadRun()
    {
        if (procent != 1000)
        {
            procent += 1000 / (timeLoad * 50);
            this.gameObject.GetComponent<LoadSprite>().SetLoad(procent);
        }
        else this.gameObject.GetComponent<LoadSprite>().SetLoad(0);
    }
}
