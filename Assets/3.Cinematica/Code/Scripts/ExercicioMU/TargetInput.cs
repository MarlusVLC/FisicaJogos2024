using UnityEngine;
using UnityEngine.Events;

namespace _3.Cinematica
{
    public class TargetInput : MonoBehaviour
    {
        [SerializeField] private Transform targetIcon;
        public UnityEvent<Vector3> onTargetSet;

        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
            DisableTarget();
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }
            SetTarget();
        }

        private void SetTarget()
        {
            var targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0;
            targetIcon.position = targetPosition;
            targetIcon.gameObject.SetActive(true);
            onTargetSet.Invoke(targetPosition);
        }

        public void DisableTarget()
        {
            targetIcon.gameObject.SetActive(false);
        }
    }
}