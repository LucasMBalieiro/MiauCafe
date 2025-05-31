using UnityEngine;
using Managers;
using Scriptables.Item;
using UnityEngine.UI;
using TMPro;

public class Pedido : MonoBehaviour
{
    public Image pedidoImagem;
    public TextMeshProUGUI quantidadeTxt;
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
    

    public void SetPedido(BaseItemScriptableObject pedidoID, int quantidade)
    {
        pedidoImagem.sprite = pedidoID.sprite;
        quantidadeTxt.text = quantidade.ToString();
        
        // Toca o som do novo pedido
        if (newOrderSound != null)
        {
            audioSource.PlayOneShot(newOrderSound, orderSoundVolume);
        }
    }
}
