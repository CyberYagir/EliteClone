using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Galaxy;
using UnityEngine;

namespace Core.Map
{
    public class GalacticPathfinder
    {
        [SerializeField] private List<string> findedTargets = new List<string>();
        private IEnumerator ienumerator;
        private List<IEnumerator> list = new List<IEnumerator>();
        private MonoBehaviour starter;
        private bool isCompleted = false;
        public GalacticPathfinder(MonoBehaviour beh ,List<string> path, SolarSystem system, string target, Action<List<string>> OnComplete)
        {
            starter = beh;
            ienumerator = Find(path, system, target, OnComplete);
            beh.StartCoroutine(ienumerator);
        }

        public void ClearTargets()
        {
            findedTargets.Clear();
            starter.StopCoroutine(ienumerator);
            foreach (var l in list)
            {
                starter.StopCoroutine(l);
            }
            isCompleted = false;
        }
        
        private IEnumerator Find(List<string> path, SolarSystem system, string target, Action<List<string>> OnComplete)
        {
            if (isCompleted) yield break;
            
            var from = system.name;
            path.Add(system.name);

            if (!findedTargets.Contains(target) && system.name != target && system.sibligs.FindAll(x => x.solarName == system.name).Count == 0)
            {
                var sorted = system.sibligs.ToList().OrderBy(x => x.solarName == target).Reverse().ToList();
                for (int i = 0; i < sorted.Count; i++)
                {
                    if (from != sorted[i].solarName &&
                        !path.Contains(sorted[i].solarName))
                    {
                        var ienum = Find(new List<string>(path), GalaxyGenerator.systems[sorted[i].solarName], target, OnComplete);
                        list.Add(ienum);
                        starter.StartCoroutine(ienum);
                    }

                    yield return null;
                }
            }
            else
            {
                if (!findedTargets.Contains(target))
                {
                    findedTargets.Add(target);
                    OnComplete.Invoke(path);
                    isCompleted = true;
                }
            }
        }
    }
}