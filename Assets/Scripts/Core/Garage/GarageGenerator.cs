using UnityEngine;

namespace Core.Garage
{
    public class GarageGenerator : MonoBehaviour
    {
        [SerializeField] private GarageDataCollect dataCollect;
        private void Start()
        {
            dataCollect.InitDataCollector();
        }
    }
}
