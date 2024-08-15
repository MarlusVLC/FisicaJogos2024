using UnityEngine;

public class DeltaMovement : MonoBehaviour
{
    public float speed = 1;

    //FixedUpdate é chamada a cada frame FÍSICO
    void FixedUpdate()
    {
        transform.position += Vector3.right * (speed * Time.fixedDeltaTime);
    }
}
