using UnityEngine;

namespace _6.AcaoReacao
{ 
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad = false;
        
        private static T instance;
        
        public static T Instance
        {
            get
            {
                // If the instance doesn't exist yet, find it in the scene
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    // If it's still null, create a new GameObject and add the SingletonExample script to it
                    if (instance == null)
                    {
                        var singletonObject = new GameObject(nameof(T));
                        instance = singletonObject.AddComponent<T>();
                    }
                }
                return instance;
            }
        }
        
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}