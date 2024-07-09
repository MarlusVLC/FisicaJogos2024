using UnityEngine;

namespace _3.Cinematica
{
    public class TargetManager : MonoBehaviour
    {
        [SerializeField] private Moveable moveable;
        [SerializeField] private TargetInput targetInput;

        private void OnEnable()
        {
            targetInput.onTargetSet.AddListener((_ => moveable.TryStopTranslation()));
            targetInput.onTargetSet.AddListener((_ => moveable.TryStopRotation()));
            targetInput.onTargetSet.AddListener(moveable.BeginNewRotation);
            moveable.onRotationCompleted.AddListener(moveable.BeginNewTranslation);
            moveable.onTranslationCompleted.AddListener(_ => targetInput.DisableTarget());
        }

        private void OnDisable()
        {
            targetInput.onTargetSet.RemoveAllListeners();
            moveable.onRotationCompleted.RemoveAllListeners();
            moveable.onTranslationCompleted.RemoveAllListeners();
        }
    }
}