using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CanvasSettingsPlayer : MonoBehaviour
{
    public Settings settings;
    public GameObject[] objects;
    public GameObject[] Joysticks;
    public GameObject[] JoysticksVariable;
    public FloatingJoystick FloatingJoystick;
    public DynamicJoystick DynamicJoystick;
    public FixedJoystick FixedJoystick;
    public GameObject DeathTable;
    [SerializeField] Image Load, Health, endurance;
    GameObject Player;
    public PlayerController mp;
    float Sensetive = 0.3f,
        Horizontal,
        Vertical;
    int SetJoystick = -1;
    private void Start()
    {
        PlayerPrefs.SetInt("Death", 0);
        if (PlayerPrefs.HasKey("Saved")) PlayerPrefs.SetInt("Saved", 1);
        LoadAllSettingsObject();
        Player = GameObject.Find("Player");
        mp = Player.GetComponent<PlayerController>();
        mp.text = this.GetComponentInChildren<Text>();
        Player.GetComponent<SpriteRenderer>().enabled = true;
        mp._ActivePlayer = true;
        Player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        mp.Load = Load;
    }

    private void SetHealth()
    {
        endurance.fillAmount = (float) mp.ArrowNow / mp.ArrowMax;
        Health.fillAmount = mp.Health / mp.HealthMax;
    }
    void changeJoystick()
    {
        bool visible = true;
        if (PlayerPrefs.HasKey("Visible"))
        {
            visible = (PlayerPrefs.GetInt("Visible") == 1);
        }
            int value = PlayerPrefs.GetInt("Joystick");
        if (SetJoystick != value)
        {
            for (int i = 0; i < 3; i++)
            {
                //Joysticks[i].GetComponent<Image>().enabled = visible;
                //Joysticks[i].GetComponentInChildren<Image>().enabled = visible;
                JoysticksVariable[i].SetActive(false);
            }
            SetJoystick = value;
            JoysticksVariable[value].SetActive(true);
        }
    }
    void LoadAllSettingsObject()
    {
        changeJoystick();
        if (PlayerPrefs.HasKey("Saved"))
        {
            if (PlayerPrefs.GetInt("Saved") == 1)
            {
                PlayerPrefs.SetInt("Saved", 0);
                settings.Load(objects);
                settings.Load(Joysticks);
            }
        }
    }
    private void CheckSensetive()
    {
        if (PlayerPrefs.HasKey("Sensetive")) Sensetive = 1 - PlayerPrefs.GetFloat("Sensetive");
    }
    private void GetAxis()
    {
        if (SetJoystick == 0)
        {
            Horizontal = DynamicJoystick.Horizontal;
            Vertical = DynamicJoystick.Vertical;
        }
        else if (SetJoystick == 1)
        {
            Horizontal = FixedJoystick.Horizontal;
            Vertical = FixedJoystick.Vertical;
        }
        else if (SetJoystick == 2)
        {
            Horizontal = FloatingJoystick.Horizontal;
            Vertical = FloatingJoystick.Vertical;
        }

    }
    private void Update()
    {
        SetHealth();
        LoadAllSettingsObject();
        CheckSensetive();
        GetAxis();
        if (Horizontal > Sensetive)  { MoveRight(true);} else MoveRight(false);
        if (Horizontal < -Sensetive) { MoveLeft(true); } else MoveLeft(false);
        if (Vertical > Sensetive) { Up(true); } else Up(false);
        if (Vertical < -Sensetive) { Down(true); } else Down(false);
        if (PlayerPrefs.GetInt("Death") == -1)
        {
            Time.timeScale = 0.000001f;
            DeathTable.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
    #region UI
    public void Up(bool State)
    {
        mp.Up(State);
    }
    public void Down(bool State)
    {
        mp.Down(State);
    }
    public void Atack()
    {
        mp.Atack();
    }
    public void Jump(bool State)
    {
        mp.Jump(State);
    }
    public void MoveLeft(bool State)
    {
        mp.Left(State);
    }
    public void MoveRight(bool State)
    {
        mp.Right(State);
    }
    public void Run()
    {
        mp.Run();
    }
    public void MoveSitDown()
    {
        mp.SitDown();
    }

    
    #endregion

}
