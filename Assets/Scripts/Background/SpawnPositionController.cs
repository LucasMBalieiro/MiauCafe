using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPositionController : MonoBehaviour
{
    
    [Header("Spawn Position")]
    public Transform startPosition;
    public Transform midPosition;
    public Transform[] endPositions;
    public Transform[] gridObjects;

    
    [SerializeField] private GameObject baseCatPrefab;
    private bool[] isPositionFree;
    private List<CompleteClient> clientList;

    public static Action OnDayEnd;

    private void Start()
    {
        isPositionFree = new bool[endPositions.Length];
        
        for (int i = 0; i < endPositions.Length; i++)
        {
            isPositionFree[i] = true;
        }
        
        SetupClientList();
        
        StartCoroutine(SpawnClientWhenReady());
    }
    
    private void SetupClientList()
    {
        clientList = new List<CompleteClient>();
        Day dayData = GameManager.Instance.GetCurrentDayData();

        if (dayData != null)
        {
            foreach (CompleteClient client in dayData.clientList)
            {
                clientList.Add(client);
            }
        }
    }
    
    private IEnumerator SpawnClientWhenReady()
    {
        while (true)
        {
            if (clientList.Count > 0)
            {
                int positionIndex = CheckIfPositionIsFree();
                if (positionIndex != -1)
                {
                    yield return new WaitForSeconds(5f);
                
                    isPositionFree[positionIndex] = false;

                    int randomIndex = Random.Range(0, clientList.Count);

                    CompleteClient clientToSpawn = clientList[randomIndex];

                    clientList.RemoveAt(randomIndex);
                
                    SpawnCat(clientToSpawn, positionIndex);
                }
            }

            if (clientList.Count == 0 && AreAllPositionsFree())
            {
                Debug.Log("Acabou o dia");
                Time.timeScale = 0;
                //TODO: adicionar animação/som de final de fase aqui
                OnDayEnd?.Invoke();
                
                yield break;
            }
        
            yield return null;
        }
    }

    private void SpawnCat(CompleteClient clientData, int positionIndex)
    {
        GameObject newCat = Instantiate(baseCatPrefab, startPosition.position, Quaternion.identity);
        CatController catController = newCat.GetComponent<CatController>();

        catController.Initialize(clientData.baseCat, clientData.pedidos, this, midPosition, endPositions[positionIndex], gridObjects[positionIndex], positionIndex);
        
        //TODO: colocar som de gato entrando no cafe aqui
        
        PedidoDropSlot dropTarget = gridObjects[positionIndex].GetComponent<PedidoDropSlot>();
        dropTarget.Initialize(catController);
    }

    private int CheckIfPositionIsFree()
    {
        for (int i = 0; i < endPositions.Length; i++)
        {
            if (isPositionFree[i])
            {
                isPositionFree[i] = false;
                return i;
            }
        }
        return -1;
    }
    
    private bool AreAllPositionsFree()
    {
        foreach (bool isFree in isPositionFree)
        {
            if (!isFree)
            {
                return false;
            }
        }
        return true;
    }
    
    public void FreeUpPosition(int positionIndex)
    {
        if (positionIndex >= 0 && positionIndex < isPositionFree.Length)
        {
            isPositionFree[positionIndex] = true;
            
            PedidoDropSlot dropSlot = gridObjects[positionIndex].GetComponent<PedidoDropSlot>();
            dropSlot.SetBackgroundActive(false);
        }
    }
    
}
