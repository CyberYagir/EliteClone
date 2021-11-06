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

    public GameObject linePrefab, holder;
    public bool updateRequire;

    public float time = 0;
    public float dist = 2500;
    Vector3 oldPos;
    private void Awake()
    {
        instance = this;
        oldPos = transform.position;

        for (int i = 0; i < 300; i++)
        {
            var line = Instantiate(linePrefab, holder.transform).GetComponent<GalaxyLine>();
            lines.Add(line);
            line.gameObject.SetActive(false);

        }
    }

    public List<GalaxyPoint> stars = new List<GalaxyPoint>();
    List<GalaxyPoint> old = new List<GalaxyPoint>();
    List<GalaxyLine> lines = new List<GalaxyLine>(), activeLines = new List<GalaxyLine>();

    const float updateTime = 0.05f;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > updateTime)
        {
            if (Vector3.Distance(oldPos, transform.position) > 5)
            {
                var cast = Physics.SphereCastAll(transform.position + transform.forward * 50, 100, Vector3.down, 100f);
                if (cast.Length > 1)
                {
                    stars = new List<GalaxyPoint>();
                    for (int i = 0; i < cast.Length; i++)
                    {
                        var g = cast[i].transform.GetComponent<GalaxyPoint>();
                        old.Remove(g);
                        stars.Add(g);
                        stars[stars.Count - 1].particles.Play();
                    }
                }
                else
                {
                    stars = new List<GalaxyPoint>();
                }
                oldPos = transform.position;
                foreach (var item in old)
                {
                    if (GalaxyManager.selectedPoint != item)
                        item.particles.Stop();
                }
                old = stars;
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
                            line.p1 = s1;
                            line.p2 = s2;
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


