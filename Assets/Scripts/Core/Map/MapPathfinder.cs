using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Galaxy;
using UnityEngine;

namespace Core.Map
{
    public class MapPathfinder : Singleton<MapPathfinder>
    {
        [SerializeField] private List<string> path = new List<string>();
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private Material pathMat;
        [SerializeField] private AnimationCurve selectedWidth;
        public bool skipFrames = true;
        private List<GameObject> lines = new List<GameObject>();


        
        private bool isEnd = false;

        public string start { get; private set; }
        public string end { get; private set; }


        public void SetStartPath(string start)
        {
            this.start = start;
        }

        public void SetEndPath(string end)
        {
            this.end = end;
        }
        
        private void Awake()
        {
            Single(this);
        }


       

        public void FindPath(string first, string second)
        {
            start = first;
            end = second;
            FindPath();
        }

        public void FindPath()
        {
            if (start == end) return;
            isEnd = false;
            path.Clear();
            var startSystem = GalaxyGenerator.systems[start];
            StartCoroutine(FindAsync(path, startSystem, end));
        }



        public void DrawLines()
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Destroy(lines[i].gameObject);
            }

            lines.Clear();

            for (int i = 0; i < path.Count - 1; i++)
            {
                var l = Instantiate(linePrefab.gameObject).GetComponent<LineRenderer>();
                var system = GalaxyGenerator.systems[path[i]];
                MapGenerator.DrawLine(l, GameObject.Find(path[i + 1]).transform, new NeighbourSolarSytem() {position = system.position, solarName = path[i]});
                l.sortingOrder = 10;
                l.startColor = Color.cyan;
                l.material = pathMat;
                l.widthCurve = selectedWidth;
                l.widthMultiplier = 0.2f;
                lines.Add(l.gameObject);
            }
        }

        public void Clear()
        {
            start = "";
            end = "";
            foreach (var line in lines)
            {
                Destroy(line.gameObject);
            }

            lines = new List<GameObject>();
        }

        IEnumerator FindAsync(List<string> prevSteps, SolarSystem system, string target)
        {
            if (isEnd) yield break;
            var from = prevSteps.Count != 0 ? prevSteps[prevSteps.Count - 1] : "";
            List<string> nextSteps = prevSteps;
            nextSteps.Add(system.name);

            if (system.sibligs.FindAll(x => x.solarName == system.name).Count == 0 && system.name != target)
            {
                var sorted = system.sibligs.ToList().OrderBy(x => x.solarName == target).Reverse().ToList();
                for (int i = 0; i < sorted.Count; i++)
                {
                    if (from != sorted[i].solarName &&
                        MapGenerator.Instance.systems.Contains(sorted[i].solarName) &&
                        !nextSteps.Contains(sorted[i].solarName))
                    {
                        StartCoroutine(FindAsync(nextSteps, GalaxyGenerator.systems[sorted[i].solarName], target));
                    }
                }

                if (skipFrames)
                    yield return null;
            }
            else
            {
                isEnd = true;
                path = nextSteps;
                StopAllCoroutines();
                DrawLines();
                yield break;
            }

            if (skipFrames)
                yield return null;
        }

    }
}
