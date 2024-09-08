using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class MultipliableFloat
{
    public float baseValue;
    [HideInInspector] public float multiplier = 1f;

    public static implicit operator float(MultipliableFloat f) => f.baseValue * f.multiplier;

    public void ResetMultiplier() => multiplier = 1f;
}

[CustomPropertyDrawer(typeof(MultipliableFloat))]
public class MultipliableFloatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty baseValueProp = property.FindPropertyRelative("baseValue");
        EditorGUI.PropertyField(position, baseValueProp, label);
    }
}