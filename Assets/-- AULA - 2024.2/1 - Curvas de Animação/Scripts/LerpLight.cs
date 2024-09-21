using System;
using UnityEngine;

public class LerpLight : MonoBehaviour
{
    public float startIntensity;
    public float endIntensity;
    public float t;
    public float time = 1f;
    public bool reset = false;
    public bool isToggled = true;
    public AnimationCurve customCurve;

    private Light _light;

    [Header("frame config")]
    public bool frameByFrame = false;
    public bool advanceFrame = false;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isToggled == false)
        {
            reset = true;
        }
        
        if (reset)
        {
            t = 0.0f;
            _light.intensity = startIntensity;
            reset = false;
        }

        if (isToggled == false)
        {
            return;
        }

        if (advanceFrame || !frameByFrame)
        {
            float x = 0.0f;
            float y = 0.0f;
            t += Time.deltaTime / time;
            _light.intensity = Mathf.LerpUnclamped(startIntensity, endIntensity, customCurve.Evaluate(t));

            advanceFrame = false;
        }
        
    }
}
