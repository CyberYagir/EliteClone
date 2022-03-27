using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    void Start()
    {
        Player.ChangeScene(); //Есть на каждой локе чтобы триггерить эвент смены локации
    }
}
