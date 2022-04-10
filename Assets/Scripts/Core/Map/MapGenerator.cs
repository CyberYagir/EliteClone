
using System;
using System.Collections.Generic;
using Core.Galaxy;
using Core.Systems;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core.Map
{
    public class MapGenerator : MonoBehaviour
    {
        public static bool Set;
        public static MapGenerator Instance;
        public const float size = 2000;
        [SerializeField] private Camera camera;
        [SerializeField] private GameObject star;
        [SerializeField] private GraphicRaycaster raycaster;
        [SerializeField] private GameObject galaxyLine;
        [SerializeField] private MapSelect selector;
        public static GameObject selected;
        public List<GameObject> spawned = new List<GameObject>();
        public List<GameObject> names = new List<GameObject>();
        public List<TMP_Text> texts = new List<TMP_Text>();
        private void Awake()
        {
            Instance = this;
            Reload();
            Set = true;
        }

        private void OnDestroy()
        {
            Set = false;
        }

        public void Reload()
        {
            foreach (var sp in spawned)
            {
                Destroy(sp.gameObject);
            }

            GalaxyGenerator.LoadSystems();
            Player.Player.inst.saves.LoadData();
            var historyList = Player.Player.inst.saves.GetHistory();
            foreach (var history in historyList)
            {
                var system = GalaxyGenerator.systems[history.Split('.')[0]];

                var spawn = Instantiate(star.gameObject, system.position.ToVector() / size, Quaternion.identity);

                for (int i = 0; i < system.sibligs.Count; i++)
                {
                    var line = Instantiate(galaxyLine).GetComponent<LineRenderer>();
                    Destroy(line.GetComponent<GalaxyLine>());
                    line.gameObject.layer = LayerMask.NameToLayer("Map");
                    var firstPos = system.position.ToVector() / size;
                    var secondPos = GalaxyGenerator.systems[system.sibligs[i].solarName].position.ToVector() / size;
                    line.SetPosition(0, firstPos);
                    line.SetPosition(2, secondPos);
                    line.SetPosition(1, Vector3.Lerp(firstPos, secondPos, 0.5f));
                    line.widthMultiplier = 0.1f;
                }
                
                var saved = SolarSystemGenerator.Load();
                var name = saved.systemName.Split('.')[0];
                if (system.name == name)
                {
                    camera.transform.position = (system.position.ToVector() / size) + new Vector3(0, 5, -5);
                    selected = spawn.gameObject;
                }

                spawn.transform.parent = transform;
                spawn.GetComponentInChildren<Renderer>().material.color += new Color(Random.value, Random.value, Random.value) / 5;
                spawn.transform.name = system.name;
                spawn.SetActive(true);

                var text = spawn.GetComponentInChildren<TMP_Text>(true);
                var textParent = text.transform.parent;

                text.text = system.name;
                textParent.parent = raycaster.transform;
                textParent.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                textParent.localRotation = Quaternion.identity;

                names.Add(textParent.gameObject);
                spawned.Add(spawn.gameObject);
                texts.Add(text);
            }
            
            for (int i = 0; i < names.Count; i++)
            {
                names[i].transform.localPosition += new Vector3(60, 80);
                names[i].transform.localEulerAngles += new Vector3(0, 0, -Vector2.Distance(names[i].transform.position, camera.transform.position));
                names[i].transform.position = selector.CalcPos(spawned[i].transform.position);
                names[i].transform.localScale = Vector3.one * (Mathf.Abs((Vector2.Distance(names[i].transform.position, camera.transform.position) / 40))) * 0.2f;
                texts[i].fontSize = 30 - Mathf.Abs((Vector2.Distance(names[i].transform.position, camera.transform.position) / 40));
            }


            
        }

        public void ChangeSelected(GameObject select)
        {
            selected = select;
            camera.transform.DOMove((select.transform.position) + new Vector3(0, 5, -5), 1);
        }
    }
}
