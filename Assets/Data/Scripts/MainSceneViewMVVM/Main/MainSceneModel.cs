using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneModel
{
    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}