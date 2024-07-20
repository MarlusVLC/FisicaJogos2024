using UnityEngine;

public class CircleDistributor : MonoBehaviour
{
    public float centerDistance = 1;

    [ContextMenu("Distribute Children")]
    public void DistributeChildren()
    {
        var angle = 2 * Mathf.PI / transform.childCount;
        var currentAngle = 0f;
        for (int i = 0; i < transform.childCount; i++)
        {
            currentAngle = angle * i;
            transform.GetChild(i).position = transform.position + new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), 0) * centerDistance; 
        }
    }
}