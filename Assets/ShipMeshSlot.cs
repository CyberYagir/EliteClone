using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using Object = UnityEngine.Object;

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
    }
    
    public int slotID;
    [SerializeField] private List<SlotMesh> meshes;
    private Weapon currentWeapon;

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
                        currentWeapon = (Weapon)gameObject.AddComponent(Type.GetType(mesh.weaponScript.name));
                        currentWeapon.Init(slot.button, slot.current);
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
