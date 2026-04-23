using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public enum Scene
    {
        LevelOne,
        LevelTwo,
        LoadingScene,
    }

    public static Scene[] levelList = { Scene.LevelOne, Scene.LevelTwo };
    public static int index = 0;

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        
        Loader.targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(levelList[index++].ToString());
    }
}
