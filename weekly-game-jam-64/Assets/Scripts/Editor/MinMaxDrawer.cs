using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

// from https://gist.github.com/LotteMakesStuff/0de9be35044bab97cbe79b9ced695585

[CustomPropertyDrawer(typeof(MinMaxAttribute))]
public class MinMaxDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        MinMaxAttribute minMax = attribute as MinMaxAttribute;

        if (property.propertyType == SerializedPropertyType.Vector2) {
            if (minMax.ShowDebugValues || minMax.ShowEditRange) {
                position = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            }

            float minValue = property.vector2Value.x;
            float maxValue = property.vector2Value.y;
            float minLimit = minMax.MinLimit;
            float maxLimit = minMax.MaxLimit;

            EditorGUI.MinMaxSlider(position, label, ref minValue, ref maxValue, minLimit, maxLimit);

            var vec = Vector2.zero;
            vec.x = minValue;
            vec.y = maxValue;

            property.vector2Value = vec;

            if (minMax.ShowDebugValues || minMax.ShowEditRange) {
                bool isEditable = minMax.ShowEditRange;
                if (!isEditable) {
                    GUI.enabled = false;
                }

                position.y += EditorGUIUtility.singleLineHeight;

                Vector4 val = new Vector4(minLimit, minValue, maxValue, maxLimit);
                val = EditorGUI.Vector4Field(position, "MinLimit/MinVal/MaxVal/MaxLimit", val);
               
                // the range part is always read only
                GUI.enabled = false;
                position.y += EditorGUIUtility.singleLineHeight;

                EditorGUI.FloatField(position, "Selected Range", maxValue - minValue);
                GUI.enabled = true;

                if (isEditable) {
                    property.vector2Value = new Vector2(val.y, val.z); // save changes to the value
                }
            }
            
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        MinMaxAttribute minMax = attribute as MinMaxAttribute;

        float size = EditorGUIUtility.singleLineHeight;

        if (minMax.ShowEditRange || minMax.ShowDebugValues) {
            size += EditorGUIUtility.singleLineHeight * 2;
        }

        return size;
    }
}