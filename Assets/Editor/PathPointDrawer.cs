using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PathPoint))]
public class PathPointDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Get properties
        SerializedProperty activateDependentProp = property.FindPropertyRelative("activateDependentDecisions");
        SerializedProperty animationProp = property.FindPropertyRelative("animation");
        SerializedProperty transformProp = property.FindPropertyRelative("point");
        SerializedProperty hasEventProp = property.FindPropertyRelative("hasEvent");
        SerializedProperty eventProp = property.FindPropertyRelative("onReachPoint");

        float y = position.y;

        // Draw activateDependentDecisions toggle
        Rect dependentRect = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(dependentRect, activateDependentProp, new GUIContent("Activate Dependent"));
        y += EditorGUIUtility.singleLineHeight + 2;

        // Draw Animation field
        Rect animationRect = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(animationRect, animationProp);
        y += EditorGUIUtility.singleLineHeight + 2;

        // Draw Transform field
        Rect transformRect = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(transformRect, transformProp);
        y += EditorGUIUtility.singleLineHeight + 2;

        // Draw hasEvent toggle
        Rect hasEventRect = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(hasEventRect, hasEventProp, new GUIContent("Has Event"));
        y += EditorGUIUtility.singleLineHeight + 2;

        // Draw UnityEvent only if hasEvent is true
        if (hasEventProp.boolValue)
        {
            float eventHeight = EditorGUI.GetPropertyHeight(eventProp);
            Rect eventRect = new Rect(position.x, y, position.width, eventHeight);
            EditorGUI.PropertyField(eventRect, eventProp);
            y += eventHeight + 2;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty hasEventProp = property.FindPropertyRelative("hasEvent");
        SerializedProperty eventProp = property.FindPropertyRelative("onReachPoint");

        // Base height: 3 lines (activateDependent + animation + transform + hasEvent) + spacing
        float height = EditorGUIUtility.singleLineHeight * 4 + 6;

        if (hasEventProp.boolValue)
        {
            height += EditorGUI.GetPropertyHeight(eventProp) + 2;
        }

        return height;
    }
}
