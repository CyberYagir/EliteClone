using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InitMenu : MonoBehaviour
{
    [SerializeField] private DOTweenAnimation menuback;
    [SerializeField] private DOTweenAnimation loading;
    
    public void Play()
    {
        menuback.DOPlayForward();
        loading.DOPlayForward();
        PlayerDataManager.Instance.LoadScene();
    }
}
