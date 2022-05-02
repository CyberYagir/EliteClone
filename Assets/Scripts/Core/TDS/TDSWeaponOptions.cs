using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Core.TDS
{
    [CreateAssetMenu(menuName = "Game/TDSWeaponOptions", fileName = "TDSWeaponOptions", order = 0)]
    public class TDSWeaponOptions: ScriptableObject
    {
        [System.Serializable]
        public class IKPointPos
        {
            public Vector3 pos, rot;

            public void Set(Transform obj)
            {
                obj.localPosition = pos;
                obj.localEulerAngles = rot;
                obj.GetComponentInParent<ChainIKConstraint>().weight = 1;
            }
        }
        public GameObject bullet;
        public IKPointPos leftH, rightH;
        
    }
}