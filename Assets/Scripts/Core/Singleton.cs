using Core.UI;
using UnityEngine;

namespace Core
{
    public abstract class Singleton<T> : MonoBehaviour
    {
        public static T Instance { get; private set; }

        public void Clear()
        {
            Instance = default(T);
        }
        public void Single(T reference)
        {
            Instance = reference;
        }
    }
    
    public abstract class SingletonUI<T> : MonoUI
    {
        public static T Instance { get; private set; }

        public void Clear()
        {
            Instance = default(T);
        }
        public void Single(T reference)
        {
            Instance = reference;
        }
    }

}