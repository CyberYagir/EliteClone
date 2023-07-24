using System;
using System.Collections;
using System.Collections.Generic;
using Core.Garage;
using Core.PlayerScripts;
using UnityEngine;
using UnityEngine.Timeline;

namespace Core.CommunistsBase.OutDemo
{
    public class OutDemoManager : MonoBehaviour
    {
        [SerializeField] private AnimationClip firstDemoClip;
        [SerializeField] private Transform firstDemoObject;
        [SerializeField] private TimelineAsset secondDemo;
        [SerializeField] private Transform secondDemoObject;
        [SerializeField] private FaderMultiScenes fader;
        
        
        
        private void Start()
        {
            StartCoroutine(Animation());
        }

        IEnumerator Animation()
        {
            firstDemoObject.gameObject.SetActive(true);
            secondDemoObject.gameObject.SetActive(false);
            yield return new WaitForSeconds(firstDemoClip.length);
            firstDemoObject.gameObject.SetActive(false);
            secondDemoObject.gameObject.SetActive(true);
            yield return new WaitForSeconds((float)secondDemo.duration-1);
            fader.LoadScene(Scenes.System);

            var tutorManager = PlayerDataManager.Instance.Services.TutorialsManager;

            var tutor = tutorManager.TutorialData;
            tutor.ValuesData.AddWatchDemo(Demos.BaseDemo);
            tutor.TutorialQuestsData.NextTutorialQuest();
            
            tutorManager.SaveTutorial();
        }
    }
}
