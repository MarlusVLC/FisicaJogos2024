using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _6.AcaoReacao
{
    public class TargetInput : Singleton<TargetInput>
    {
        public UnityEvent<Vector3> onMouseLeft;
        public UnityEvent<Vector3> onMouseLeftDown;
        public UnityEvent<Vector3> onMouseLeftUp;
        public UnityEvent<Vector3> onMouseRight;
        public UnityEvent<Vector3> onMouseRightDown;
        public UnityEvent<Vector3> onDirectionalAxisPressed;
        public UnityEvent onSpaceBarPressed;

        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            TryGetInputAxis();
            if (Input.GetMouseButton(0))
            {
                onMouseLeft.Invoke(GetMousePosition());
            }
            if (Input.GetMouseButtonDown(0))
            {
                onMouseLeftDown.Invoke(GetMousePosition());
            }
            if (Input.GetMouseButtonUp(0))
            {
                onMouseLeftUp.Invoke(GetMousePosition());
            }
            if (Input.GetMouseButton(1))
            {
                onMouseRight.Invoke(GetMousePosition());
            }
            if (Input.GetMouseButtonDown(1))
            {
                onMouseRightDown.Invoke(GetMousePosition());
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                onSpaceBarPressed.Invoke();
            }
        }
        
        private new void OnDestroy()
        {
            base.OnDestroy();
            onMouseRight.RemoveAllListeners();
            onMouseLeft.RemoveAllListeners();
            onMouseLeftDown.RemoveAllListeners();
            onMouseRightDown.RemoveAllListeners();
            onDirectionalAxisPressed.RemoveAllListeners();
            onSpaceBarPressed.RemoveAllListeners();
        }

        private static Vector3 GetInputAxis()
        {
            return new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        }

        private void TryGetInputAxis()
        {
            var direction = GetInputAxis();
            onDirectionalAxisPressed.Invoke(direction);
        }

        public Vector3 GetMousePosition()
        {
            var targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0;
            return targetPosition;
        }
    }
}