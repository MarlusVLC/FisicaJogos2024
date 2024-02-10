using UnityEngine;

namespace _3.Cinematica
{
    public class TargetManager : MonoBehaviour
    {
        [SerializeField] private Moveable moveable;
        [SerializeField] private TargetInput targetInput;

        private void OnEnable()
        {
            targetInput.onTargetSet.AddListener((_ => moveable.StopTranslation()));
            targetInput.onTargetSet.AddListener((_ => moveable.StopRotation()));
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