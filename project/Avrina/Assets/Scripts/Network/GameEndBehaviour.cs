using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameEndBehaviour : NetworkBehaviour
{
    /// <summary>
    ///  Will store the player name who won
    /// </summary>
    // Ramajana05
    [SyncVar]
    [HideInInspector] public int wonPlayerId;
    /// <summary>
    ///  Did the first or second player win
    /// </summary>
    [SyncVar]
    [HideInInspector] public bool wonSecondPlayer;

    /// <summary>
    ///  Will be used to load the end scene
    /// </summary>
    [SerializeField] private SceneLoader endSceneLoader;
    /// <summary>
    ///  Name of the end game scene
    /// </summary>
    [SerializeField] private string endSceneName;
    /// <summary>
    ///  Used to return to the start menu scene
    /// </summary>
    [SerializeField] private SceneLoader startMenuScene;

    /// <summary>
    ///  What is the string on the end game screen
    /// </summary>
    private string endGameContent;

    /// <summary>
    ///  Will be called on each client after the game ended
    /// </summary>
    public override void OnStartClient()
    {
        this.endSceneLoader.LoadScene(true);

        if (this.wonPlayerId != NetworkClient.connection.connectionId)
        {
            this.endGameContent = "You Lost!";
        }
        else if (this.wonSecondPlayer)
        {
            this.endGameContent = "Second Player won!";
        }
        else
        {
            this.endGameContent = "First Player won!";
        }

        StartCoroutine("waitForSceneLoad", this.endSceneName);
    }

    /// <summary>
    ///  Waits for the scene to be loaded
    /// </summary>
    /// <param name="sceneNumber"></param>
    /// <returns></returns>
    IEnumerator waitForSceneLoad(string sceneName)
    {
        var isSceneLoaded = false;
        while (!isSceneLoaded)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (sceneName.Equals(SceneManager.GetSceneAt(i).name))
                {
                    isSceneLoaded = true;
                }
            }

            if (!isSceneLoaded)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        TextMeshProUGUI textMesh;
        do
        {
            GameObject go;
            do
            {
                go = GameObject.Find("GameStatus");
                if (go == null)
                {
                    yield return new WaitForEndOfFrame();
                }
            } while (go == null);

            Debug.Log(go);

            textMesh = go.GetComponent<TextMeshProUGUI>();
            if (textMesh == null)
            {
                yield return new WaitForEndOfFrame();
            }

            Debug.Log(textMesh);
        } while (textMesh == null);

        textMesh.text = this.endGameContent;

        yield return new WaitForSecondsRealtime(7);

        this.startMenuScene.LoadScene();

        NetworkManager.Shutdown();
    }
}
