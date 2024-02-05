using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _2._RevisaoVetores
{
    public class FOVThresholdCheck : MonoBehaviour
    {
        [FormerlySerializedAs("cameraTransform")] public Camera _camera;

        /// <summary>
        /// Can only be used to check if target is visible within a conical field of view.
        /// Shouldn't be used in a frustum
        /// </summary>
        [ContextMenu("Check Conical Threshold")]
        private void CheckConicalThreshold()
        {
            VectorN cameraPosition = _camera.transform.position;
            Debug.Log($"Camera Position = {cameraPosition}");
            VectorN targetPosition = transform.position;
            Debug.Log($"Target Position = {targetPosition}");
            VectorN toTarget = (targetPosition - cameraPosition).Normalize();
            Debug.Log($"To Target Normalized Vector = {toTarget}");
            VectorN cameraForward = _camera.transform.forward;
            Debug.Log($"Camera Forward = {cameraForward}");
            var dot = Vector3.Dot(cameraForward, toTarget);
            Debug.Log($"Dot Product Between Camera Forward and ToTarget = {dot}");

            var verticalFOVDegrees = _camera.fieldOfView;
            Debug.Log($"Vertical FOV Angle in degrees = {verticalFOVDegrees}");
            var verFOVAngle = verticalFOVDegrees * Mathf.PI / 180;
            Debug.Log($"Vertical FOV Angle in radians = {verFOVAngle}");
            var verThreshold = Mathf.Cos(verFOVAngle/2f);
            Debug.Log($"Vertical Threshold = {verThreshold}");

            var isVisible = dot >= verThreshold;
            Debug.Log($"Is Visible = {isVisible}");
        }
    }
}