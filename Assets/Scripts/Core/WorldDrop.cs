using Core.Game;
using Core.PlayerScripts;
using Core.Systems;
using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class WorldDrop : MonoBehaviour
    {
        [SerializeField] private Item item;
        private float cooldown;
        private WorldDataHandler worldDataHandler;

        public void Init(Item dropped, WorldDataHandler worldDataHandler, float delay = 0)
        {
            this.worldDataHandler = worldDataHandler;
            
            
            item = dropped;
            cooldown = delay;
            GetComponent<BoxCollider>().enabled = true;
            transform.name = "Storage: " + item.itemName + $" [{item.amount.value}]";
            GetComponent<ContactObject>().Init();
        }

        private void Update()
        {
            cooldown -= Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (cooldown <= 0)
            {
                if (other.GetComponent<ShipMeshManager>())
                {
                    if (worldDataHandler.ShipPlayer.cargo.AddItem(item))
                    {
                        transform.DOMove(other.transform.position, 0.7f);
                        transform.DOScale(Vector3.zero, 0.4f);
                        foreach (var col in GetComponents<Collider>())
                        {
                            col.enabled = false;
                        }

                        Destroy(gameObject, 1);
                        Destroy(this);
                    }
                }
            }
        }
    }
}
