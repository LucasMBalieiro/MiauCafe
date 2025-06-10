using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("Scene - Bala");
    }

    public void LoadGame()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
