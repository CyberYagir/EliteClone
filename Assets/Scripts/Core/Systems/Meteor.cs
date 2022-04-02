using System;
using System.Collections.Generic;
using Core.Game;
using Core.Player;
using Core.UI;
using UnityEngine;
using Random = System.Random;

namespace Core.Systems
{
    public class Meteor : MonoBehaviour, IDamagable
    {
        [SerializeField] private List<Mesh> meshes;
        [SerializeField] private List<Mesh> colliders;
        [SerializeField] private List<Material> materials;
        [SerializeField] private float maxMass, maxSize;
        [SerializeField] private GameObject mineralDrop;
        [SerializeField] private AnimationCurve sizePercent;
        private Item resource;
        private float mass;
        private Damager damager;
        private ValueLimit heath;

        public void Init(Random rnd, Item res)
        {
            damager = GetComponent<Damager>();
            var percent = sizePercent.Evaluate((float) rnd.NextDouble());
            mass = maxMass * percent;
            transform.localScale = Vector3.one * (maxSize * percent);
            var id = rnd.Next(0, meshes.Count);
            var mesh = meshes[id];
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshRenderer>().material = materials[rnd.Next(0, materials.Count)];
            resource = res;
            GetComponent<MeshCollider>().sharedMesh = colliders[id];
            heath = new ValueLimit();
            heath.SetClamp(0, (int) mass);
            heath.SetValue(heath.Max);
            if (resource.IsHaveKeyPair(KeyPairValue.Value))
            {
                var r = GetComponent<Renderer>();
                var mats = new List<Material>();
                r.GetMaterials(mats);
                mats.Add((Material)resource.GetKeyPair(KeyPairValue.Value));
                r.materials = mats.ToArray();
                GetComponent<MeshCollider>().sharedMesh = mesh;
            }

            var displayData = GetComponent<DataToDisplay>();
            displayData.AddLabel("Object", "Asteroid");
            displayData.AddLabel("Resource", resource.itemName);
            displayData.AddLabel("Mass", mass.ToString());
        }

        public void TakeDamage(float damage)
        {
            damager.TakeDamage(ref heath.GetValue(), damage);
        }

        public float GetHealth()
        {
            return heath.Value;
        }

        public void SpawnDrop(Vector3 normal, Vector3 point)
        {
            if (new System.Random().Next(1, 50) == 5)
            {
                var item = Instantiate(mineralDrop, point, Quaternion.identity);
                var drop = item.GetComponent<WorldDrop>();
                var itemData = resource.Clone();
                itemData.amount.SetValue(new System.Random(DateTime.Now.Millisecond + DateTime.Now.Second).Next(10, 25));
                drop.Init(itemData);
                item.GetComponent<Rigidbody>().AddForce((normal + UnityEngine.Random.insideUnitSphere) * 0.5f, ForceMode.Impulse);
            }
        }
    }
}
    