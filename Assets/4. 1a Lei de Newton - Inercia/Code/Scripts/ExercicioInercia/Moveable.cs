using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _4.Inercia
{
    public class Moveable : MonoBehaviour
    {
        [SerializeField] private float mass;
        [SerializeField] private float propulsionIntensity;
        [SerializeField] private Vector3 velocity;
        [SerializeField] private float rotationDuration;

        private Coroutine rotationMovement;
        
        public UnityEvent<Vector3> onRotationCompleted;

        private void FixedUpdate()
        {
            Move();
        }
        
        private void OnValidate()
        {
            mass = Mathf.Clamp(mass, 0.0000000001f, float.MaxValue);
        }

        public void AddImpulse(float impulseIntensity, Vector3 target)
        {
            var direction = (target - transform.position).normalized;
            var force = direction * impulseIntensity;
            // var acceleration = force / mass;
            // var momentum = acceleration * Time.fixedDeltaTime
            // velocity += momentum;
            velocity += (force / mass) * Time.fixedDeltaTime;
        }

        public void AddImpulse(Vector3 target) => AddImpulse(propulsionIntensity, target);

        public void Move()
        {
            transform.position += velocity * Time.fixedDeltaTime;
        }

        public IEnumerator LinearlyRotateTowards(Vector3 target, float movementDuration)
        {
            var direction = (target - transform.position).normalized;
            var crossUptoDirection = transform.up.x * direction.y - direction.x * transform.up.y;
            if (crossUptoDirection == 0) crossUptoDirection = 1;
            var rotationDirection = crossUptoDirection / Mathf.Abs(crossUptoDirection);
            var currentToTarget = VectorN.Angle(transform.up, direction) * rotationDirection;
            if (float.IsNaN(currentToTarget))
            {
                yield break;
            }
            var velocity = currentToTarget  * Time.fixedDeltaTime / movementDuration;
            velocity *= Mathf.Rad2Deg;
            var velocityVector = new Vector3(0, 0 ,velocity);

            var timer = 0.0f;
            while (timer < movementDuration) 
            {
                transform.eulerAngles += velocityVector ;
                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            onRotationCompleted.Invoke(target);
        }
        
        //TODO(Marlus) Create an accelerated version of rotation uniform motion
        public void BeginNewRotation(Vector3 target)
        {
            if (rotationMovement == null)
            {
                rotationMovement = StartCoroutine(LinearlyRotateTowards(target, rotationDuration));
            }
            else
            {
                StopRotation();
                rotationMovement = StartCoroutine(LinearlyRotateTowards(target, rotationDuration));
            }
        }

        public void StopRotation()
        {
            if (rotationMovement != null)
            {
                StopCoroutine(rotationMovement);
            }
        }
    }
}