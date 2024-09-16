using UnityEngine;

public class Lerp : MonoBehaviour
{
    public enum TransitionType
    {
        Linear,
        Relative,
        Manual,
    }

    public Transform start;
    public Transform end;
    public float t;
    public float time = 1f;
    public TransitionType type = TransitionType.Linear;
    public bool reset = false;

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
            switch (type)
            {
                case TransitionType.Linear:
                    t += Time.deltaTime / time;
                    transform.position = Vector3.Lerp(start.position, end.position, t);
                    break;
                case TransitionType.Relative:
                    transform.position = Vector3.Lerp(transform.position, end.position, Time.deltaTime / time);
                    break;
            }
            advanceFrame = false;
        }
    }
}
