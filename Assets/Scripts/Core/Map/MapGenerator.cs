
using System.Collections.Generic;
using Core.Galaxy;
using Core.Systems;
using DG.Tweening;
using UnityEngine;

namespace Core.Map
{
    public class MapGenerator : MonoBehaviour
    {
        public static bool Set;
        public static MapGenerator Instance;
        public const float size = 2000;
        [SerializeField] private Camera camera;
        [SerializeField] private GameObject star;
        public static GameObject selected;

        public List<GameObject> spawned = new List<GameObject>();
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Player.Player.OnSceneChanged += Reload;
            Reload();
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

                var spawn = Instantiate(star.gameObject, system.position.ToVector()/size, Quaternion.identity);

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
                
                spawned.Add(spawn.gameObject);
            }
        }

        public void ChangeSelected(GameObject select)
        {
            selected = select;
            camera.transform.DOMove((select.transform.position) + new Vector3(0, 5, -5), 1);
        }
    }
}
