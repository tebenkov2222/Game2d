using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportRoomScript : MonoBehaviour
{
    public GameObject Player, PlayerEmpty, TeleportSettings;
    public string LevelName;
    public bool Levels = false;
    public void Teleported()
    {
        Player = GameObject.Find("Player");
        TeleportSettings = GameObject.Find("TeleportRoomSettings"); 
        if (Levels)
        {
            if (LevelName != "Main") 
            {
                DontDestroyOnLoad(Player);
            }
            Player.transform.position = PlayerEmpty.transform.position;
            Player.GetComponent<MovePlayer>().enabled = false;
            Player.GetComponent<Rigidbody2D>().isKinematic = true;
            this.GetComponent<Animator>().Play("TeleportLevel", 0); 
            
        }
        else {
            Player.transform.position = PlayerEmpty.transform.position;
            Player.GetComponent<MovePlayer>().enabled = false;
            Player.GetComponent<Rigidbody2D>().isKinematic = true;
            this.GetComponent<Animator>().Play("TeleportRoom", 0);
        }
    }
    public void DisablePlayer()
    {
        Player.GetComponent<SpriteRenderer>().enabled = false;
    }
    public void TeleportEnd()
    {
        if (Levels) SceneManager.LoadScene(LevelName);
        else TeleportSettings.GetComponent<TeleportRoomSettingsScript>().NextRoom(); 
    }
}
