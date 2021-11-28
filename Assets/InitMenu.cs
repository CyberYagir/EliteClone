using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InitMenu : MonoBehaviour
{
    public DOTweenAnimation menuback;
    public DOTweenAnimation loading;
    
    public void Play()
    {
        menuback.DOPlayForward();
        loading.DOPlayForward();
        PlayerDataManager.instance.LoadScene();
    }
}
