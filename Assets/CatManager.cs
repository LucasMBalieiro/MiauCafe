using System.Linq;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public Transform[] posMesa;
    private bool[] posEstaLivre = { true, true, true };
    public GameObject prefabGato;
    private float timer = 0;
    public float intervalo = 10;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > intervalo)
        {
            
            {
                Instantiate(prefabGato, posMesa[0]);   
            }
        }
    }
}
