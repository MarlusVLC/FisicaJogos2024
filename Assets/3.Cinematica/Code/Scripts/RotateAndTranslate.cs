using UnityEngine;

namespace _3.Cinematica
{
    public class RotateAndTranslate : MonoBehaviour
    {
        private void Move(Vector3 target, float movementDuration)
        {
            var direction = (target - transform.position).normalized;
            float[,] tmat =
            {
                { 1, 0, 0 }, 
                { sin, cos, 0 },
                {0, 0, 1}
            };            
        }
    }
}