using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class BonfireVFXManager : MonoBehaviour
{
    public AnimationCurve lightIntensityCurve;
    public VisualEffect vfx;
    public AudioSource sfx;
    public LerpLight light;
    public Vector2 flameMinScale;
    public Vector2 flameMaxScale;
    public string flameScaleAccessName;
    [FormerlySerializedAs("time")] public float curveDuration = 1f;
    
    [SerializeField][ReadOnly] private float t;

    private void Awake()
    {
        light.frameByFrame = true;
        light.customCurveMax = lightIntensityCurve;
    }

    private void Update()
    {
        t += Time.deltaTime / curveDuration;
        var lerpValue = lightIntensityCurve.Evaluate(t);
        
        vfx.SetVector2(flameScaleAccessName, Vector2.LerpUnclamped(flameMinScale, flameMaxScale, lerpValue));
        sfx.volume = Mathf.LerpUnclamped(0f, 1f, lerpValue);
        light.ApplyLerp(t);
    }
}