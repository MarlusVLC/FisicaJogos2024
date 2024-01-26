using System.Collections;
using UnityEngine;
using CustomVector = _2._RevisaoVetores.CustomVector; 

namespace _2._RevisaoVetores
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private CustomVector target;
        [SerializeField] private float speed;

        private void Update()
        {
            var distance = CustomVector.Distance(target, transform.position);
            if (distance <= 1f)
            {
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }
}
