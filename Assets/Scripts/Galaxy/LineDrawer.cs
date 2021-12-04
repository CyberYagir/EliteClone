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
    private List<GalaxyPoint> oldStars = new List<GalaxyPoint>();
    private List<GalaxyLine> linesPool = new List<GalaxyLine>(), activeLines = new List<GalaxyLine>();

    private const float updateTime = 0.05f;

    private Vector3 oldCameraPos;
    private void Awake()
    {
        instance = this;
        oldCameraPos = transform.position;

        for (int i = 0; i < 300; i++)
        {
            var line = Instantiate(linePrefab, holder.transform).GetComponent<GalaxyLine>();
            linesPool.Add(line);
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
        if (Vector3.Distance(oldCameraPos, transform.position) > 5)
        {
            var cast = Physics.SphereCastAll(transform.position + transform.forward * 50, 100, Vector3.down, 100f);
            if (cast.Length > 1)
            {
                stars = new List<GalaxyPoint>();
                for (int i = 0; i < cast.Length; i++)
                {
                    var g = cast[i].transform.GetComponent<GalaxyPoint>();
                    oldStars.Remove(g);
                    stars.Add(g);
                    stars[stars.Count - 1].particles.Play();
                }
            }
            else
            {
                stars = new List<GalaxyPoint>();
            }
            oldCameraPos = transform.position;
            foreach (var item in oldStars)
            {
                if (GalaxyManager.selectedPoint != item)
                    item.particles.Stop();
            }
            oldStars = stars;
            UpdateLines();
        }
    }

    public void UpdateLines()
    {
        time = 0;
        foreach (var item in activeLines)
        {
            linesPool.Add(item);
            item.gameObject.SetActive(false);
        }
        linesPool = linesPool.OrderBy(x => x.transform.name).ToList();
        activeLines = new List<GalaxyLine>();
        foreach (var s1 in stars)
        {
            foreach (var sibling in s1.solarSystem.sibligs)
            {
                if (linesPool.Count != 0)
                {
                    var mainpos = s1.solarSystem.position.ToVector() / GalaxyGenerator.scale;
                    var secondpos = sibling.position.ToVector() / GalaxyGenerator.scale;
                    linesPool[0].Init(mainpos , Vector3.Lerp(mainpos, secondpos, 0.5f), secondpos, s1);
                    
                    activeLines.Add(linesPool[0]);
                    linesPool.RemoveAt(0);
                }
            }
        }
    }
}


