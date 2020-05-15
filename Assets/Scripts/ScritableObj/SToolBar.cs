using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ToolBarObject", menuName = "ToolBarObject", order = 51)]
public class SToolBar : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string tag;
    public Sprite GetSprite
    {
        get {
            return sprite;
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
