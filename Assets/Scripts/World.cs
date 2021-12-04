using UnityEngine.SceneManagement;
public enum Scenes
{
    Init, Galaxy, System, Location
}

public static class World
{
    public static readonly float unitSize = 100;
    public static Scenes Scene { get; private set; } = Scenes.Init;

   
    public static void LoadLevel(Scenes scenes)
    {
        SceneManager.LoadSceneAsync(scenes.ToString());
        Scene = scenes;
    }

    public static void SetScene(Scenes scene)
    {
        Scene = scene;
    }
}