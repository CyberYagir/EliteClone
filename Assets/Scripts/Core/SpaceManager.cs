using UnityEngine;

namespace Core
{
    public class SpaceManager : MonoBehaviour
    {
        void Start()
        {
            Player.Player.ChangeScene(); //Есть на каждой локе чтобы триггерить эвент смены локации
        }
    }
}
