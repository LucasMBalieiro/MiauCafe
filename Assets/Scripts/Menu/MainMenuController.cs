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
    
    [SerializeField] private Image background;
    [SerializeField] private Sprite optionsMenuImage;
    
    
    private int _day;
    private int _coins;
    
    private void Start()
    {
        bool hasSaveData = DataPersistenceManager.Instance.HasSaveData();

        if (hasSaveData && (_day != 0 || _day != 5))
        {
            loadButton.interactable = true;
            GameData.SetActive(true);
        }
        else
        {
            loadButton.interactable = false;
            GameData.SetActive(false);
        }
        
    }

    public void PlaySound()
    {
        SoundManager.Instance.PlaySFX("Button");
    }

    public void NewGame()
    {
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadScene("Intro Scene - FINAL");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Scene - Bala");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeBackground()
    {
        background.sprite = optionsMenuImage;
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
