using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HorrorLightManager : MonoBehaviour
{
    public LerpLight light;
    public AnimationCurve introCurve;
    public AnimationCurve[] idleCurves;
    [Min(0.01f)]public float changeRate = 1f;

    public float changeTimer = 0f;

    private void Awake()
    {
        light.customCurveMax = introCurve;
    }

    private void Update()
    {
        changeTimer += Time.deltaTime;
        if (changeTimer > changeRate)
        {
            light.customCurveMax = idleCurves[Random.Range(0, idleCurves.Length)];
            changeTimer = 0f;
        }
    }
}