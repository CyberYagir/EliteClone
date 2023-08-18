using UnityEngine;

namespace Core.PlayerScripts
{
    public class PlayerHead : Singleton<PlayerHead>
    {
        [SerializeField] private float sence;
        private Transform headcamera;
        private Player player;

        private void Awake()
        {
            Single(this);
        }

        private void Start()
        {
            player = PlayerDataManager.Instance.WorldHandler.ShipPlayer;
            headcamera = transform.GetChild(0);
        }

        void Update()
        {
            if (player.Control.headView)
            {
                LookHead();
            }
            else
            {
                HeadBack();
            }
        }

        public void HeadBack()
        {
            var localRotation = transform.localRotation;
            localRotation = Quaternion.Lerp(localRotation, Quaternion.identity, 10 * Time.deltaTime);
            transform.localRotation = localRotation;
            headcamera.localRotation = Quaternion.Lerp(localRotation, Quaternion.identity, 10 * Time.deltaTime);
        }
        public void LookHead()
        {
            var localRot = transform.localRotation;
            localRot *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sence * Time.deltaTime, 0);
            localRot = new Quaternion(localRot.x, Mathf.Clamp(localRot.y, -0.5f, 0.5f), localRot.z, localRot.w);
            transform.localRotation = localRot;

            var camRot = headcamera.localRotation;
            camRot *= Quaternion.Euler(-Input.GetAxis("Mouse Y") * sence * Time.deltaTime, 0, 0);
            camRot = new Quaternion(Mathf.Clamp(camRot.x, -0.5f, 0.5f), camRot.y, camRot.z, camRot.w);
            headcamera.localRotation = camRot;
        }
    }
}
