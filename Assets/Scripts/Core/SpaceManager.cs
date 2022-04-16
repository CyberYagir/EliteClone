using Core.PlayerScripts;
using UnityEngine;

namespace Core
{
    public class SpaceManager : MonoBehaviour
    {
        void Start()
        {
            Player.ChangeScene(); //Есть на каждой локе чтобы триггерить эвент смены локации
        }
    }
}
