using UnityEngine;

[CreateAssetMenu(fileName = "NewCat", menuName = "Cats/BaseCat")]
public class BaseCatScriptableObject : ScriptableObject
{
    public int tier;
    public Sprite headSprite;
    public Sprite bodySprite;
    public Sprite legsSprite;
}
