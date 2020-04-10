using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public GameObject _GOPlayer, _Camera;
    public float _deltaX = 2, _deltayUP = 10, _deltayDown = 10;
    void Start()
    {
        _GOPlayer = GameObject.Find("Player");
        _Camera.transform.position = new Vector3(_GOPlayer.transform.position.x, _GOPlayer.transform.position.y, -10);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_GOPlayer.transform.position.x - _deltaX > _Camera.transform.position.x)
        {
            _Camera.transform.position = new Vector3(_GOPlayer.transform.position.x - _deltaX, _Camera.transform.position.y, -10);
        }
        if (_GOPlayer.transform.position.x + _deltaX < _Camera.transform.position.x)
        {
            _Camera.transform.position = new Vector3(_GOPlayer.transform.position.x + _deltaX, _Camera.transform.position.y, -10);
        }
        if (_GOPlayer.transform.position.y - _deltayUP > _Camera.transform.position.y)
        {
            _Camera.transform.position = new Vector3(_Camera.transform.position.x, _GOPlayer.transform.position.y - _deltayUP, -10);
        }
        if (_GOPlayer.transform.position.y + _deltayDown < _Camera.transform.position.y)
        {
            _Camera.transform.position = new Vector3(_Camera.transform.position.x, _GOPlayer.transform.position.y + _deltayDown, -10);
        }
    }
}
