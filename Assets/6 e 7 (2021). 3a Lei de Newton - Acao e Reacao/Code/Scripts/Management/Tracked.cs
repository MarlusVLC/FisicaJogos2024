using System;
using UnityEngine;

namespace _6.AcaoReacao
{
    public abstract class Tracked<T> : MonoBehaviour where T : Tracked<T>
    {
        public int SystemID { get; private set; } = -1;

        protected void OnEnable()
        {
            SystemID = Tracker<T>.Instance.StartTracking((T)this);
        }

        protected void OnDisable()
        {
            if (CollisionManager.HasInstance == false)
                return;
            Tracker<T>.Instance.StopTracking((T)this);
        }
    }
}