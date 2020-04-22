using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSettingsPlayer : MonoBehaviour
{
    [SerializeField] Image Load;
    GameObject Player;
    PlayerController mp;
    private void Start()
    {
        Player = GameObject.Find("Player");
        mp = Player.GetComponent<PlayerController>();
       mp.text = this.GetComponentInChildren<Text>();
        Player.GetComponent<SpriteRenderer>().enabled = true;
       mp.ActivePlayer = true;
        Player.GetComponent<Rigidbody2D>().isKinematic = false;
       mp.Load = Load;
    }
    #region UI
    public void Atack()
    {
        mp.Atack();
    }
    public void Jump()
    {
        mp.Jump();
    }
    public void MoveLeft()
    {
        mp.Left();
    }
    public void MoveRight()
    {
        mp.Right();
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
