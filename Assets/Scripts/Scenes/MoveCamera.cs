using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public GameObject _GOPlayer, _Camera;
    public float _deltaX = 2, _deltayUP = 10, _deltayDown = 10, t = 0.1f;
    public float offsety;
    void Start()
    {
        _GOPlayer = GameObject.Find("Player");
        _Camera.transform.position = new Vector3(_GOPlayer.transform.position.x, _GOPlayer.transform.position.y + offsety, -10);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float tm = Mathf.Abs(_GOPlayer.transform.position.y - _Camera.transform.position.y)/10;
        float ty;
        if (tm < 0.1f) ty = 0.1f;
        else ty = tm + 0.07f;
        if (_GOPlayer.transform.position.x - _deltaX > _Camera.transform.position.x)
        {
            _Camera.transform.position = Vector3.Lerp(this.transform.position, new Vector3(_GOPlayer.transform.position.x - _deltaX, _Camera.transform.position.y, -10), t);
        }
        if (_GOPlayer.transform.position.x + _deltaX < _Camera.transform.position.x)
        {
            _Camera.transform.position = Vector3.Lerp(this.transform.position, new Vector3(_GOPlayer.transform.position.x + _deltaX, _Camera.transform.position.y,-10), t);
        }
        if (_GOPlayer.transform.position.y - _deltayUP > _Camera.transform.position.y)
        {
            _Camera.transform.position = Vector3.Lerp(this.transform.position, new Vector3(_Camera.transform.position.x, _GOPlayer.transform.position.y - _deltayUP,-10), ty);
        }
        if (_GOPlayer.transform.position.y + _deltayDown < _Camera.transform.position.y)
        {
            _Camera.transform.position = Vector3.Lerp(this.transform.position, new Vector3(_Camera.transform.position.x, _GOPlayer.transform.position.y + _deltayDown,-10), ty); 
        }
    }
}
