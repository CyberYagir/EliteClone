using Core.PlayerScripts;

namespace Core.Systems
{
    public class ContactObject : GalaxyObject
    {
        public void Init(bool triggerEvent = true)
        {
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.TargetsManager.AddContact(this, triggerEvent);
            Player.OnPreSceneChanged += RemoveContact;
        }

        private void OnDisable()
        {
            RemoveContact();
        }

        public void RemoveContact()
        {
            var playerShip = PlayerDataManager.Instance.WorldHandler.ShipPlayer;
            if (playerShip != null)
            {
                playerShip.TargetsManager.RemoveContact(this);
            }
        }
    }
}
