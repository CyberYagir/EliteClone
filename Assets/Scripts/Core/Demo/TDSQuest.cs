using UnityEngine;

namespace Core.Core.Demo
{
    [System.Serializable]
    public class TDSQuest
    {
        [SerializeField] protected string text;
        [SerializeField] protected bool isCompleted;

        public bool IsComplited => isCompleted;
        public virtual void Calculate(){}

        public virtual void OnApply(){}

        public string GetText()
        {
            return text;
        }
    }
}