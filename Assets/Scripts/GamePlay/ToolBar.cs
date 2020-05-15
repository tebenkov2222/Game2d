using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolBar : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SToolBar[] tools;
    private float StartTouch, EndTouch;
    public float Drag = 2f;
    private bool Active = false;
    private int Capacity = 0;
    private PlayerController mp;
    public void OnBeginDrag(PointerEventData eventData)
    {
        StartTouch = eventData.position.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Active = true;
        EndTouch = eventData.position.y;
    }
    private void Start()
    {
        mp = GameObject.Find("Player").GetComponent<PlayerController>();
        ChangeSprite();
    }
    private void UpCapacity()
    {
        if (Capacity < tools.Length-1) Capacity++; else Capacity = 0;
    }
    private void DownCapacity()
    {
        if (Capacity > 0) Capacity--; else Capacity = tools.Length-1;
    }
    private void ChangeSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = tools[Capacity].GetSprite;
        mp.TagWeapons = tools[Capacity].GetTag;
    }
    void Update()
    {
        if (Active)
        {
            Active = false;
            if (EndTouch - StartTouch > Drag)
            {
                UpCapacity();
                ChangeSprite();
            }
            if (StartTouch - EndTouch > Drag)
            {
                DownCapacity();
                ChangeSprite();
            }
        }
    }
}
