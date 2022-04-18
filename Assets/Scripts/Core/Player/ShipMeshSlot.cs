using System;
using System.Collections.Generic;
using Core.Game;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.PlayerScripts
{
    public class ShipMeshSlot : MonoBehaviour
    {
        [Serializable]
        public class SlotMesh
        {
            public GameObject mesh;
            public Object weaponScript;
            public WeaponOptionsItem options;
            public string weaponScriptName = "Core.Player.Weapon.";
        }
    
        public int slotID;
        [SerializeField] private List<SlotMesh> meshes;
        private Weapon.Weapon currentWeapon;

        public void SetMesh(Slot slot)
        {
            Destroy(currentWeapon);
            if (slot.current.itemType == ItemType.Weapon)
            {
                Destroy(currentWeapon);
                gameObject.SetActive(true);

                var meshID = int.Parse(slot.current.GetKeyPair(KeyPairValue.Value).ToString());
                for (var i = 0; i < meshes.Count; i++)
                {
                    if (meshID == i)
                    {
                        var mesh = meshes[i];
                        if (mesh.weaponScript != null)
                        {
                            currentWeapon = (Weapon.Weapon) gameObject.AddComponent(Type.GetType(mesh.weaponScriptName + mesh.weaponScript.name));
                            currentWeapon.Init(slot.button, slot.current, mesh.options);
                        }
                    }
                    meshes[i].mesh.SetActive(meshID == i);
                }

            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
