using Core.PlayerScripts;
using Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Systems
{
    public class FloatingPoint : StartupObject
    {
        [SerializeField] private float threshold = 10000.0f;
        [SerializeField] private float cooldown = 2;
        [SerializeField] private int clamp = 100000;

        private WorldDataHandler worldDataHandler;
        
        
        public override void Init(PlayerDataManager playerDataManager)
        {
            base.Init(playerDataManager);
            worldDataHandler = playerDataManager.WorldHandler;
        }

        public override void Loop()
        {
            base.Loop();
            
            if (worldDataHandler.ShipPlayer)
            {
                Vector3 cameraPos = worldDataHandler.ShipPlayer.transform.position;
                if (cameraPos.magnitude > threshold)
                {
                    foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
                    {
                        go.transform.position -= cameraPos;
                    }
                    foreach (var wsp in SolarStaticBuilder.Objects)
                    {
                        wsp.UpdateVisibility();
                    }
                    WorldSpaceObjectCanvas.Instance.SkipFrame();
                }

                transform.position = 
                    new Vector3(
                        Mathf.Clamp(transform.position.x, -clamp, clamp),
                        Mathf.Clamp(transform.position.y, -clamp, clamp), 
                        Mathf.Clamp(transform.position.z, -clamp, clamp));
            }
        }
    }
}