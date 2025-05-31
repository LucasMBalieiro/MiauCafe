using OldItem;
using UnityEngine;
using UnityEngine.UI;

public class Cliente : MonoBehaviour
{
    public GameObject prefabPedido;
    public Transform listaPedido;
    private GameObject pedidoUI;
    public ItemID pedidoID;
    private bool sentado;

    //background music
    //[Header("Configuração de Áudio")]
    //[SerializeField] private AudioClip backgroundLoop;

    //private AudioSource audioSource;


    void Start()
    {
        // Configura o AudioSource
        //audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.clip = backgroundLoop;
        //audioSource.loop = true;
        //audioSource.spatialBlend = 0; // Áudio 2D
        //audioSource.Play();
        //
        pedidoID = new ItemID(ItemType.Cafe, 1);
        sentado = false;
        Entrar();
    }

    void Entrar()
    {
        
    }

    void Update()
    {
        if(sentado) 
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
            pedidoUI = Instantiate(prefabPedido, listaPedido);
            pedidoUI.GetComponent<Pedido>().SetPedido(pedidoID);
        }
        
    }
    
}
