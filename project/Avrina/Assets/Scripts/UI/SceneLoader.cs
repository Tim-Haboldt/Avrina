using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    ///  Stores the name of the scene to be laoded
    /// </summary>
    [Scene] [SerializeField] private string sceneName;


    /// <summary>
    /// Used to load the stored scene
    /// </summary>
    /// <param name="additive"></param>
    public void LoadScene(bool additive = false)
    {
        SceneManager.LoadSceneAsync(this.sceneName, (additive) ? LoadSceneMode.Additive : LoadSceneMode.Single);
    }

    /// <summary>
    ///  Used to unload the stored scene
    /// </summary>
    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(this.sceneName);
    }

    /// <summary>
    ///  Used to quit the application
    /// </summary>
    public void ExitUnity()
    {
        Application.Quit();
    }
}
