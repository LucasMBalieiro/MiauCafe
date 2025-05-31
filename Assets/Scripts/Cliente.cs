using UnityEngine;
using Item;
using System.Collections.Generic;
using Scriptables.Item;

public class Cliente : MonoBehaviour
{
    public GameObject prefabPedido;
    private List<GameObject> pedidoUI = new List<GameObject>();

    // O CatManager deverá settar a referência e os atributos abaixo
    public Transform bandeija;
    public List<IngredientScriptableObject> ingredients;
    public List<int> quantidades;

    //background music
    //[Header("Configuração de Áudio")]
    //[SerializeField] private AudioClip backgroundLoop;

    //private AudioSource audioSource;


    void Start()
    {

        pedidoID.Add(new ItemID(ItemType.Cafe, 1));
        quantidades.Add(2);
        pedidoID.Add(new ItemID(ItemType.Cafe, 2));
        quantidades.Add(1);
        
        for (int i = 0; i < pedidoID.Count; i++)
        {
            if (prefabPedido == null) { Debug.Log("n existe prefab de slot de pedido"); }
            if (bandeija == null) { Debug.Log("n existe bandeija"); }
            pedidoUI.Add(Instantiate(prefabPedido, bandeija));
            pedidoUI[i].GetComponent<Pedido>().SetPedido(pedidoID[i], quantidades[i]);
        }
    }

    void Update()
    {
        
    }
    
}
