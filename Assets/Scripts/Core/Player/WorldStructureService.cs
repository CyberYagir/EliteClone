using System.Collections.Generic;
using Core.PlayerScripts;
using Core.Systems;
using UnityEngine;

namespace Core
{
    public class WorldStructureService : SaveLoadScriptable<WorldStructuresSO>
    {

        [System.Serializable]
        public class Structure
        {
            [SerializeField] private StructureNames name;
            [SerializeField] private GameObject prefab;

            public GameObject Prefab => prefab;

            public StructureNames Name => name;
        }

        [SerializeField] private List<Structure> structuresList;
        [SerializeField] private List<WorldSpaceObject> spawnedStructures;

        private WorldDataHandler worldDataHandler;

        public override void Init(string path)
        {
            base.Init(path);
            worldDataHandler = PlayerDataManager.Instance.WorldHandler;
        }
        
        
        public List<WorldStructuresSO.WorldStructure> GetSystemStructures(string system)
        {
            return scriptable.GetBySystem(system);
        }

        public WorldStructuresSO.WorldStructure AddStructure(string structureName, string system, StructureNames type, Vector3 position)
        {
            var structure = scriptable.AddStructure(structureName, system, type, position);
            Save();

            if (system == worldDataHandler.CurrentSolarSystem.name)
            {
                SolarStaticBuilder.RespawnCustomStructures(worldDataHandler.CurrentSolarSystem, SolarStaticBuilder.BaseTransform, SolarStaticBuilder.GalaxyScale, true);
            }
            
            return structure;
        }

        public GameObject GetPrefab(StructureNames structureStructure)
        {
            return structuresList.Find(x => x.Name == structureStructure).Prefab;
        }

        public void ClearSpawnedStructures()
        {
            foreach (var spawned in spawnedStructures)
            {
                if (spawned != null)
                {
                    var contact = spawned.GetComponent<ContactObject>();
                    if (contact)
                    {
                        var player = worldDataHandler.ShipPlayer;
                        if (player)
                        {
                            player.TargetsManager.RemoveContact(contact, false);
                        }
                    }

                    SolarStaticBuilder.RemoveSpawnedObject(spawned);
                    Destroy(spawned.gameObject);
                }
            }
            
            spawnedStructures.Clear();
        }

        public void AddSpawned(WorldSpaceObject wso, bool trigger)
        {
            var contact = wso.GetComponent<ContactObject>();
            if (contact)
            {
                var player = worldDataHandler.ShipPlayer;
                if (player)
                {
                    player.TargetsManager.AddContact(contact, trigger);
                }
            }
            spawnedStructures.Add(wso);
        }

        public void UpdateContacts()
        {
            var player = PlayerDataManager.Instance.WorldHandler.ShipPlayer;
            if (player)
            {
                player.TargetsManager.UpdateContacts();
            }
        }
    }
}
