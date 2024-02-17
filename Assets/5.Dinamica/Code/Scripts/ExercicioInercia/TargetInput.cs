using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _5.Dinamica
{
    public class TargetInput : MonoBehaviour
    {
        public UnityEvent<Vector3> onMouseLeft;
        public UnityEvent<Vector3> onMouseRightDown;
        public UnityEvent<Vector3> onDirectionalAxisPressed;

        private Camera camera;

        private void Start()
        {
            camera = Camera.main;
        }

        private void FixedUpdate()
        {
            onDirectionalAxisPressed.Invoke(GetInputAxis());
            if (Input.GetMouseButton(0))
            {
                SetTarget(true);
            }

            if (Input.GetMouseButtonDown(1))
            {
                SetTarget(false);
            }
        }

        private Vector3 GetInputAxis()
        {
            return new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
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