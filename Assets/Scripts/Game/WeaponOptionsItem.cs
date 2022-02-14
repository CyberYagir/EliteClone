using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "", menuName = "Game/Weapon Options", order = 1)]
public class WeaponOptionsItem : ScriptableObject
{
    [System.Serializable]
    public class LaserOptions
    {
        public AnimationCurve width;
        public Material materal;
    }

    public LaserOptions laserOptions;
    public float maxDistance;
    public GameObject attackParticles;
    public GameObject attackDecal;
    public float damage;
}
