using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    public SPlayers[] 
        SWoman,
        SMag;
    public SPlayers GetDataNow(int value)
    {
        if (value == 0) return SWoman[PlayerPrefs.GetInt("WomanUpgrade")];
        else return SWoman[PlayerPrefs.GetInt("MagUpgrade")];
    }
    public SPlayers GetDataNext(int value)
    {
        if (value == 0)
        {
            Debug.Log(PlayerPrefs.GetInt("WomanUpgrade"));
            if (PlayerPrefs.GetInt("WomanUpgrade") < 4) return SWoman[PlayerPrefs.GetInt("WomanUpgrade") + 1];
            else return null;
        }
        else
        {
            if (PlayerPrefs.GetInt("MagUpgrade") < 4) return SWoman[PlayerPrefs.GetInt("MagUpgrade") + 1];
            else return null;
        }
    }
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("WomanUpgrade")) PlayerPrefs.SetInt("WomanUpgrade", 0);
        if (!PlayerPrefs.HasKey("MagUpgrade")) PlayerPrefs.SetInt("MagUpgrade", 0);
    }
    #region GetData
    public float GetSpeed(int value, int count)
    {
        if (value == 1)
        {
            return SWoman[count].GetSpeed();
        }
        else return SMag[count].GetSpeed();
    }    
    public float GetDamage_A(int value, int count)
    {
        if (value == 1)
        {
            return SWoman[count].GetDamage_A();
        }
        else return SMag[count].GetDamage_A();
    }    
    public float GetDamage_B(int value, int count)
    {
        if (value == 1)
        {
            return SWoman[count].GetDamage_B();
        }
        else return SMag[count].GetDamage_B();
    }    
    public float GetHealth(int value, int count)
    {
        if (value == 1)
        {
            return SWoman[count].GetHealth();
        }
        else return SMag[count].GetHealth();
    }    
    public float GetArrows(int value, int count)
    {
        if (value == 1)
        {
            return SWoman[count].GetArrows();
        }
        else return SMag[count].GetArrows();
    }
    public int GetUpgradeNow(int value)
    {
        if (value == 1)
        {
            return PlayerPrefs.GetInt("WomanUpgrade");
        }
        else
        {
            return PlayerPrefs.GetInt("MagUpgrade");
        }
    }
    #endregion
}
