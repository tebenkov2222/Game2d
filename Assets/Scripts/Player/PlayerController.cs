using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public LayerMask lm;
    public GameObject AtackRegion, AtackRegionRunner, ElevatorRegion;
    public Image Load;
    public Text text;
    public string TagWeapons;
    [HideInInspector]
    public bool
    #region UI Buttons
        _AtackBool = false,
        _JumpBool = false,
        _LeftBool = false,
        _RightBool = false,
        _DownBool = false,
        _UpBool = false,
        _Runnerbool = false,
        _SitBool = false,
    #endregion
    #region Sides
        _DownSide = false,
        _RightSide = false,
        _LeftSide = false,
        _RightBlc = false,
        _LeftBlc = false,
        _JumpSide = false,
    #endregion
    #region StatesPlayer
        _ActivePlayer = true,
        _AtackMob = false,
        _Damage = false,
        _DamagePlayer = false,
        _Movd = false;
    #endregion
    private bool Desctop;
    private Vector2 TransformPositionAtackRegionStart,
        TransformPositionAtackRegionRunnerStart;
    public float Health = 50, HealthMax = 50;
    float procent = 1000,
        timeLoad = 2;
    public int ArrowNow,
        ArrowMax= 5;
    private void Start()
    {
        ArrowNow = ArrowMax;
        TransformPositionAtackRegionRunnerStart = AtackRegionRunner.transform.localPosition;
        Desctop = SystemInfo.deviceType == DeviceType.Desktop;
        TransformPositionAtackRegionStart = AtackRegion.transform.localPosition;
    }
    public bool GetArrow()
    {
        return ArrowNow != 0;
    }
    public void ArrowMinus()
    {
        ArrowNow--;
    }
    public string GetTagWeapons()
    {
        return TagWeapons;
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
        if ((_LeftBool || _RightBool) && !_SitBool) return true;
        else return false;
    }
    public void LeftAtackRegion()
    {
        AtackRegionRunner.transform.localPosition = new Vector2(-TransformPositionAtackRegionRunnerStart.x, TransformPositionAtackRegionRunnerStart.y);
        AtackRegion.transform.localPosition = new Vector2(-1 * TransformPositionAtackRegionStart.x, TransformPositionAtackRegionStart.y);
    }
    public void RightAtackRegion()
    {
        AtackRegionRunner.transform.localPosition = new Vector2(TransformPositionAtackRegionRunnerStart.x, TransformPositionAtackRegionRunnerStart.y);
        AtackRegion.transform.localPosition = new Vector2(TransformPositionAtackRegionStart.x, TransformPositionAtackRegionStart.y);
    }
    private bool ElevatorTest()
    {

        List<Collider2D> arr = new List<Collider2D>(Physics2D.OverlapBoxAll(ElevatorRegion.transform.position, ElevatorRegion.transform.localScale, 0)) ;
        for (int i = 0; i < arr.Count; i++)
        {
            if (arr[i].gameObject.layer == 16) { return true; }
        }
        return false;
    }
    public void Elevate()
    {
        List<Collider2D> arr = new List<Collider2D>(Physics2D.OverlapBoxAll(ElevatorRegion.transform.position, ElevatorRegion.transform.localScale, 0));
        for (int i = 0; i < arr.Count; i++)
        {
            if (arr[i].gameObject.layer == 16)
            {
                this.gameObject.transform.position = new Vector2(arr[i].transform.position.x, this.gameObject.transform.position.y);
            }
        }
    }
    private void FixedUpdate()
    {
        if (Desctop)
        {
            if (Input.GetKey(KeyCode.D))
            {
                _RightBool = true;
            }
            else _RightBool = false;
            if (Input.GetKey(KeyCode.A))
            {
                _LeftBool = true;
            }
            else _LeftBool = false;
            if (Input.GetKey(KeyCode.S))
            {
                _DownBool = true;
            }
            else _DownBool = false;
            if (Input.GetKey(KeyCode.W))
            {
                _UpBool = true;
            }
            else _UpBool = false;
        }
        _Movd = checkMove();
        LoadRun();
    }
    #region UI
    public void Atack()
    {
        _AtackBool = true;
    }
    public void Jump(bool State)
    {
        _JumpBool = State;
    }
    public void Down(bool State)
    {
        if (!Desctop) _DownBool = State;
    }
    public void Up(bool State)
    {
        if (!Desctop) _UpBool = State;
    }
    public void Left(bool State)
    {
        if (!Desctop) _LeftBool = State;
    }
    public void Right(bool State)
    {
        if (!Desctop) _RightBool = State;
    }
    public void Run()
    {
        if (procent == 1000)
        {
            _Runnerbool = true;
            procent = 0;
        }
    }
    public void SitDown()
    {
        _SitBool = !_SitBool;
    }

    public bool SetRun()
    {
        if (_Runnerbool)
        {
            _Runnerbool = false;
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
            procent += 10 / (timeLoad * 100 * Time.deltaTime);
            SetLoad(procent);
        }
        else SetLoad(0);
    }
    public bool SetAtackMobs()
    {
        if (_AtackMob)
        {
            _AtackMob = false;
            return true;
        }
        else return false;
    }
    public void SetLoad(float proc)
    {
        Load.fillAmount = proc / 1000;
    }
    #endregion
    private bool Life()
    {
        if (Health > 0) return true;
        return false;
    }
    public bool SetDamage()
    {
        if (_Damage)
        {
            _Damage = false;
            return true;
        }
        else return false;
    }
    /// <summary>
   ///   SetAtack(), 
   ///  _JumpBool,
   ///  _LeftBool,
   ///  _RightBool,
   ///  _DownBool,
   ///  _UpBool, 
   ///  SetRun(),
   ///  _SitBool,
   ///  SetDamage(),
   ///  _DownSide,
   ///  _RightSide,
   ///  _LeftSide,
   ///  _RightBlc,
   ///  _LeftBlc,
   ///  _JumpSide,
   ///  SetDamageMobs(),
    /// </summary>
    /// <returns></returns>
    public List<bool> GetAllDatabool()
    {
        return new List<bool>
        {
            SetAtack(), // buttons
            _JumpBool,
            _LeftBool,
            _RightBool,
            _DownBool,
            _UpBool, 
            SetRun(),
            _SitBool,
            SetDamage(),
            _DownSide,
            _RightSide,
            _LeftSide,
            _RightBlc,
            _LeftBlc,
            _JumpSide,
            SetAtackMobs(),
            Life(),
            ElevatorTest()
        };
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
