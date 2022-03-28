namespace Core.Systems
{
    public class ContactObject : GalaxyObject
    {
        public void Init(bool triggerEvent = true)
        {
            Player.Player.inst.targets.AddContact(this, triggerEvent);
            Player.Player.OnPreSceneChanged += RemoveContact;
        }

        private void OnDisable()
        {
            RemoveContact();
        }

        public void RemoveContact()
        {
            Player.Player.inst.targets.RemoveContact(this);
        }
    }
}
