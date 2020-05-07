using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanvasSettingsPlayer : MonoBehaviour
{
    public FloatingJoystick variableJoystick;
    [SerializeField] Image Load;
    GameObject Player;
    PlayerController mp;
    private void Start()
    {
        variableJoystick = gameObject.GetComponentInChildren<FloatingJoystick>();
        Player = GameObject.Find("Player");
        mp = Player.GetComponent<PlayerController>();
       mp.text = this.GetComponentInChildren<Text>();
        Player.GetComponent<SpriteRenderer>().enabled = true;
       mp._ActivePlayer = true;
        Player.GetComponent<Rigidbody2D>().isKinematic = false;
       mp.Load = Load;
    }
    private void Update()
    {
        if (variableJoystick.Horizontal > 0.3)  { MoveRight(true);} else MoveRight(false);
        if (variableJoystick.Horizontal < -0.3) { MoveLeft(true); } else MoveLeft(false);
        if (variableJoystick.Vertical > 0.3) { Up(true); } else Up(false);
        if (variableJoystick.Vertical < -0.3) { Down(true); } else Down(false);
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
