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

    public List<GalaxyPoint> stars = new List<GalaxyPoint>();
    private List<GalaxyPoint> old = new List<GalaxyPoint>();
    private List<GalaxyLine> lines = new List<GalaxyLine>(), activeLines = new List<GalaxyLine>();

    private const float updateTime = 0.05f;

    private Vector3 oldPos;
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

   

    private void Update()
    {
        time += Time.deltaTime;
        if (time > updateTime)
        {
            CollectLines();
        }
    }

    public void CollectLines()
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

    public void UpdateLines()
    {
        time = 0;
        foreach (var item in activeLines)
        {
            lines.Add(item);
            item.gameObject.SetActive(false);
        }
        lines = lines.OrderBy(x => x.transform.name).ToList();
        activeLines = new List<GalaxyLine>();
        foreach (var s1 in stars)
        {
            foreach (var sibling in s1.solarSystem.sibligs)
            {
                if (lines.Count != 0)
                {
                    var mainpos = s1.solarSystem.position.toVector() / GalaxyGenerator.scale;
                    var secondpos = sibling.position.toVector() / GalaxyGenerator.scale;
                    lines[0].Init(mainpos , Vector3.Lerp(mainpos, secondpos, 0.5f), secondpos, s1);
                    
                    activeLines.Add(lines[0]);
                    lines.RemoveAt(0);
                }
            }
        }
    }
}


