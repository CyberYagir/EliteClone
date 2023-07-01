using System.IO;
using UnityEngine;
using Zenject;

namespace Core.Core.Inject.FoldersManagerService
{
    public class FolderManagerService : MonoBehaviour
    {
        private string playerFolder;
        private string globalFolder;
        private string cacheSystemsFolder;
        private string rootFolder;


        private string galaxyFile;
        private string currentSystemFile;
        private string currentLocationFile;
        private string playerDataFile;
        private string configFile;
        private string deadsNPCFile;
        private string mapFile;
        private string tutorialsFile;

        
        
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

        [Inject]
        public void Constructor()
        {
            CreateFoldersPaths();
        }
        
        public void CreateFoldersPaths()
        {
            if (!Directory.Exists(Directory.GetParent(Application.dataPath).FullName + "/Saves"))
            {
                Directory.CreateDirectory(Directory.GetParent(Application.dataPath).FullName + "/Saves");
            }

            rootFolder = Directory.GetParent(Application.dataPath)?.FullName + "/Saves";

            if (!Directory.Exists(rootFolder + "/Player"))
            {
                Directory.CreateDirectory(rootFolder + "/Player/");
            }

            playerFolder = rootFolder + "/Player/";
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
            tutorialsFile = globalFolder + "tutorials.config";
            configFile = playerFolder + "options.config";
        }
        
        
        public void RemoveSave()
        {
            Directory.Delete(CacheSystemsFolder, true);
            Directory.Delete(GlobalFolder, true);
            
            CreateFoldersPaths();
        }
    }
}
