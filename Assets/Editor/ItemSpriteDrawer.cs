using Managers;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SpriteDictionary.ItemSprite))]
public class ItemSpriteDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Get properties
        var typeProp = property.FindPropertyRelative("type");
        var tierSpritesProp = property.FindPropertyRelative("tierSprites");
        var hasPriceProp = property.FindPropertyRelative("hasPrice");
        var tierPricesProp = property.FindPropertyRelative("tierPrices");

        // Calculate rects
        Rect typeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect spritesRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUI.GetPropertyHeight(tierSpritesProp));
        
        // Draw fields
        EditorGUI.PropertyField(typeRect, typeProp);
        EditorGUI.PropertyField(spritesRect, tierSpritesProp, true);

        // Pricing header
        Rect priceHeaderRect = new Rect(position.x, position.y + EditorGUI.GetPropertyHeight(tierSpritesProp) + EditorGUIUtility.singleLineHeight + 4, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(priceHeaderRect, "Preço", EditorStyles.boldLabel);

        // Has price toggle
        Rect hasPriceRect = new Rect(position.x, priceHeaderRect.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(hasPriceRect, hasPriceProp);

        // Only show tier prices if hasPrice is true
        if (hasPriceProp.boolValue)
        {
            Rect pricesRect = new Rect(position.x, hasPriceRect.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUI.GetPropertyHeight(tierPricesProp));
            EditorGUI.PropertyField(pricesRect, tierPricesProp, true);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight * 2; // type + spacing
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("tierSprites"));
        height += EditorGUIUtility.singleLineHeight * 2; // header + hasPrice
        
        var hasPriceProp = property.FindPropertyRelative("hasPrice");
        if (hasPriceProp.boolValue)
        {
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("tierPrices")) + 2;
        }

        return height;
    }
}
#endif