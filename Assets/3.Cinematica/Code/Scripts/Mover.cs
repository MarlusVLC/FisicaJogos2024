using UnityEngine;

namespace _3.Cinematica
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private VectorN target;
        [SerializeField] private float speed;

        private void Update()
        {
            var distance = VectorN.Distance(target, transform.position);
            if (distance <= 1f)
            {
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }
}
