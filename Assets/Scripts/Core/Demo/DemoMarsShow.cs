using UnityEngine;

namespace Core.Demo
{
    public class DemoMarsShow : MonoBehaviour
    {
        [SerializeField] private Demo demo;
        public void ShowMars()
        {
            demo.ShowMars();
            demo.StartCoroutine(demo.StartTime());
        }
    }
}
