using System;
using UnityEngine;
using UnityEngine.VFX;

public class BonfireVFXManager : MonoBehaviour
{
    public AnimationCurve lightIntensityCurve;
    public VisualEffect vfx;
    public LerpLight light;
    public float simulationDeltaTime;
    public uint simulationSteps;
    
    public float t;
    public float time = 1f;

    private void Awake()
    {
        light.frameByFrame = true;
        light.customCurveMax = lightIntensityCurve;
    }

    private void Update()
    {
        t += Time.deltaTime / time;

        var lerpValue = lightIntensityCurve.Evaluate(t);
        // _light.intensity = Mathf.LerpUnclamped(startIntensity, endIntensity, lerpValue);
        vfx.SetFloat("FlameScale", Mathf.LerpUnclamped(0, 1, lerpValue));
        vfx.Simulate(simulationDeltaTime, simulationSteps);
        light.ApplyLerp(t);
    }
}