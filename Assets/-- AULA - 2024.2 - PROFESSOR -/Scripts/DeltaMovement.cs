using UnityEngine;

public class DeltaMovement : MonoBehaviour
{
    public float speed = 1;

    //FixedUpdate � chamada a cada frame F�SICO
    void FixedUpdate()
    {
        transform.position += Vector3.right * (speed * Time.fixedDeltaTime);
    }
}
