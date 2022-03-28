using UnityEngine;

namespace Core.Systems
{
    public class PosToPlayerPos : MonoBehaviour
    {
        public void Update()
        {
            transform.position = Player.Player.inst.transform.position;
        }
    }
}