using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Galaxy;
using UnityEngine;

namespace Core.Map
{
    public class GalacticPathfinder : MonoBehaviour
    {
        [SerializeField] private List<string> findedTargets = new List<string>();
        public void FindPathFrom(List<string> path, SolarSystem system, string target, Action<List<string>> OnComplete)
        {
            StartCoroutine(Find(path, system, target, OnComplete));
        }

        public void ClearTargets()
        {
            findedTargets.Clear();
            StopAllCoroutines();
        }
        
        private IEnumerator Find(List<string> path, SolarSystem system, string target, Action<List<string>> OnComplete)
        {
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
                        StartCoroutine(Find(new List<string>(path), GalaxyGenerator.systems[sorted[i].solarName], target, OnComplete));
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
                }
            }
        }
    }
}