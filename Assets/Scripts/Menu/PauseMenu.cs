using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]private GameObject pauseMenu;
    [SerializeField]private GameObject optionsMenu;
    [SerializeField]private GameObject dayCompleteMenu;
    [SerializeField]private GameObject[] hudElements;
    private bool isPaused = false;
    
    [SerializeField] private SpawnPositionController spawner;
    
    private void Start()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        dayCompleteMenu.SetActive(false);
    }

    private void OnEnable()
    {
        SpawnPositionController.OnDayEnd += DayCompleteScreen;
    }

    private void OnDisable()
    {
        SpawnPositionController.OnDayEnd -= DayCompleteScreen;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void DayCompleteScreen()
    {
        Time.timeScale = 0;
        foreach (GameObject element in hudElements)
        {
            element.SetActive(false);
        }
        
        dayCompleteMenu.SetActive(true);
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);

        foreach (GameObject element in hudElements)
        {
            element.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        foreach (GameObject element in hudElements)
        {
            element.SetActive(true);
        }
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        GameManager.Instance.ChangeDay();
        if (GameManager.Instance.GetCurrentDay() <= 3)
        {
            SceneManager.LoadScene("Scene - Bala"); 
        }
        else
        {
            SceneManager.LoadScene("MainMenu - Bala"); //TODO: fazer cena final
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu - Bala");
    }
}
