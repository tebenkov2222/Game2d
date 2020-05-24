using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class ShowAd : MonoBehaviour, IUnityAdsListener
{
#if UNITY_ANDROID
    private string gameId = "3617319";
#endif
    float tm = -1;
    Button myButton;
    public string myPlacementId = "rewardedVideo";
    public bool Ready, pressed;
    public GameObject PanelWait, PanelCanceled, panelFinish;
    float timeWait;

    void Start()
    {
        myButton = GetComponent<Button>();

        // Set interactivity to be dependent on the Placement’s status:
        Ready = Advertisement.IsReady(myPlacementId);

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (myButton) myButton.onClick.AddListener(ShowRewardedVideo);

        // Initialize the Ads listener and service:
        Advertisement.Initialize(gameId, false);
    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo()
    {
        Advertisement.AddListener(this);
        if (Advertisement.IsReady(myPlacementId))
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
    void CheckPanelWait()
    {
        if (pressed)
        {
            if (Ready)
            {
                pressed = false;
                Advertisement.Show(myPlacementId);
                PanelWait.SetActive(false);
            }
            if (Time.time - timeWait > 10)
            {
                pressed = false;
                PanelWait.SetActive(false);
                PanelCanceled.SetActive(true);
            }
        }
    }
    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            Advertisement.RemoveListener(this);
            Debug.Log("ADD LISTENER");
            if (Time.time - tm > 1)
            {
                panelFinish.SetActive(true);
                tm = Time.time;
                PlayerPrefs.SetInt("Coin", 500 + PlayerPrefs.GetInt("Coin"));
            }
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

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
    private void Update()
    {
        Ready = Advertisement.IsReady();
        CheckPanelWait();
    }
}