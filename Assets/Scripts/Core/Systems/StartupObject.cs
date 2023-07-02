using UnityEngine;

namespace Core
{
    public class StartupObject : MonoBehaviour
    {
        public virtual void Init(PlayerDataManager playerDataManager){}
        public virtual void Loop(){}
    }
}