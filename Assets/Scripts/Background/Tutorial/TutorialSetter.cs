using System.Collections;
using Item.Grid;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class TutorialSetter : MonoBehaviour
{
    private Coroutine tutorialCoroutine;
    
    //Inventario
    [SerializeField] private GameObject inventario;
    private GameObject firstGridSlot;
    private NextStepTutorial nextStepTutorial;
    
    //DEBUG
    private float timer = 0f;
    [SerializeField] private TMP_Text tutorialText;
    
    
    //Limitar clicks na tela
    [SerializeField] private float tutorialStartTime;
    [SerializeField] private Image tutorialStartImage;
    
    //Mensagens
    [SerializeField] private GameObject FirstMessage;
    [SerializeField] private GameObject SecondMessage;
    [SerializeField] private GameObject ThirdMessage;
    [SerializeField] private GameObject FourthMessage;
    
    
    private bool isClickableSecondMessage = true;
    private bool isClickableThirdMessage = true;
    private bool isClickableFourthMessage = false;
    
    private void Start()
    {
        tutorialCoroutine = StartCoroutine(TutorialStart());
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        tutorialText.text = timer.ToString("00.00");
    }

    private IEnumerator TutorialStart()
    {
        while (tutorialStartTime > 0)
        {
            tutorialStartTime -= Time.deltaTime;
            yield return null;
        }
        
        FirstMessage.SetActive(true);
    }

    private IEnumerator MachineCooldown()
    {
        while (tutorialStartTime > 0)
        {
            tutorialStartTime -= Time.deltaTime;
            yield return null;
        }
        
        FourthMessage.SetActive(true);
        isClickableFourthMessage = true;
    }

    public void FirstMessageButton()
    {
        tutorialStartImage.enabled = false;
        FirstMessage.SetActive(false);
        SecondMessage.SetActive(true);
    }

    public void SecondMessageButton()
    {
        if (isClickableSecondMessage)
        {
            isClickableSecondMessage = false;
            SecondMessage.SetActive(false);
            ThirdMessage.SetActive(true);
            
            GetGridSlot();

            firstGridSlot.AddComponent<NextStepTutorial>();

            nextStepTutorial = firstGridSlot.GetComponent<NextStepTutorial>();
            
            nextStepTutorial.clickedMachine.AddListener(CoffeeMachineButton);
        }
    }

    public void CoffeeMachineButton()
    {
        if (isClickableThirdMessage)
        {
            isClickableThirdMessage = false;
            ThirdMessage.SetActive(false);

            tutorialStartTime = 4f;
            tutorialCoroutine = StartCoroutine(MachineCooldown());
        }

        if (isClickableFourthMessage)
        {
            isClickableFourthMessage = false;
            FourthMessage.SetActive(false);
        }
    }

    private void GetGridSlot()
    {
        firstGridSlot = inventario.transform.GetChild(0).GetChild(0).gameObject;
    }
}
