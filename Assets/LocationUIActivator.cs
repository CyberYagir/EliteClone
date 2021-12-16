using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationUIActivator : MonoBehaviour
{
    [SerializeField] private GameObject baseUI;
    void Start()
    {
        Player.OnSceneChanged += () => { baseUI.SetActive(World.Scene == Scenes.Location); };
    }
}
