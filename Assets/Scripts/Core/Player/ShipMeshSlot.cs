using System;
using System.Collections.Generic;
using Core.Game;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Player
{
    public class ShipMeshSlot : MonoBehaviour
    {
        [System.Serializable]
        public class SlotMesh
        {
            public enum WeaponMesh
            {
                None, Laser, MineLaser
            }
            public WeaponMesh meshName;
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
            if (slot.current.IsHaveKeyPair(KeyPairValue.MeshType))
            {
                Destroy(currentWeapon);
                gameObject.SetActive(true);
                var type = (float)slot.current.GetKeyPair(KeyPairValue.MeshType);
                foreach (var mesh in meshes)
                {
                    if (mesh.meshName == (SlotMesh.WeaponMesh) (int) type)
                    {
                        if (mesh.weaponScript != null)
                        {
                            currentWeapon = (Weapon.Weapon)gameObject.AddComponent(Type.GetType(mesh.weaponScriptName + mesh.weaponScript.name));
                            currentWeapon.Init(slot.button, slot.current, mesh.options);
                        }

                        mesh.mesh.SetActive(true);
                    }
                    else
                    {
                        mesh.mesh.SetActive(false);
                    }
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
