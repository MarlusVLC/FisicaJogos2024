using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve lightingCurve;
    [SerializeField] private AnimationCurve fireCurve;
    [SerializeField] private LerpLight lerpLight;
    [SerializeField] private float[] particleMultipliers;
    [SerializeField] private ParticleSystem[] particleSystems;
    [SerializeField] private float particleOffset;
    
    public float t;
    public float time = 1f;

    private void Start()
    {
        lerpLight.customCurveMax = lightingCurve;
        lerpLight.frameByFrame = true;
    }

    private void Update()
    {
        t += Time.deltaTime / time;

        lerpLight.ApplyLerp(t);
        
        for (var i = 0; i < particleSystems.Length; i++)
        {
            var system = particleSystems[i];
            var lifetimeSize = system.emission;
            lifetimeSize.rateOverTime = fireCurve.Evaluate(t+particleOffset) * particleMultipliers[i];
        }
    }
}