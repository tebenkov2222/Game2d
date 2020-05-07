using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float lengthx, lengthy, startposx, startposy;
    public GameObject cam;
    public float parallexEffect;
    void Start()
    {
        startposx = transform.position.x;
        startposy = transform.position.y;
        lengthx  = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        lengthy = GetComponentInChildren<SpriteRenderer>().bounds.size.y;
    }
    void Update()
    {
        float tempx = (cam.transform.position.x * (1 - parallexEffect));
        float tempy = (cam.transform.position.y * (1 - parallexEffect));
        float distx = (cam.transform.position.x * parallexEffect);
        float disty = (cam.transform.position.y * parallexEffect);
        transform.position = new Vector3(startposx + distx, startposy + disty, transform.position.z);
        if (tempx > startposx + lengthx) startposx += lengthx;
        else if (tempx < startposx - lengthx) startposx -= lengthx;
        if (tempy > startposy + lengthy) startposy += lengthy;
        else if (tempy < startposy - lengthy) startposy -= lengthy;
    }
}