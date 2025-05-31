using UnityEngine;
using Managers;
using OldItem;
using UnityEngine.UI;

public class Pedido : MonoBehaviour
{
    public Image pedidoImagem;
    //Som de pedido
    [Header("Configuração de Áudio")]
    [SerializeField] private AudioClip newOrderSound;
    [SerializeField] [Range(0f, 1f)] private float orderSoundVolume = 1f;

    private AudioSource audioSource;
    private void Awake()
    {
        // Configura o AudioSource automaticamente
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0; // Som 2D
        }
    }
    

    public void SetPedido(ItemID pedidoID)
    {
        pedidoImagem.sprite = SpriteDictionary.Instance.GetSpriteForItem(pedidoID);
        
        // Toca o som do novo pedido
        if (newOrderSound != null)
        {
            audioSource.PlayOneShot(newOrderSound,orderSoundVolume);
        }
    }
}
