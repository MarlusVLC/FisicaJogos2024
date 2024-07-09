using UnityEngine;

namespace _1._AvaliacaoDiagnostica
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private Vector3 target;
        [SerializeField] private float speed;

        private void Update()
        {
            // if (Vector3.Distance(transform.position, target) <= 1.0f)
            if ((target - transform.position).sqrMagnitude <= 1.0f)
            {
                print((target - transform.position).sqrMagnitude);
                return;
            }

            // transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            transform.position += (target - transform.position).normalized * (speed * Time.deltaTime);
        }
    }
}
