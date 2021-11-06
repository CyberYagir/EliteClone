using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Paths
{
    public GalaxyPoint p1, p2;
}

public class LineDrawer : MonoBehaviour
{
    public static LineDrawer instance;

    public bool updateRequire;

    public float time = 0;
    public float dist = 2500;
    Vector3 oldPos;
    private void Awake()
    {
        instance = this;
        oldPos = transform.position;
    }

    public List<GalaxyPoint> stars = new List<GalaxyPoint>();
    [SerializeField] List<GalaxyLine> lines, activeLines;

    const float updateTime = 0.05f;
    private void Update()
    {

        

        time += Time.deltaTime;
        if (time > updateTime)
        {
            if (Vector3.Distance(oldPos, transform.position) > 5)
            {
                var cast = Physics.SphereCastAll(transform.position + transform.forward * 127f, 135f, Vector3.down, Mathf.Infinity);
                if (cast.Length > 1)
                {
                    stars = new List<GalaxyPoint>();
                    for (int i = 0; i < cast.Length; i++)
                    {
                        stars.Add(cast[i].transform.GetComponent<GalaxyPoint>());
                        if (stars[stars.Count - 1].particles.particleCount == 0)
                        {
                            stars[stars.Count - 1].particles.Play();
                        }
                    }
                }
                oldPos = transform.position;
                UpdateLines();
            }
        }
    }

    public void UpdateLines()
    {
        time = 0;
        List<Paths> paths = new List<Paths>();
        foreach (var item in activeLines)
        {
            lines.Add(item);
            item.gameObject.SetActive(false);
        }
        lines = lines.OrderBy(x => x.transform.name).ToList();
        activeLines = new List<GalaxyLine>();
        foreach (var s1 in stars)
        {
            foreach (var s2 in stars)
            {
                if (s1 != s2 && paths.Find(x=>x.p1 == s1 && x.p2 == s2) == null)
                {
                    if (s1.solarSystem.position.Dist(s2.solarSystem.position) < (decimal)dist)
                    {
                        if (lines.Count > 0)
                        {
                            var line = lines[0];
                            activeLines.Add(line);
                            lines.RemoveAt(0);

                            line.Init(s1.transform.position, Vector3.Lerp(s1.transform.position, s2.transform.position, 0.5f), s2.transform.position);
                            line.gameObject.SetActive(true);

                            paths.Add(new Paths() { p1 = s1, p2 = s2 });
                        }
                    }
                }
            }
        }
    }
}


