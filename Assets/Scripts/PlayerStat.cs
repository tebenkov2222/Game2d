using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat Instance;
    private int _countKills = 0;

    private int _countKritDamage = 0;
    private int _countFireBallRepulsed = 0;

    private int _timeStart;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
    }

    public void CountKillsUp()
    {
        _countKills++;
    }
    public void CountKritDamageUp()
    {
        _countKritDamage++;
    }
    public void CountFireBallRepulsedUp()
    {
        _countFireBallRepulsed++;
    }

    public int CountFireBallRepulsed => _countFireBallRepulsed;
    public int CountKritDamage => _countKritDamage;
    public int CountKills => _countKills;
}
