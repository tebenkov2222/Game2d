using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerObject", menuName = "Player", order = 51)]
public class SPlayers : ScriptableObject
{
    [SerializeField] private float 
        Speed, 
        Damage_A,
        Damage_B,
        Health,
        Arrows,
        Money;
    public float GetSpeed()
    {
        return Speed;
    }
    public float GetDamage_A()
    {
        return Damage_A;
    }
    public float GetDamage_B()
    {
        return Damage_B;
    }
    public float GetHealth()
    {
        return Health;
    }
    public int GetArrows()
    {
        return (int) Arrows;
    }
    public int GetMoney()
    {
        return (int) Money;
    }
}
