using System;
using Scriptables.Item;
using UnityEngine;

public class CatDisplay : MonoBehaviour
{
    [SerializeField]private SpriteRenderer headSprite;
    [SerializeField]private SpriteRenderer bodySprite;
    [SerializeField]private SpriteRenderer legsSprite;
    
    public BaseCatScriptableObject teste;
    
    public void SetSprite(BaseCatScriptableObject catData)
    {
        headSprite.sprite = catData.headSprite;
        bodySprite.sprite = catData.bodySprite;
        legsSprite.sprite = catData.legsSprite;
    }

    public void Start()
    {
        SetSprite(teste);
    }
}
