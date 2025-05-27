using Managers;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(SpriteDictionary))]
public class SpriteDictionaryEditor : Editor
{
    private SerializedProperty itemSpritesProp;

    private void OnEnable()
    {
        itemSpritesProp = serializedObject.FindProperty("itemSprites");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw default properties except itemSprites
        DrawPropertiesExcluding(serializedObject, "itemSprites");

        // Custom drawing for itemSprites array
        EditorGUILayout.PropertyField(itemSpritesProp, true);

        // Apply modified properties
        serializedObject.ApplyModifiedProperties();
    }
}
#endif