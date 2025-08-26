using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CatAnimation : MonoBehaviour
{
    private static readonly int StartTalk = Animator.StringToHash("StartTalk");
    private static readonly int StartIdle = Animator.StringToHash("StartIdle");
    private Animator animator;
    
    private Transform midPosition;
    private Transform endPosition;
    
    [SerializeField] private float speed;
    [SerializeField] private float talkTime;
    
    [Header("Distancia Porta - Bancada")]
    [SerializeField] private float startScale = 1.5f;
    [SerializeField] private float endScale = 2.5f;

    private float totalMoveToEndDistance;
    
    public UnityEvent OnEndTalk = new UnityEvent();


    private enum State
    {
        MoveToMid,
        MoveToEnd,
        Talking,
        Idle
    }
    
    private State currentState;
    private Transform currentTarget;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        transform.localScale = Vector3.one * startScale;
    }

    private void Update()
    {
        if (currentState != State.MoveToMid && currentState != State.MoveToEnd)
        {
            return;
        }

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, step);
        
        if (currentState is State.MoveToMid or State.MoveToEnd)
        {
            float distanceRemaining = Vector3.Distance(transform.position, endPosition.position);
            float progress = 1.0f - (distanceRemaining / totalMoveToEndDistance);

            float currentScale = Mathf.Lerp(startScale, endScale, progress);
            transform.localScale = Vector3.one * currentScale;
        }
        
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.01f)
        {
            if (currentState == State.MoveToMid)
            {
                currentState = State.MoveToEnd;
                currentTarget = endPosition;
                totalMoveToEndDistance = Vector3.Distance(transform.position, endPosition.position);
            }
            else if (currentState == State.MoveToEnd)
            {
                transform.localScale = Vector3.one * endScale;
                StartCoroutine(TalkingSequence());
            }
        }
    }
    
    private IEnumerator TalkingSequence()
    {
        currentState = State.Talking;
        animator.SetTrigger(StartTalk);
        SoundManager.Instance.PlayRandomMeow();
        
        yield return new WaitForSeconds(talkTime);
        OnEndTalk.Invoke();
        
        currentState = State.Idle;
        animator.SetTrigger(StartIdle);
    }

    public void SetPositions(Transform midPoint, Transform endPoint)
    {
        midPosition = midPoint;
        endPosition = endPoint;
        
        SoundManager.Instance.PlaySFX("Door");
        currentState = State.MoveToMid;
        currentTarget = midPosition;
    }
}