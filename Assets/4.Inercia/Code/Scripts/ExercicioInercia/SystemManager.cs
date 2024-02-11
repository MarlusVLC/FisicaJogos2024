using UnityEngine;

namespace _4.Inercia
{
    public class SystemManager : MonoBehaviour
    {
        [SerializeField] private Moveable moveable;
        [SerializeField] private TargetInput targetInput;

        private void OnEnable()
        {
            // targetInput.onTargetSet.AddListener((_ => moveable.StopTranslation()));
            targetInput.onTargetSet.AddListener((_ => moveable.StopRotation()));
            targetInput.onTargetSet.AddListener(moveable.BeginNewRotation);
            targetInput.onTargetSet.AddListener(moveable.AddPropulsion);
            // moveable.onRotationCompleted.AddListener(moveable.BeginNewTranslation);
            // moveable.onTranslationCompleted.AddListener(_ => targetInput.DisableTarget());
        }

        private void OnDisable()
        {
            targetInput.onTargetSet.RemoveAllListeners();
            moveable.onRotationCompleted.RemoveAllListeners();
            moveable.onTranslationCompleted.RemoveAllListeners();
        }
    }
}