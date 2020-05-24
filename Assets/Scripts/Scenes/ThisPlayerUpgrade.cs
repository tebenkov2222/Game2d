using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ThisPlayerUpgrade : MonoBehaviour
{
    public SPlayers Now, Next;
    public PlayerUpgrade Controller;
    public HorizontalScrollSnap Snap;
    public Text
        speedNow,
        Damage_ANow,
        Damage_BNow,
        HealthNow,
        ArrowNow,

        speedNext,
        Damage_ANext,
        Damage_BNext,
        HealthNext,
        ArrowNext,
        
        Money;
    public Color
        colorPlus,
        ColorNull,
        ColorMax,
        CoinSet,
        CoinNull;
    public int value;
    void Update()
    {
        value = Snap.GetPage();
        Now = Controller.GetDataNow(value);
        Next = Controller.GetDataNext(value);
        UpdateAllText();
    }
    void UpdateAllText()
    {

        speedNow.text = Now.GetSpeed().ToString();
        Damage_ANow.text = Now.GetDamage_A().ToString();
        Damage_BNow.text = Now.GetDamage_B().ToString();
        HealthNow.text = Now.GetHealth().ToString();
        ArrowNow.text = Now.GetArrows().ToString();
        if (Next != null)
        {
            Money.text = Next.GetMoney().ToString();
            Money.color = GetColorCoin(PlayerPrefs.GetInt("Coin") - Next.GetMoney());
            speedNext.text = GetText(Next.GetSpeed() - Now.GetSpeed());
            Damage_ANext.text = GetText(Next.GetDamage_A() - Now.GetDamage_A());
            Damage_BNext.text = GetText(Next.GetDamage_B() - Now.GetDamage_B());
            HealthNext.text =  GetText (Next.GetHealth() - Now.GetHealth());
            ArrowNext.text =  GetText (Next.GetArrows() - Now.GetArrows());

            speedNext.color = GetColor(Next.GetSpeed() - Now.GetSpeed());
            Damage_ANext.color = GetColor(Next.GetDamage_A() - Now.GetDamage_A());
            Damage_BNext.color = GetColor(Next.GetDamage_B() - Now.GetDamage_B());
            HealthNext.color = GetColor(Next.GetHealth() - Now.GetHealth());
            ArrowNext.color = GetColor(Next.GetArrows() - Now.GetArrows());
        }
        else
        {
            Money.text = "MAX";
            speedNext.text = "MAX";
            Damage_ANext.text = "MAX";
            Damage_BNext.text = "MAX";
            HealthNext.text = "MAX";
            ArrowNext.text = "MAX";

            speedNext.color = GetColor(-1);
            Damage_ANext.color = GetColor(-1);
            Damage_BNext.color = GetColor(-1);
            HealthNext.color = GetColor(-1);
            ArrowNext.color = GetColor(-1);
        }
    }
    public void Button()
    {
        if (Next != null)
        {
            if (PlayerPrefs.GetInt("Coin") > Next.GetMoney())
            {
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - Next.GetMoney());
                upgrade();

            }
        }
    }
    public void upgrade()
    {
        if (value == 0)
        {
            if (PlayerPrefs.GetInt("WomanUpgrade") < 4) PlayerPrefs.SetInt("WomanUpgrade", PlayerPrefs.GetInt("WomanUpgrade") + 1);
        }
        else
        {
            if (PlayerPrefs.GetInt("MagUpgrade") < 4) PlayerPrefs.SetInt("MagUpgrade", PlayerPrefs.GetInt("MagUpgrade") + 1);
        }
    }
    string GetText(float Sum)
    {
        if (Sum != 0) return "+" + Sum.ToString();
        else return "-";
    }
    Color GetColor(float Sum)
    {
        if (Sum == -1) return ColorMax;
        else if (Sum != 0) return colorPlus;
        else return ColorNull;
    }
    Color GetColorCoin(float Sum)
    {
        if (Sum >= 0) return CoinSet;
        else return CoinNull;
    }
}
