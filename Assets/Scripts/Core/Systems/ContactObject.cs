using Core.PlayerScripts;

namespace Core.Systems
{
    public class ContactObject : GalaxyObject
    {
        public void Init(bool triggerEvent = true)
        {
            Player.inst.targets.AddContact(this, triggerEvent);
            Player.OnPreSceneChanged += RemoveContact;
        }

        private void OnDisable()
        {
            RemoveContact();
        }

        public void RemoveContact()
        {
            Player.inst.targets.RemoveContact(this);
        }
    }
}
