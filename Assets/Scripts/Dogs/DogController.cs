using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DogController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float talkTime = 1.5f;
    [SerializeField] private float startScale = 1.5f;
    [SerializeField] private float endScale = 2.5f;
    [SerializeField] private int totalTalkSequences = 3;

    private static readonly int StartTalk = Animator.StringToHash("StartTalk");
    private static readonly int StartIdle = Animator.StringToHash("StartIdle");
    
    private Animator animator;
    private Transform midPosition;
    private Transform endPosition;
    private float totalMoveToEndDistance;
    private int currentTalkStep = 0;
    private bool isTalking = false;
    
    public UnityEvent OnStartTalk = new UnityEvent();

    private enum State
    {
        Idle,
        MovingToMid,
        MovingToEnd,
        WaitingForInput
    }
    
    private State currentState = State.Idle;
    private Transform currentTarget;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        transform.localScale = Vector3.one * startScale;
        SoundManager.Instance.PlaySFX("Door");
    }

    public void SetPositions(Transform midPoint, Transform endPoint)
    {
        midPosition = midPoint;
        endPosition = endPoint;
        StartCoroutine(MainSequence());
    }

    private void Update()
    {
        if (currentState != State.MovingToMid && currentState != State.MovingToEnd)
        {
            return;
        }

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, step);
        
        float distanceRemaining = Vector3.Distance(transform.position, endPosition.position);
        float progress = 1.0f - (distanceRemaining / totalMoveToEndDistance);
        float currentScale = Mathf.Lerp(startScale, endScale, progress);
        transform.localScale = Vector3.one * currentScale;
    }

    private IEnumerator MainSequence()
    {
        currentState = State.MovingToMid;
        currentTarget = midPosition;
        yield return new WaitUntil(() => Vector3.Distance(transform.position, midPosition.position) < 0.01f);

        currentState = State.MovingToEnd;
        currentTarget = endPosition;
        totalMoveToEndDistance = Vector3.Distance(transform.position, endPosition.position);
        yield return new WaitUntil(() => Vector3.Distance(transform.position, endPosition.position) < 0.01f);
    
        transform.localScale = Vector3.one * endScale;

        if (totalTalkSequences > 0)
        {
            isTalking = true;
            currentState = State.Idle;
            animator.SetTrigger(StartTalk);
            OnStartTalk.Invoke();
        
            yield return new WaitForSeconds(talkTime);
        
            animator.SetTrigger(StartIdle);
            isTalking = false;
            currentTalkStep++; 
        }

        while (currentTalkStep < totalTalkSequences)
        {
            currentState = State.WaitingForInput;

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !isTalking);
        
            isTalking = true;
            currentState = State.Idle;
            animator.SetTrigger(StartTalk);
            OnStartTalk.Invoke();
        
            yield return new WaitForSeconds(talkTime);
        
            animator.SetTrigger(StartIdle);
            isTalking = false;
            currentTalkStep++;
        }
    
        currentState = State.Idle;
    }
}