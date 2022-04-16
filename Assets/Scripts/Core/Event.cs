using System;
using UnityEngine.Events;

namespace Core
{
    [Serializable]
    public class Event : UnityEvent
    {
        public static Event operator+ (Event b, UnityAction c) {
            b.AddListener(c);
            return b;
        }
        public static Event operator- (Event b, UnityAction c) {
            b.RemoveListener(c);
            return b;
        }

        public void Run()
        {
            Invoke();
        }
    }

    [Serializable]
    public class Event<T> : UnityEvent<T>
    {
        public static Event<T> operator+ (Event<T> b, UnityAction<T> c) {
            b.AddListener(c);
            return b;
        }
        public static Event<T> operator- (Event<T>  b, UnityAction<T> c) {
            b.RemoveListener(c);
            return b;
        }
    
        public void Run(T data)
        {
            Invoke(data);
        }
    }
    [Serializable]
    public class Event<T,TK> : UnityEvent<T,TK>
    {
        public static Event<T, TK> operator+ (Event<T, TK> b, UnityAction<T, TK> c) {
            b.AddListener(c);
            return b;
        }
        public static Event<T, TK> operator- (Event<T, TK>  b, UnityAction<T, TK> c) {
            b.RemoveListener(c);
            return b;
        }
    
        public void Run(T data, TK data2)
        {
            Invoke(data, data2);
        }
    }
}