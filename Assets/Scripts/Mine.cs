using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Mine : MonoBehaviour
{
    public Exposion Exposion;
    public Animator _animator;
    public Light2D _light2D;
    public float speed = 0.5f, damage = 5;
    public GameObject PointBoom;
    private bool Damage = false;
    void Update()
    {
        List<Collider2D> ColAttack = new List<Collider2D>(Physics2D.OverlapBoxAll(this.transform.position, new Vector2(3, 0.3f), 0));
        for (int i = 0; i < ColAttack.Count; ++i)
        {
            if (ColAttack[i].gameObject.name == "Player")
            {
                Damage = true;
            }
        }
        if (Damage)
        {
            _animator.Play("MineAtack"); 
            if (_light2D.intensity < 5)
            {
                _light2D.enabled = true;
                _light2D.intensity += speed * Time.deltaTime;
            }
        }
    }
    public void StartAnim()
    {
        List<Collider2D> ColAttack = new List<Collider2D>(Physics2D.OverlapBoxAll(this.transform.position, new Vector2(3, 0.3f), 0));
        for (int i = 0; i < ColAttack.Count; ++i)
        {
            if (ColAttack[i].gameObject.name == "Player")
            {
                PlayerController.Instance.GetDamage(damage);
            }
        }
        Exposion.Boom();
        _light2D.intensity = 0;
    }
    public void endAnim()
    {
        Destroy(this.gameObject);
        _light2D.enabled = false;
    }
}
