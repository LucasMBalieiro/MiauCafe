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
        
        fadeScreen.DOFade(0, 2f).SetEase(Ease.InQuint)
            .OnComplete(() => {
            isAnimating = false;
            fadeScreen.gameObject.SetActive(false);
            
            clickIconImage.DOFade(1, 1f).OnComplete(() =>
            {
                _clickAnimationCoroutine = StartCoroutine(AnimateClickIcon());
            });
        });
    }
    
    private void Update()
    {
        // Only allow a click if no animation is currently playing
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

                // Return the previous tile to its original parent *before* animating it down
                // Ensure originalParentOfCurrentTile is not null before using it
                if (originalParentOfCurrentTile != null)
                {
                    previousTransform.SetParent(originalParentOfCurrentTile, true); // true maintains local position/scale
                    previousTransform.SetAsLastSibling(); // Optional: Bring it to the top of its original parent
                }
                
                // Add the "fade back" and "shrink" animations to the sequence
                mySequence.Append(previousImage.DOColor(originalColor, 0.5f));
                mySequence.Join(previousTransform.DOScale(originalScale, 0.5f));
            }

            // --- Step 2: Animate the CURRENT tile to pop ---
            if (counter < sketchTiles.Length)
            {
                GameObject currentTile = sketchTiles[counter];
                Image currentImage = currentTile.GetComponent<Image>();
                RectTransform currentTransform = currentTile.GetComponent<RectTransform>();

                // Store the original parent before changing it
                originalParentOfCurrentTile = currentTransform.parent;
                
                // Reparent the current tile to the popOutContainer
                currentTransform.SetParent(popOutContainer, true); // true maintains local position/scale
                currentTransform.SetAsLastSibling(); // Bring it to the very front of the PopOutContainer
                
                // Use Join() to make this happen at the same time as the previous tile animates down
                mySequence.Join(currentImage.DOColor(Color.white, 0.5f));
                mySequence.Join(currentTransform.DOScale(popScale, 0.5f));
                
                // --- Step 3: Update state for the next click ---
                previousTile = currentTile; // The current tile is now the "previous" one for the next click
                counter++;
            }
            else if (previousTile != null)
            {
                // Ensure the last animated tile goes back to its original parent
                RectTransform previousTransform = previousTile.GetComponent<RectTransform>();
                if (originalParentOfCurrentTile != null)
                {
                    previousTransform.SetParent(originalParentOfCurrentTile, true);
                    previousTransform.SetAsLastSibling();
                }
                previousTile = null; // Clear previous tile reference
                counter = 0; // Optional: Reset counter to loop through tiles again if needed
            }
            
            // --- Step 4: Unlock input AFTER the sequence is complete ---
            mySequence.OnComplete(() => {
                isAnimating = false;
                
                if (counter >= sketchTiles.Length)
                {
                    // If all tiles have been shown, make the button appear
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
        StopCoroutine(_clickAnimationCoroutine);
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