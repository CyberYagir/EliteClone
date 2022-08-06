using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Core.Bot
{
    public class BotVisual : MonoBehaviour
    {
        [SerializeField] private List<EngineParticles> engines = new List<EngineParticles>();
        [SerializeField] private List<GameObject> ships = new List<GameObject>();
        private int visualID;

        public string GetShipName()
        {
            return ships[visualID].name.Replace("ShipMesh", "");
        }
    
        private void Awake()
        {
            foreach (var lights in GetComponentsInChildren<Light>(true))
            {
                var engine = lights.transform.parent.gameObject.GetComponent<EngineParticles>();
                engines.Add(engine);
                engine.Init();
                
                
            }

            StartCoroutine(UpdateEngines());
        }


        IEnumerator UpdateEngines()
        {
            while (true)
            {
                for (int i = 0; i < engines.Count; i++)
                {
                    engines[i].UpdateObject();
                }

                yield return null;
            }
        }

        public void SetVisual(Random rnd)
        {
            var id = rnd.Next(0, ships.Count);
            SetVisual(id);
        }

        private void OnDestroy()
        {
            for (int i = 0; i < engines.Count; i++)
            {
                Destroy(engines[i].gameObject);
            }
        }

        public void SetVisual(int id)
        {
            visualID = id;
            for (int i = 0; i < ships.Count; i++)
            {
                ships[i].SetActive(i == id);
            }

            GetComponent<BotBuilder>().SetShip(ItemsManager.GetShipItem(GetShipName()));
        }

        public void ActiveLights()
        {
            foreach (var lights in engines)
            {
                lights.gameObject.SetActive(true);
            }
        }

        public void SetLights(bool state)
        {
            var current = ships.Find(x => x.active);
            if (current != null)
            {
                foreach (var lights in engines)
                {
                    lights.gameObject.SetActive(state);
                }
            }
        }

    }
}