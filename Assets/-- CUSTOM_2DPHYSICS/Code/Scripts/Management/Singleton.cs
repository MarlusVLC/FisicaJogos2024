using System;
using UnityEngine;
using UnityEngine.Events;

namespace _6.AcaoReacao
{ 
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad = false;

        protected UnityEvent<T> onInstanceDestruction;
        
        private static T instance;

        public static readonly Lazy<T> lazyInstance = new(() =>
        {
            T instance = FindObjectOfType<T>();
            
            if (instance == null)
            {
                var singletonObject = new GameObject(nameof(T));
                instance = singletonObject.AddComponent<T>();
            }

            return instance;
        });

        public static T Instance => lazyInstance.Value;

        public static bool HasInstance => lazyInstance.IsValueCreated;
        
        protected virtual void Awake()
        {
            if (HasInstance && Instance != this)
            {
                Destroy(Instance.gameObject);
            }
            else if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        protected void OnDestroy()
        {
            onInstanceDestruction?.Invoke(Instance);
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}