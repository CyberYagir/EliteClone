using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.TDS.Bot
{
    public class ShooterBotDrop : MonoBehaviour
    {
        [SerializeField] private List<Transform> drop;

        private bool droped;

        public void SpawnDrop()
        {
            if (!droped)
            {
                var id = Random.Range(0, drop.Count);
                var n = Instantiate(drop[id]).GetComponent<Rigidbody>();
                n.transform.position = transform.position + Vector3.up*2;
                n.AddForce(200*(Random.insideUnitSphere));
                n.AddForce(Vector3.up * 5);
                droped = true;
            }
        }
    }
}
