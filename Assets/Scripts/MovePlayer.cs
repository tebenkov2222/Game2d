using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public GameObject _GOPlayer;
    public Vector3 _posPlayer;
    private float _maxJump = 1060,
        _speed, _speedstandart = 20, _speedSitDown = 10, NormalGravityScale = 7, _speedRun = 1100,
        _timeRun = 0;
    bool _JumpBool = false, _LeftBool = false, _RightBool = false, _DownBool = false, _StopBlock = false;
    public bool _RightSide = false, _LeftSide = false, _DownSide = false, _RightBlc = false, _LeftBlc = false, _Runner = false, _Vector = false; [HideInInspector]
    void Start()
    {
        _speed = _speedstandart;
        _posPlayer = _GOPlayer.transform.position;
        _GOPlayer.GetComponent<Rigidbody2D>().freezeRotation = true;
    }
    void FixedUpdate()
    {
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
                    _StopBlock = false;
                    _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.1f, 1f) * _maxJump);
                }
                if ((_LeftSide || _StopBlock) && _RightBool)
                {
                    _StopBlock = false;
                    _GOPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    _GOPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.1f, 1f) * _maxJump);
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
            _Vector = true;
            _GOPlayer.GetComponent<SpriteRenderer>().flipX = true;
            _GOPlayer.transform.position += new Vector3(-0.01f * _speed, 0, 0);
        }
        if (_RightBool && !_RightSide)
        {
            _Vector = false;
            _GOPlayer.GetComponent<SpriteRenderer>().flipX = false;
            _GOPlayer.transform.position += new Vector3(0.01f * _speed, 0, 0);
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
        _timeRun = Time.time ;
        _Runner = true;
    }
    public void MoveSitDown()
    {
        _DownBool = !_DownBool;
    }
}
