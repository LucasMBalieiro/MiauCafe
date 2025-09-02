using System.Collections;
using DataPersistence;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class IntroMainScript : MonoBehaviour
{
    [Header("Sketch Tiles")]
    [SerializeField] private Image fadeScreen;
    [SerializeField] private GameObject[] sketchTiles;
    [SerializeField] private Transform popOutContainer;
    [SerializeField] private float scale;
    
    [Header("Click Icon")]
    [SerializeField] private Image clickIconImage;
    [SerializeField] private Sprite handSprite;
    [SerializeField] private Sprite handClickSprite;
    [SerializeField] private float clickAnimationInterval = 0.5f;
    
    [Header("Continue Button")]
    [SerializeField] private GameObject continueButton;
    
    private Coroutine _clickAnimationCoroutine;
    
    private int counter = 0;
    private GameObject previousTile = null;
    private bool isAnimating = false;
    private bool firstClick = false;
    private Transform originalParentOfCurrentTile;

    private Color originalColor = new Color32(80, 80, 80, 255);
    private Vector3 originalScale = Vector3.one;
    private Vector3 popScale = new Vector3(1.1f, 1.1f, 1.1f);

    private void Start()
    {
        popScale = new Vector3(scale, scale, scale);
        
        isAnimating = true;
        
        fadeScreen.DOFade(0, 2.2f).SetEase(Ease.InOutQuad)
            .OnComplete(() => {
            isAnimating = false;
            fadeScreen.gameObject.SetActive(false);
            
            clickIconImage.DOFade(1, 1.5f).SetEase(Ease.InOutQuad)
                .OnComplete(() =>
            {
                _clickAnimationCoroutine = StartCoroutine(AnimateClickIcon());
            });
        });
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAnimating)
        {
            isAnimating = true;
            firstClick = true;
            SoundManager.Instance.PlaySFX("Button");

            Sequence mySequence = DOTween.Sequence();

            if (previousTile != null)
            {
                Image previousImage = previousTile.GetComponent<Image>();
                RectTransform previousTransform = previousTile.GetComponent<RectTransform>();

                if (originalParentOfCurrentTile != null)
                {
                    previousTransform.SetParent(originalParentOfCurrentTile, true); 
                    previousTransform.SetAsLastSibling();
                }
                
                mySequence.Append(previousImage.DOColor(originalColor, 0.5f));
                mySequence.Join(previousTransform.DOScale(originalScale, 0.5f));
            }

            if (counter < sketchTiles.Length)
            {
                GameObject currentTile = sketchTiles[counter];
                Image currentImage = currentTile.GetComponent<Image>();
                RectTransform currentTransform = currentTile.GetComponent<RectTransform>();

                originalParentOfCurrentTile = currentTransform.parent;
                
                currentTransform.SetParent(popOutContainer, true);
                currentTransform.SetAsLastSibling();
                
                mySequence.Join(currentImage.DOColor(Color.white, 0.5f));
                mySequence.Join(currentTransform.DOScale(popScale, 0.5f));
                
                previousTile = currentTile; 
                counter++;
            }
            else if (previousTile != null)
            {
                RectTransform previousTransform = previousTile.GetComponent<RectTransform>();
                if (originalParentOfCurrentTile != null)
                {
                    previousTransform.SetParent(originalParentOfCurrentTile, true);
                    previousTransform.SetAsLastSibling();
                }
                previousTile = null; 
                counter = 0; 
            }
            
            mySequence.OnComplete(() => {
                isAnimating = false;
                
                if (counter >= sketchTiles.Length)
                {
                    ShowNextSceneButton();
                }
            });
        }
    }

    private void ShowNextSceneButton()
    {
        CanvasGroup buttonCanvasGroup = continueButton.GetComponent<CanvasGroup>();
        
        continueButton.SetActive(true);
        buttonCanvasGroup.DOFade(1, 0.5f);
    }
    
    private IEnumerator AnimateClickIcon()
    {
        while (!firstClick)
        {
            clickIconImage.sprite = handClickSprite;
            yield return new WaitForSeconds(clickAnimationInterval);

            clickIconImage.sprite = handSprite;
            yield return new WaitForSeconds(clickAnimationInterval);
        }
        
        clickIconImage.gameObject.SetActive(false);
    }

    public void ContinueButton()
    {
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadScene("Tutorial - FINAL");
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}