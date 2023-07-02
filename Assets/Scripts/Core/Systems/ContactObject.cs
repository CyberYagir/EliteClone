using Core.PlayerScripts;

namespace Core.Systems
{
    public class ContactObject : GalaxyObject
    {
        public void Init(bool triggerEvent = true)
        {
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.targets.AddContact(this, triggerEvent);
            Player.OnPreSceneChanged += RemoveContact;
        }

        private void OnDisable()
        {
            RemoveContact();
        }

        public void RemoveContact()
        {
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.targets.RemoveContact(this);
        }
    }
}
