using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class TODO : MonoBehaviour
    {
        [Serializable]
        public class TODORow
        {
            public enum TaskPriority {
                None, Middle, High
            
            }
            public string text;
            public bool isComplited;
            public TaskPriority priority;
        }
        public List<TODORow> tasks = new List<TODORow>();
    
    }
}
