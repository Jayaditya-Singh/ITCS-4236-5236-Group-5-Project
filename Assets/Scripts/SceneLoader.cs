using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Tiny helper so any UI Button can switch scenes without writing code.
///
/// Use: add this to any GameObject (e.g. the Canvas on the title screen), then on a
/// Button's On Click () list press +, drag that GameObject in, choose
/// SceneLoader > LoadScene (string), and type the scene name, e.g. "Instructions".
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene '" + sceneName + "' isn't in File > Build Settings > Scenes In Build (or is misspelled).");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
