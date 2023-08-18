using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.PlayerScripts;
using Core.Systems;
using UnityEngine;

namespace Core.UI
{
    public class TargetManager : MonoBehaviour
    {
        public GalaxyObject target { get; private set; }
        public List<ContactObject> contacts { get; private set; } = new List<ContactObject>();

        public Event ContactsChanges = new Event();
        public Event OnChangeTarget = new Event();

        private Camera camera;

        private string lastTarget;
        
        private void Start()
        {
            camera = PlayerDataManager.Instance.WorldHandler.ShipPlayer.GetCamera();
            Player.OnPreSceneChanged += SaveLastSelected;
            Player.OnSceneChanged += LoadLastSelected;
        }

        public void SaveLastSelected()
        {
            if (target != null)
            {
                lastTarget = target.transform.name;
            }
            else
            {
                lastTarget = null;
            }
        }

        public void LoadLastSelected()
        {
            if (lastTarget != null)
            {
                SetTarget(GameObject.Find(lastTarget)?.GetComponent<GalaxyObject>());
                OnChangeTarget.Run();
            }
        }
        
        private void Update()
        {
            if (InputService.GetAxisDown(KAction.SetTarget))
            {
                if (target == null)
                {
                    var worldBody = TargetFromArray(SolarStaticBuilder.Objects.Cast<GalaxyObject>().ToList());
                    var contactBody = TargetFromArray(contacts.Cast<GalaxyObject>().ToList());
                    if (worldBody == null && contactBody == null)
                    {
                        SetTarget(null);
                    }
                    else if (worldBody != null && contactBody != null)
                    {
                        if (Vector3.Distance(transform.position, worldBody.transform.position * SolarStaticBuilder.GalaxyScale) < Vector3.Distance(transform.position, contactBody.transform.position))
                        {
                            SetTarget(worldBody);
                        }
                        else
                        {
                            SetTarget(contactBody);
                        }
                    }
                    else if (worldBody != null)
                    {
                        SetTarget(worldBody);
                    }
                    else if (contactBody != null)
                    {
                        SetTarget(contactBody);
                    }
                }
                else
                {
                    SetTarget(null);
                }
            }
        }

        public GalaxyObject TargetFromArray(List<GalaxyObject> objectsList)
        {
            int id = -1;
            float angle = 9999;
            for (int i = 0; i < objectsList.Count; i++)
            {
                if (objectsList[i] is WorldSpaceObject)
                {
                    (objectsList[i] as WorldSpaceObject).UpdateVisibility();
                }
                if (Vector3.Dot(camera.transform.forward, objectsList[i].transform.position - camera.transform.position) > 0)
                {
                    var ang = Vector3.Angle(objectsList[i].transform.position - camera.transform.position, camera.transform.forward);
                    if (ang < angle)
                    {
                        angle = ang;
                        id = i;
                    }
                }
            }

            if (angle < 5)
            {
                return objectsList[id];
            }

            return null;
        }

        public void SetTarget(GalaxyObject target)
        {
            this.target = target;
        }

        public void AddContact(ContactObject contact, bool trigger = true)
        {
            contacts.Add(contact);
            if (trigger)
                ContactsChanges.Run();
        }

        public void RemoveContact(ContactObject contact, bool trigger = true)
        {
            contacts.Remove(contact);
            if (trigger)
                ContactsChanges.Run();
        }

        public void UpdateContacts()
        {
            ContactsChanges.Run();
        }
    }
}
