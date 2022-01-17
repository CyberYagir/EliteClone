using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Meteor : MonoBehaviour
{
    [SerializeField] private List<Mesh> meshes;
    [SerializeField] private List<Material> materials;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float maxMass, maxSize;
    private float mass;
    
    public void Init(Random rnd)
    {
        var percent = Mathf.Clamp((float)rnd.NextDouble(), 0.2f, 1);
        mass = maxMass * percent;
        transform.localScale = Vector3.one * (maxSize * percent);
        var mesh = meshes[rnd.Next(0, meshes.Count)];
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = materials[rnd.Next(0, materials.Count)];

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}