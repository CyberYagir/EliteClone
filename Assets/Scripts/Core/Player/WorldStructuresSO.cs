using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public enum StructureNames
    {
        ComunistsBase
    }
    public class WorldStructuresSO : ScriptableObject
    {
        [System.Serializable]
        public class WorldStructure
        {
            [SerializeField] private string name;
            [SerializeField] private string guid;
            [SerializeField] private Vector3 position;
            [SerializeField] private StructureNames structure;

            public WorldStructure(string name, Vector3 position, StructureNames structure)
            {
                this.position = position;
                this.structure = structure;
                this.name = name;
                guid = Guid.NewGuid().ToString();
            }

            public StructureNames Structure => structure;

            public Vector3 Position => position;

            public string Name => name;
        }

        [System.Serializable]
        public class SystemStructure
        {
            [SerializeField] private string systemName;
            [SerializeField] private List<WorldStructure> structures = new List<WorldStructure>();

            public SystemStructure(string systemName)
            {
                this.systemName = systemName;
            }

            public string SystemName => systemName;
            public List<WorldStructure> Structures => structures;

            public void AddStructure(WorldStructure worldStructure)
            {
                structures.Add(worldStructure);
            }
        }


        [SerializeField] private List<SystemStructure> systemsData = new List<SystemStructure>();


        public List<WorldStructure> GetBySystem(string system)
        {
            var s = systemsData.Find(x => x.SystemName == system);

            if (s == null)
            {
                return new List<WorldStructure>();
            }

            return s.Structures;
        }

        public WorldStructure AddStructure(string structureName, string system, StructureNames type, Vector3 position)
        {
            var n = systemsData.Find(x => x.SystemName == system);

            if (n == null)
            {
                n = new SystemStructure(system);
                systemsData.Add(n);
            }

            var str = new WorldStructure(structureName, position, type);
            
            n.AddStructure(str);

            return str;
        }
    }
}
