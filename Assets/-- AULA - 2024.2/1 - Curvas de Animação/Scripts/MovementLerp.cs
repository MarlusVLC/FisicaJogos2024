using UnityEngine;

public enum TransitionType
{
    Linear,
    EaseInQuad,
    EaseInBack,
    EaseOutBounce,
    Relative,
    Custom,
}

public class MovementLerp : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public float t;
    public float time = 1f;
    public TransitionType type = TransitionType.Linear;
    public bool reset = false;
    public AnimationCurve customCurve;

    [Header("frame config")]
    public bool frameByFrame = false;
    public bool advanceFrame = false;
    
    // Update is called once per frame
    void Update()
    {
        if (reset)
        {
            t = 0.0f;
            transform.position = start.position;
            reset = false;
        }

        if (advanceFrame || !frameByFrame)
        {
            float x = 0.0f;
            float y = 0.0f;
            t += Time.deltaTime / time;
            switch (type)
            {
                case TransitionType.Linear:
                    transform.position = Vector3.Lerp(start.position, end.position, t);
                    // if (t is > 1 or < 0)
                    //     break;
                    // x = (1 - t) * start.position.x + t * end.position.x;
                    // y = (1 - t) * start.position.y + t * end.position.y;
                    // transform.position = new Vector3(x, y, transform.position.z);
                    break;
                case TransitionType.EaseInQuad:
                    // et = t * t; //EaseInQuad
                    // et = 1 - (1 - t) * (1 - t); //EaseOutQuad
                    transform.position = EaseInQuad(start.position, end.position, t);
                    
                    // if (t is > 1 or < 0)
                    //     break;
                    //
                    // x = (1 - et) * start.position.x + et * end.position.x;
                    // y = (1 - et) * start.position.y + et * end.position.y;
                    // transform.position = new Vector3(x, y, transform.position.z);
                    break;
                case TransitionType.EaseInBack:
                    transform.position = EaseInBack(start.position, end.position, t);
                    break;
                case TransitionType.EaseOutBounce:
                    transform.position = EaseOutBounce(start.position, end.position, t);
                    break;
                case TransitionType.Relative:
                    transform.position = Vector3.Lerp(transform.position, end.position, Time.deltaTime / time);
                    break;
                case TransitionType.Custom:
                    transform.position = Vector3.LerpUnclamped(start.position, end.position, customCurve.Evaluate(t));
                    break;
            }
            advanceFrame = false;
        }
        
    }

    public Vector3 EaseInQuad(Vector3 startPosition, Vector3 endPosition, float transition)
    {
        var easedValued = Mathf.Clamp01(transition);
        easedValued = easedValued * easedValued;
        return Vector3.LerpUnclamped(startPosition, endPosition, easedValued);
    }
    
    public Vector3 EaseInBack(Vector3 startPosition, Vector3 endPosition, float transition)
    {
        var c1 = 1.70158f;
        var c3 = c1 + 1;
        
        var easedValued = Mathf.Clamp01(transition);
        easedValued = c3 * Mathf.Pow(easedValued, 3) - c1 * Mathf.Pow(easedValued, 2);
        return Vector3.LerpUnclamped(startPosition, endPosition, easedValued);
    }

    public Vector3 EaseOutBounce(Vector3 startPosition, Vector3 endPosition, float transition)
    {
        var n1 = 7.5625f;
        var d1 = 2.75f;
        
        var easedValued = Mathf.Clamp01(transition);
        if (easedValued < 1 / d1) {
            easedValued = n1 * easedValued * easedValued;
        } else if (easedValued < 2 / d1) {
            easedValued = n1 * (easedValued - 1.5f / d1) * easedValued + 0.75f;
        } else if (easedValued < 2.5 / d1) {
            easedValued = n1 * (easedValued - 2.25f / d1) * easedValued + 0.9375f;
        } else {
            easedValued = n1 * (easedValued - 2.625f / d1) * easedValued + 0.984375f;
        }
        return Vector3.LerpUnclamped(startPosition, endPosition, easedValued);
    }
}
