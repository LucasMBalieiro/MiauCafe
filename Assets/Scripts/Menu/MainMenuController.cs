using System;
using DataPersistence;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject GameData;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private Button loadButton;
    
    private int _day;
    private int _coins;
    
    private void Start()
    {
        bool hasSaveData = DataPersistenceManager.Instance.HasSaveData();
        loadButton.interactable = hasSaveData;
        GameData.SetActive(hasSaveData);
    }

    public void NewGame()
    {
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadScene("Scene - Bala");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Scene - Bala");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void UpdateUI()
    {
        dayText.text = "Day - " + _day;
        coinsText.text = "Coins - " + _coins;
    }

    public void LoadData(GameData data)
    {
        _day = data.currentDay;
        _coins = data.currentCoins;

        UpdateUI();
    }

    public void SaveData(ref GameData data)
    {
    }
}
