using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishStat : MonoBehaviour
{
    public Text CountKillsText;
    public Text CountCritDamageText;
    public Text CountFireBallRepulsedText;
    public Text AddMOneyText;
    [SerializeField] private int _addCountKills = 20;
    [SerializeField] private int _addCountCritDamage = 5;
    [SerializeField] private int _addCountKillsFireBall = 3;
    private int AddMoney = 0;
    void Start()
    {
        CountKillsText.text = PlayerStat.Instance.CountKills.ToString();
        CountCritDamageText.text = PlayerStat.Instance.CountKritDamage.ToString();
        CountFireBallRepulsedText.text = PlayerStat.Instance.CountFireBallRepulsed.ToString();
        AddMoney = PlayerStat.Instance.CountKills * _addCountKills + PlayerStat.Instance.CountKritDamage * _addCountCritDamage +
                   PlayerStat.Instance.CountFireBallRepulsed * _addCountKillsFireBall;
        AddMOneyText.text = AddMoney.ToString();
    }
}
