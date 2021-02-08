using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LiveFire : MonoBehaviour
{
    public AnimationCurve Intensivity;
    public float AnimationTime = 0.41f;
    private float _startAnimation;
    public Animator Animator;
    private float LightIntensivity;
    private float timeNow;
    private float maxTime;
    public Light2D Light2D;

    private float _startIntensivity;
    // Start is called before the first frame update
    void Start()
    {
        float offset = Random.Range(0, 1f);
        Animator.SetFloat("offset", offset);
        _startAnimation = AnimationTime * offset;
        _startIntensivity = Light2D.intensity;
        maxTime = Intensivity.keys[Intensivity.length-1].time;
        timeNow = _startAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        Light2D.intensity = _startIntensivity * Intensivity.Evaluate(timeNow);
        timeNow += Time.deltaTime;
        Debug.Log(Time.deltaTime);
        if (timeNow > maxTime) timeNow = 0;
    }
}
