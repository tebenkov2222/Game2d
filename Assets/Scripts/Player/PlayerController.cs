using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
interface IAnimation
{

}
public class PlayerController : MonoBehaviour
{
    public VariableJoystick variableJoystick;
    public LayerMask lm;
    public GameObject AtackRegion, AtackRegionRunner;
    public Image Load;
    public Text text;
    [HideInInspector] 
    public bool 
        ActivePlayer = true,
        _DownSide = false,
        _RightSide = false,
        _LeftSide = false,
        _RightBlc = false,
        _LeftBlc = false;
    [HideInInspector]
    public bool 
        _DamageMob = false,
        _AtackBool = false, 
        _JumpBool = false, 
        _LeftBool = false, 
        _RightBool = false,
        _Runner = false,
        _DownBool = false,
        _Damage = false,
        MoveActive = false,
        _JumpBlock = false,
        _AnimationAtack = false,
        _RunnerAtack = false;
    private Vector2 TransformPositionAtackRegionStart;
    public float Health = 50;
    float procent = 1000,
        timeLoad = 2;
    private void Start()
    {
        TransformPositionAtackRegionStart = AtackRegion.transform.localPosition;
    }
    public bool TrailEnabled()
    {
        if (AtackRegionRunner.GetComponent<TrailRenderer>().enabled) return true;
        return false;
    }
    public void TrailRunner(bool Set)
    {
        AtackRegionRunner.GetComponent<TrailRenderer>().enabled = Set;
    }
    public List<Collider2D> CheckRunerAtackRegion() { return new List<Collider2D>(Physics2D.OverlapPointAll(AtackRegionRunner.transform.position, lm)); }
    public List<Collider2D> CheckAtackRegion(){return new List<Collider2D>(Physics2D.OverlapBoxAll(AtackRegion.transform.position, AtackRegion.transform.localScale, 0, lm));}
    private bool checkMove()
    {
        if ((_LeftBool || _RightBool) && !_DownBool) return true;
        else return false;
    }
    public void LeftAtackRegion()
    {
            AtackRegion.transform.localPosition = new Vector2(-1 * TransformPositionAtackRegionStart.x, TransformPositionAtackRegionStart.y);
    }
    public void RightAtackRegion()
    {
        AtackRegion.transform.localPosition = new Vector2(TransformPositionAtackRegionStart.x, TransformPositionAtackRegionStart.y);
    }


    private void Update()
    {
        MoveActive = checkMove();
        LoadRun();
        setText();
    }
    #region UI
    public void Atack()
    {
        _AtackBool = true;
    }
    public void Jump()
    {
        _JumpBool = !_JumpBool;
    }
    public void Left()
    {
        _LeftBool = !_LeftBool;
    }
    public void Right()
    {
        _RightBool = !_RightBool;
    }
    public void Run()
    {
        if (procent == 1000)
        {
            _Runner = true;
            procent = 0;
        }
    }
    public void SitDown()
    {
        _DownBool = !_DownBool;
    }

    public bool SetRun()
    {
        if (_Runner)
        {
            _Runner = false;
            return true;
        }
        else return false;

    }
    public bool SetAtack()
    {
        if (_AtackBool)
        {
            _AtackBool = false;
            return true;
        }
        else return false;

    }
    public void LoadRun()
    {
        if (procent != 1000)
        {
            procent += 1000 / (timeLoad * 100);
            SetLoad(procent);
        }
        else SetLoad(0);
    }
    public bool SetDamageMobs()
    {
        if (_DamageMob)
        {
            _DamageMob = false;
            return true;
        }
        else return false;
    }
    public void SetLoad(float proc)
    {
        Load.fillAmount = proc / 1000;
    }
    #endregion
    public bool SetDamage()
    {
        if (_Damage)
        {
            _Damage = false;
            return true;
        }
        else return false;
    }
    public List<bool> GetAllDatabool()
    {
        return new List<bool>
        {
            SetAtack(),
            _JumpBool,
            _LeftBool,
            _RightBool,
            SetRun(),
            _DownBool,
            SetDamage(),
            _DownSide,
            _RightSide,
            _LeftSide,
            _RightBlc,
            _LeftBlc,
            _JumpBlock,
            SetDamageMobs(),
            _AnimationAtack,
            _RunnerAtack
        };
    }
    public List<float> GetAllDatafloat()
    {
        return new List<float>
        {
            Health
        };
    }

    public void setText()
    {
        text.text = Health.ToString();
    }
    public void GetDamage(float damage)
    {
        if (Random.Range(0, 50) == 1)
        {
            Health -= damage * 0.3f;
        }
        else
        {
            Health -= damage;
        }
        _Damage = true;
    }

}
