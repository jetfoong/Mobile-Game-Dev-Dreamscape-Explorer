using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Setting");
    }
    public void Credit()
    {
        SceneManager.LoadScene("Credit");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
