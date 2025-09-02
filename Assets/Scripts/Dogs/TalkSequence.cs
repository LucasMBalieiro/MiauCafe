using System.Threading;
using DataPersistence;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TalkSequence : MonoBehaviour
{
    private bool canTalk = false;
    private int counter = 0;
    private SoundManager sm;

    [SerializeField] private GameObject firstMessage;
    [SerializeField] private GameObject secondMessage;
    [SerializeField] private GameObject thirdMessage;
    [SerializeField] private GameObject winMessage;
    [SerializeField] private GameObject loseMessage;
    [SerializeField] private GameObject backToMenuMessage;
    
    private void Update()
    {
        if (canTalk)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                switch (counter)
                {
                    case 0:
                        sm.PlaySFX("Button");
                        firstMessage.SetActive(false);
                        secondMessage.SetActive(true);
                        break;
                    case 1:
                        sm.PlaySFX("Button");
                        secondMessage.SetActive(false);
                        thirdMessage.SetActive(true);
                        break;
                    case 2:
                        thirdMessage.SetActive(false);
                        HandleWinCondition();
                        break;
                    case 3:
                        sm.PlaySFX("Button");
                        DataPersistenceManager.Instance.NewGame();
                        backToMenuMessage.SetActive(true);
                        break;
                    case 4:
                        sm.PlaySFX("Button");
                        sm.ResumeMusic();
                        SceneManager.LoadScene("Main Menu - FINAL");
                        break;
                }
                
                if(counter < 3) canTalk = false;
                counter++;
            }
        }
    }

    public void Initialize(DogController dogController)
    {
        sm = SoundManager.Instance;
        dogController.OnStartTalk.AddListener(CanTalk);
    }
    
    private void CanTalk()
    {
        canTalk = true;

        if (counter == 0)
        {
            sm.PlaySFX("Button");
            firstMessage.SetActive(true);
        }
    }

    private void HandleWinCondition()
    {
        
        
        if (GameManager.Instance.GetCoins() > 1000)
        {
            sm.PlaySFX("Success");
            winMessage.SetActive(true);
        }
        else
        {
            sm.PlaySFX("Fail");
            loseMessage.SetActive(true);
        }
    }
}
