using UnityEngine;

namespace Core.PlayerScripts
{
    public class ShipMeshSlotRotate : MonoBehaviour
    {
        private Quaternion startRotation;
        private Weapon.Weapon weapon;
        private void Awake()
        {
            startRotation = transform.localRotation;
        }

        private void Start()
        {
            weapon = GetComponent<Weapon.Weapon>();
        }

        private void Update()
        {
            Rotate();
        }

        public void LateUpdate()
        {
            Rotate();
        }

        public void Rotate()
        {
            var player = PlayerDataManager.Instance.WorldHandler.ShipPlayer;
            if (player)
            {
                var target = player.GetTarget();
                if (target && target.GetComponent<IDamagable>() != null && Vector3.Distance(transform.position, target.transform.position) < weapon.GetDistance())
                {
                    Quaternion targetRotation = Quaternion.identity;
                    var dir = target.transform.position - player.transform.position;
                    if (Vector3.Dot(player.transform.forward, dir) > 0)
                    {
                        var ang = Vector3.Angle(player.transform.forward, dir);
                        if (ang < 30)
                        {
                            targetRotation = Quaternion.LookRotation(dir);
                            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 20 * Time.deltaTime);
                        }
                        else
                        {
                            RotateLocal();
                        }
                    }
                    else
                    {
                        RotateLocal();
                    }
                }
                else
                {
                    RotateLocal();
                }
            }
        }

        public void RotateLocal()
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, startRotation, 20 * Time.deltaTime);
        }
    }
}
