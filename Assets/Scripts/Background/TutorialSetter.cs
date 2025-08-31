using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class TutorialSetter : MonoBehaviour
{
    private Coroutine tutorialCoroutine;
    
    [SerializeField] private float tutorialStartTime;
    [SerializeField] private Image tutorialStartImage;
    
    private void Start()
    {
        tutorialCoroutine = StartCoroutine(TutorialStart());
    }

    private IEnumerator TutorialStart()
    {
        while (tutorialStartTime > 0)
        {
            tutorialStartTime -= Time.deltaTime;
            
            yield return null;
        }
        
        tutorialStartImage.enabled = false;
    }
}
