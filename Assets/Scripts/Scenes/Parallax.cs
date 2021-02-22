using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Parallax : MonoBehaviour
{
    private float lengthx, lengthy, startposx, startposy,deltaposx = 0, deltaposy = 0,startposxcam, startposycam;
    public GameObject cam;
    public float parallexEffectx;
    public float parallexEffecty;
    void Start()
    {
        startposxcam = cam.transform.position.x;
        startposycam = cam.transform.position.y;
        startposx = transform.position.x;
        startposy = transform.position.y;
        lengthx  = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        lengthy = GetComponentInChildren<SpriteRenderer>().bounds.size.y;
    }
    void Update()
    {
        float tempx = (cam.transform.position.x - startposxcam) * (1 + parallexEffectx);
        float tempy = (cam.transform.position.y - startposycam) * (1 + parallexEffecty);
        float distx = (cam.transform.position.x - startposxcam) * -parallexEffectx;
        float disty = (cam.transform.position.y - startposycam) * -parallexEffecty;
        transform.position = new Vector3(startposx + distx + deltaposx, startposy + disty + deltaposy, transform.position.z);
        
        if (tempx > deltaposx + lengthx) deltaposx += lengthx;
        else if (tempx < deltaposx - lengthx) deltaposx -= lengthx;
        //if (tempy > deltaposy + lengthy) deltaposy += lengthy;
        //else if (tempy < deltaposy - lengthy) deltaposy -= lengthy;
    }

    public void UpdateValues()
    {
        startposxcam = cam.transform.position.x;
        startposycam = cam.transform.position.y;
        startposx = transform.position.x;
        startposy = transform.position.y;
        lengthx  = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        lengthy = GetComponentInChildren<SpriteRenderer>().bounds.size.y;
        deltaposx = 0;
        deltaposy = 0;
    }
}