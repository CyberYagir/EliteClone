using UnityEngine;

namespace Core.Demo
{
    [System.Serializable]
    public class TDSQuest
    {
        public string Text { get; set; }
        [SerializeField] protected bool isCompleted;
        public bool IsComplited => isCompleted;
        public virtual void Calculate(){}

        public virtual void OnApply(){}

        public string GetText()
        {
            return Text;
        }
    }
}