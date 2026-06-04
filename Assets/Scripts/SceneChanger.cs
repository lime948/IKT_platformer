using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    string previousScene;

    public void LoadScene(string sceneName)
    {
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void Back()
    {   
        if (previousScene == null)
        {
            previousScene = "MainMenu";
            SceneManager.LoadScene(previousScene);
        }
        else       
        {
            SceneManager.LoadScene(previousScene);
        }
    }

    public void NextLvl()
    {
        previousScene = SceneManager.GetActiveScene().name;
        if (previousScene == "Lvl1")
        {
            SceneManager.LoadScene("Lvl2");
        }
        else if (previousScene == "Lvl2")
        {
            SceneManager.LoadScene("Lvl3");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void RetryDebug()
    {
        SceneManager.LoadScene("DebugScene");
    }
}