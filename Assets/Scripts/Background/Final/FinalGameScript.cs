using System.Collections;
using UnityEngine;

public class FinalGameScript : MonoBehaviour
{
    [Header("Spawn Position")]
    [SerializeField] private Transform midPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private Transform startPosition;
    
    [Header("Objects")]
    [SerializeField] private GameObject DogPrefab;
    [SerializeField] private TalkSequence talkSequence;
    private DogController dogController;
    private Coroutine coroutine;
    
    

    private void Start()
    {
        SoundManager.Instance.StopMusic();

        coroutine = StartCoroutine(SpawnCooldown());
    }

    private IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(3f);
        GameObject instantiatedDog = Instantiate(DogPrefab, startPosition.position, startPosition.rotation);
        dogController = instantiatedDog.GetComponent<DogController>();
        dogController.SetPositions(midPosition, endPosition);
        
        talkSequence.Initialize(dogController);
        
    }
}
