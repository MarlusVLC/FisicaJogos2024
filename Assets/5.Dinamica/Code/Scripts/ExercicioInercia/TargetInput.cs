using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _5.Dinamica
{
    public class TargetInput : MonoBehaviour
    {
        public UnityEvent<Vector3> onMouseLeft;
        public UnityEvent<Vector3> onMouseRightDown;

        private Camera camera;

        private void Start()
        {
            camera = Camera.main;
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                SetTarget(true);
            }

            if (Input.GetMouseButtonDown(1))
            {
                SetTarget(false);
            }
        }

        private void SetTarget(bool isLeft)
        {
            var targetPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0;
            if (isLeft)
            {
                onMouseLeft.Invoke(targetPosition);
            }
            else
            {
                onMouseRightDown.Invoke(targetPosition);
            }
        }
    }
}