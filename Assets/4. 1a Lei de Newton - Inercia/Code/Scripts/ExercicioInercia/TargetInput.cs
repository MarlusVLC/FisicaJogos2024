using UnityEngine;
using UnityEngine.Events;

namespace _4.Inercia
{
    public class TargetInput : MonoBehaviour
    {
        [SerializeField] private Transform targetIcon;
        public UnityEvent<Vector3> onTargetSet;

        private Camera camera;

        private void Start()
        {
            camera = Camera.main;
            DisableTarget();
        }

        private void FixedUpdate()
        {
            if (!Input.GetMouseButton(0))
            {
                targetIcon.gameObject.SetActive(false);
                return;
            }
            SetTarget();
        }

        private void SetTarget()
        {
            var targetPosition = camera.ScreenToWorldPoint(Input.mousePosition);
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