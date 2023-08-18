using System.IO;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class FilesSystemHandler
    {
        [SerializeField] private string rootFolder;
        [SerializeField] private string playerFolder;
        [SerializeField] private string globalFolder;
        [SerializeField] private string cacheSystemsFolder;


        [SerializeField] private string galaxyFile;
        [SerializeField] private string currentSystemFile;
        [SerializeField] private string currentLocationFile;
        [SerializeField] private string playerDataFile;
        [SerializeField] private string configFile;
        [SerializeField] private string deadsNPCFile;
        [SerializeField] private string mapFile;
        [SerializeField] private string tutorialsFile;
        [SerializeField] private string structuresFile;

        public string TutorialsFile => tutorialsFile;
        public string MapFile => mapFile;
        public string DeadsNpcFile => deadsNPCFile;
        public string ConfigFile => configFile;
        public string PlayerDataFile => playerDataFile;
        public string CurrentLocationFile => currentLocationFile;
        public string CurrentSystemFile => currentSystemFile;
        public string GalaxyFile => galaxyFile;
        public string RootFolder => rootFolder;
        public string CacheSystemsFolder => cacheSystemsFolder;
        public string GlobalFolder => globalFolder;
        public string PlayerFolder => playerFolder;
        public string StructuresFile => structuresFile;


        public void CreateFolders()
        {
            if (!Directory.Exists(Directory.GetParent(Application.dataPath).FullName + "/Saves"))
            {
                Directory.CreateDirectory(Directory.GetParent(Application.dataPath).FullName + "/Saves");
            }

            rootFolder = Directory.GetParent(Application.dataPath)?.FullName + "/Saves";

            if (!Directory.Exists(rootFolder + "/Player"))
            {
                Directory.CreateDirectory(rootFolder + "/Player");
            }

            playerFolder = rootFolder + "/Player";
            if (!Directory.Exists(playerFolder + "/Global"))
            {
                Directory.CreateDirectory(playerFolder + "/Global");
            }

            globalFolder = playerFolder + "/Global/";
            if (!Directory.Exists(playerFolder + "/Locations"))
            {
                Directory.CreateDirectory(playerFolder + "/Locations");
            }

            cacheSystemsFolder = playerFolder + "/Locations/";

            galaxyFile = globalFolder + "galaxy.json";
            currentSystemFile = globalFolder + "system.json";
            currentLocationFile = globalFolder + "location.json";
            deadsNPCFile = globalFolder + "npcs.json";
            mapFile = globalFolder + "map.json";
            playerDataFile = globalFolder + "player.json";
            structuresFile = globalFolder + "structures.json";
            tutorialsFile = globalFolder + "tutorials.config";
            configFile = playerFolder + "options.config";
        }



        public void RemoveSave()
        {
            Directory.Delete(CacheSystemsFolder, true);
            Directory.Delete(GlobalFolder, true);
            CreateFolders();
        }
    }
}