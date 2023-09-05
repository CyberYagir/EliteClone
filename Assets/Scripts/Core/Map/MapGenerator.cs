
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Galaxy;
using Core.PlayerScripts;
using Core.Systems;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core.Map
{
    public class MapGenerator : Singleton<MapGenerator>
    {
        public static bool Set;
        public const float size = 1500;
        
        [SerializeField] private Camera camera;
        [SerializeField] private GameObject star;
        [SerializeField] private GraphicRaycaster raycaster;
        [SerializeField] private GameObject galaxyLine;
        [SerializeField] private MapSelect selector;
        [SerializeField] private GameObject dataManager;
        [SerializeField] private GameObject volume, exitCanvas;
        [SerializeField] private EventSystem eventSystem;
        private List<GameObject> spawned = new List<GameObject>();
        private List<GameObject> names = new List<GameObject>();
        private List<TMP_Text> texts = new List<TMP_Text>();

        public static MapSelect.MapMode mode;
        
        private SaveLoadData saves = null;

        public List<string> systems = new List<string>();
        public Event OnInited;
        private static readonly int EmissiveColor = Shader.PropertyToID("_EmissiveColor");


        private void Awake()
        {
            Single(this);
            Set = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        private void Start()
        {
            Init();
            Reload();
            OnInited.Run();
        }

        public void Init()
        {
            if (World.Scene == Scenes.Map || SceneManager.GetActiveScene().name == "Map")
            {
                mode = MapSelect.MapMode.Active;
                ActiveInit();
            }else
            {
                mode = MapSelect.MapMode.Frame;     
                FrameInit();
            }
        }


        public void ActiveInit()
        {
            World.SetScene(Scenes.Map);
            camera.targetTexture = null;
            camera.GetComponent<HDAdditionalCameraData>().clearColorMode = HDAdditionalCameraData.ClearColorMode.Sky;
            Destroy(camera.GetComponent<HDCameraUI>());
            exitCanvas.SetActive(true);
            if (PlayerDataManager.Instance == null)
            {
                var playerData = Instantiate(dataManager.gameObject).GetComponent<PlayerDataManager>();
            }
        }

        public void FrameInit()
        {
            StartCoroutine(WaitForDisableCamera());
            eventSystem.gameObject.SetActive(false);
            Destroy(volume.gameObject);
        }
        

        private void OnDestroy()
        {
            Set = false;
        }

        public void Clear()
        {
            foreach (var sp in spawned)
            {
                Destroy(sp.gameObject);
            }

            foreach (var t in names)
            {
                if (t != null)
                {
                    Destroy(t.gameObject);
                }   
            }

            names.Clear();
            spawned.Clear();
            spawned.Clear();
        }

        public List<string> GetHistory()
        {
            if (mode == MapSelect.MapMode.Frame && PlayerDataManager.Instance.WorldHandler.ShipPlayer)
            {
                saves = PlayerDataManager.Instance.WorldHandler.ShipPlayer.SaveData;
            }
            else
            {
                saves = gameObject.AddComponent<SaveLoadData>();
            }
            saves.LoadData();
            return saves.GetHistory();
        }

        public static LineRenderer DrawLine(LineRenderer line, Transform parent, NeighbourSolarSytem system)
        {
            Destroy(line.GetComponent<GalaxyLine>());
            line.transform.parent = parent;
            line.gameObject.layer = LayerMask.NameToLayer("Map");
            var firstPos = parent.position;
            var secondPos = GalaxyGenerator.systems[system.solarName].position.ToVector() / size;
            line.SetPosition(0, firstPos);
            line.SetPosition(2, secondPos);
            line.SetPosition(1, Vector3.Lerp(firstPos, secondPos, 0.5f));
            line.widthMultiplier = 0.1f;
            return line;
        }
        
        public void DrawWorld(List<string> historyList)
        {
            var worldStructures = PlayerDataManager.Instance.Services.WorldStructuresManager;
            
            foreach (var history in historyList)
            {
                var system = GalaxyGenerator.systems[history.Split('.')[0]];
                systems.Add(system.name);
                var spawn = Instantiate(star.gameObject, system.position.ToVector() / size, Quaternion.identity);

                if (worldStructures.IsHaveStructure(system.name, StructureNames.ComunistsBase))
                {
                    var render = spawn.GetComponentInChildren<Renderer>();
                    render.material.color = Color.red;
                    render.material.SetColor(EmissiveColor, new Color(0.5f, 0.074f, 0.074f));
                }

                for (int i = 0; i < system.sibligs.Count; i++)
                {
                    var line = Instantiate(galaxyLine).GetComponent<LineRenderer>();
                    DrawLine(line, spawn.transform, system.sibligs[i]);
                }

                var saved = SolarStaticBuilder.SystemLoad();
                var name = saved.systemName.Split('.')[0];
                if (system.name == name)
                {
                    camera.transform.position = (system.position.ToVector() / size) + new Vector3(0, 5, -5);
                    selector.ChangeSelected(spawn.gameObject);
                }

                spawn.transform.parent = transform;
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

   
        }
        
        public void DrawCanvas()
        {
            float maxSize = 2;
            Dictionary<int, float> ids = new Dictionary<int, float>();
            for (int i = 0; i < names.Count; i++)
            {
                var dirToTarget = Vector3.Normalize(spawned[i].transform.position - camera.transform.position);
                var dot = Vector3.Dot(camera.transform.forward, dirToTarget);
                var dist = Vector3.Distance(camera.transform.position, spawned[i].transform.position);
                if (dot > 0 && dist < 100)
                {
                    names[i].SetActive(true);
                }
                else
                {
                    names[i].SetActive(false);
                    continue;
                }
                var distToSelected = Vector2.Distance(MapSelect.selected.transform.position, spawned[i].transform.position);
                names[i].transform.localEulerAngles = new Vector3(0, 0, 25 + distToSelected);
                names[i].transform.position = selector.CalcPos(spawned[i].transform.position);
                ids.Add(i, dist);
                var scale = Vector3.one * maxSize / ((distToSelected / 2f) + 1);
                if (mode == MapSelect.MapMode.Frame)
                {
                    names[i].transform.localScale = scale;
                }
                else
                {
                    names[i].transform.localScale = Vector3.Lerp(names[i].transform.localScale, scale, Time.deltaTime * 20f);
                }
                texts[i].fontSize = 30 - (Vector2.Distance(MapSelect.selected.transform.position, camera.transform.position) / 40);
            }

            var dictionary = ids.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            foreach (var el in dictionary)
            {
                names[el.Key].transform.SetAsFirstSibling();
            }
        }
        public void Reload()
        {
            Clear();
            var historyList = GetHistory();
            GalaxyGenerator.LoadSystems();
            DrawWorld(historyList);
            DrawCanvas();
            selector.UpdatePoint();
        }


        private void Update()
        {
            if (mode == MapSelect.MapMode.Active)
            {
                DrawCanvas();
            }
        }

        IEnumerator WaitForDisableCamera()
        {
            if (SceneManager.GetActiveScene().name != "Map")
            {
                yield return null;
                DrawCanvas();//Skip Frames And Wait For Canvas Init
                yield return null;
                camera.enabled = false;
            }
        }


        public SaveLoadData GetSaves()
        {
            return saves;
        }
    }
}
