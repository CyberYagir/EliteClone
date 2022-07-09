using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public enum Scenes
    {
        Init, Galaxy, System, Location, Garage, Death, Map,
        Demo, CommunistsBase
    }

    public static class World
    {
        public static readonly float unitSize = 100;
        public static Scenes Scene { get; private set; } = Scenes.Init;
        public static int maxPlanetsCount { get; set; } = 5;


        public static void LoadLevel(Scenes scenes)
        {
            if (scenes == Scenes.Init)
            {
                Debug.Log("Go To Init");
            }
            SceneManager.LoadScene(scenes.ToString());
            Scene = scenes;
        }
    
        public static AsyncOperation LoadLevelAsync(Scenes scenes)
        {
            var op = SceneManager.LoadSceneAsync(scenes.ToString());
            Scene = scenes;
            return op;
        }

        public static void SetScene(Scenes scene)
        {
            Scene = scene;
        }
    }
}