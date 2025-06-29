using System;
using Scriptables.Item;
using UnityEngine;

public class CatDisplay : MonoBehaviour
{
    [SerializeField]private SpriteRenderer headSprite;
    [SerializeField]private SpriteRenderer bodySprite;
    [SerializeField]private SpriteRenderer lArmSprite;
    [SerializeField]private SpriteRenderer rArmSprite;
    [SerializeField]private SpriteRenderer legsSprite;
    [SerializeField]private SpriteRenderer tailSprite;
    
    public BaseCatScriptableObject teste;
    
    public void SetSprite(BaseCatScriptableObject catData)
    {
        headSprite.sprite = catData.headSprite;
        bodySprite.sprite = catData.bodySprite;
        lArmSprite.sprite = catData.lArmSprite;
        rArmSprite.sprite = catData.rArmSprite;
        legsSprite.sprite = catData.legsSprite;
        tailSprite.sprite = catData.tailSprite;

    }

    public void Start()
    {
        SetSprite(teste);
    }
}
