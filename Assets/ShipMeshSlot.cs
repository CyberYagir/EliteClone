using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

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
    }
    
    public int slotID;
    [SerializeField] private List<SlotMesh> meshes;


    public void SetMesh(Slot slot)
    {
        if (slot.current.IsHaveKeyPair(KeyPairValue.MeshType))
        {
            gameObject.SetActive(true);
            var type = (float)slot.current.GetKeyPair(KeyPairValue.MeshType);
            foreach (var mesh in meshes)
            {
                if (mesh.meshName == (SlotMesh.WeaponMesh)(int)type)
                {
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
