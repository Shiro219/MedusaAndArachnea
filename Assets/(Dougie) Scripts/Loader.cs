using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public enum Scene
    {
        LevelOne,
        LevelTwo,
        LevelThree,
        LevelA,
        Credits,
        LoadingScene,
    }

    public static Scene[] levelList = { Scene.LevelOne, Scene.LevelTwo, Scene.LevelThree, Scene.LevelA, Scene.Credits};
    public static int index = 1;

    public static void setIndex(int indexNew)
    {
        index = indexNew;
    }

    // public int getIndex(Scene newScene)
    // {
    //     return index;
    // }

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        
        Loader.targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(levelList[index].ToString());
    }
}
