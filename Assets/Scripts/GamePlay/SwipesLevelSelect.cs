﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
/// <summary>
/// Keeps constant camera width instead of height, works for both Orthographic & Perspective cameras
/// Made for tutorial https://youtu.be/0cmxFjP375Y
/// </summary>
public class SwipesLevelSelect : MonoBehaviour
{
    public string LevelName;
    public Text Coins;
    public Vector2 posN, scalN, 
        posP, scalP,
        posU, scalU;
    public GameObject[] Players;
    public GameObject Snap, Camera;
    public GameObject SelectedWindow, PrevWindow, UpdateWindow;
    public AudioClip backClip;
    public AudioSource audioSource;
    int value;
    bool isSelect = false,
        isUpdate = false;
    public void Selected()
    {
        value = Snap.GetComponent<HorizontalScrollSnap>().GetPage();value = Snap.GetComponent<HorizontalScrollSnap>().GetPage();
        if (value != 1)
        {
            isSelect = true;
            PrevWindow.SetActive(false);
            SelectedWindow.SetActive(true);

            Snap.GetComponent<HorizontalScrollSnap>().enabled = false;
            Snap.GetComponent<ScrollRect>().enabled = false;
        }
    }
    public void PlayerUpdate()
    {
        UpdateWindow.SetActive(true);
        SelectedWindow.SetActive(false);
        isUpdate = true;
    }
    public void SetPosition()
    {
        if (isSelect)
        {
            if (isUpdate)
            {
                Camera.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(Camera.GetComponent<RectTransform>().anchoredPosition, posU, 0.5f);
                Camera.transform.localScale = Vector2.Lerp(Camera.transform.localScale, scalU, 0.5f);
            }
            else
            {
                Camera.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(Camera.GetComponent<RectTransform>().anchoredPosition, posP, 0.5f);
                Camera.transform.localScale = Vector2.Lerp(Camera.transform.localScale, scalP, 0.5f);
            }
        }
        else
        {
            Camera.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(Camera.GetComponent<RectTransform>().anchoredPosition, posN, 0.5f);
            Camera.transform.localScale = Vector2.Lerp(Camera.transform.localScale, scalN, 0.5f);
        }
    }
    public void Back()
    {
        audioSource.PlayOneShot(backClip);
        if (isSelect)
        {
            if (isUpdate)
            {
                UpdateWindow.SetActive(false);
                SelectedWindow.SetActive(true);
                isUpdate = false;
            }
            else
            {
                PrevWindow.SetActive(true);
                SelectedWindow.SetActive(false);
                isSelect = false;
                Snap.GetComponent<HorizontalScrollSnap>().enabled = true;
                Snap.GetComponent<ScrollRect>().enabled = true;
            }
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
    }
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Coin")) PlayerPrefs.SetInt("Coin", 100);
    }
    public void Update()
    {
        Coins.text = PlayerPrefs.GetInt("Coin").ToString();
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
        }
        SetPosition();
    }
    public void SetPlayer()
    {
        if (value == 0)
        {
            SceneManager.LoadScene(LevelName);
        }
    }
}
