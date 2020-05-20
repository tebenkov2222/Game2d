using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerObject", menuName = "Player", order = 51)]
public class SPlayers : ScriptableObject
{
    [SerializeField] private Vector2 Position;
    [SerializeField] private string tag;
    public Vector2 GetSprite
    {
        get
        {
            return Position;
        }
        protected set { }
    }
    public string GetTag
    {
        get
        {
            return tag;
        }
        protected set { }
    }
}
