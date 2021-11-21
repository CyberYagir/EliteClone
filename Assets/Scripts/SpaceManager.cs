using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Player.ChangeScene(); //Есть на каждой локе чтобы триггерить эвент смены локации
    }
}
