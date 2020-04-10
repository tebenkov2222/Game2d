using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSettingsPlayer : MonoBehaviour
{
    GameObject Player;
    MovePlayer mp;
    private void Start()
    {
        Player = GameObject.Find("Player");
        Player.GetComponent<MovePlayer>().text = this.GetComponentInChildren<Text>();
        Player.GetComponent<SpriteRenderer>().enabled = true;
        Player.GetComponent<MovePlayer>().enabled = true;
        Player.GetComponent<Rigidbody2D>().isKinematic = false;
        mp = Player.GetComponent<MovePlayer>();
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
        mp.MoveLeft();
    }
    public void MoveRight()
    {
        mp.MoveRight();
    }
    public void Run()
    {
        mp.Run();
    }
    public void MoveSitDown()
    {
        mp.MoveSitDown();
    }
    #endregion
}
