using Core.PlayerScripts;
using UnityEngine;

namespace Core.Systems
{
    public class PosToPlayerPos : MonoBehaviour
    {
        public void Update()
        {
            if (Player.inst)
            {
                transform.position = Player.inst.transform.position;
            }
        }
    }
}