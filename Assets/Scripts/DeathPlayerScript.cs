using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathPlayerScript : MonoBehaviour, IUnityAdsListener
{
    private string gameId = "3617319";
    public string myPlacementId = "rewardedVideo";
    private bool Ready = false, pressed = false;
    public GameObject Player, PanelWait, PanelCanceled, canvas;
    public Text Coins, PayCoins;
    float timeWait;
    public PlayerController mp;
    public Color colorPlus, ColorNull;
    public void Play()
    {
        mp.Health = mp.HealthMax;
        PlayerPrefs.SetInt("Death", 0);
        this.gameObject.SetActive(false);
        canvas.SetActive(true);
        Time.timeScale = 1;
        SceneManager.LoadScene("Finish");
    }
    public void Pay()
    {
        if (PlayerPrefs.GetInt("Coin") > 200)
        {
            Time.timeScale = 1;
            this.gameObject.SetActive(false);
            canvas.SetActive(true);
            PlayerPrefs.SetInt("Death", 1);
            mp.Health = mp.HealthMax;
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 200);
        }
    }
    public void Ads()
    {
        Advertisement.AddListener(this);
        if (Ready)
        {
            Advertisement.Show(myPlacementId);
        }
        else
        {
            pressed = true;
            PanelWait.SetActive(true);
            timeWait = Time.time;
        }
    }
    Color GetColor(bool value)
    {
        if (value) return colorPlus;
        else return ColorNull;
    }
    void Start()
    {
        Player = GameObject.Find("Player");
        mp = Player.GetComponent<PlayerController>();
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, false);
    }

    // Update is called once per frame
    void Update()
    {
        PayCoins.color = GetColor(PlayerPrefs.GetInt("Coin") > 200);
        Coins.text = PlayerPrefs.GetInt("Coin").ToString();
        Ready = Advertisement.IsReady();
        CheckPanelWait();
    }

    void CheckPanelWait()
    {
        if (pressed)
        {
            if (Ready)
            {
                pressed = false;
                Advertisement.Show(myPlacementId);
                PanelWait.SetActive(false);
                this.gameObject.SetActive(false);
                canvas.SetActive(true);
            }
            if (Time.time - timeWait > 10)
            {
                pressed = false;
                PanelWait.SetActive(false);
                PanelCanceled.SetActive(true);
            }
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
    }
    public void OnUnityAdsDidError(string message)
    {
    }
    public void OnUnityAdsDidStart(string placementId)
    {
    }
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            Advertisement.RemoveListener(this);
            mp.Health = mp.HealthMax;
            PlayerPrefs.SetInt("Death", 1);
            this.gameObject.SetActive(false);
            canvas.SetActive(true);
            // Reward the user for watching the ad to completion.
        }
        else if (showResult == ShowResult.Skipped)
        {
            Advertisement.RemoveListener(this);
            Debug.Log("ОБМАНЩИК!!");
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Advertisement.RemoveListener(this);
            Debug.LogWarning("The ad did not finish due to an error");
        }
    }
}
