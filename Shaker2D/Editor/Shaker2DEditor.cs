using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

/*

    Shaker2D created by Lucas Sarkadi.

    Creative Commons Zero v1.0 Universal licence, 
    meaning it's free to use in any project with no need to ask permission or credits the author.

    Check out the github page for more informations:
    https://github.com/Slyp05/Unity_Shaker2D

*/
[CustomEditor(typeof(Shaker2D)), CanEditMultipleObjects]
public class Shaker2DEditor : Editor
{
    // consts
    const string categoryPivot = "Pivot";
    const string categoryShakeType = "Shake Type";
    const string categoryShakeIntensity = "Shake Intensity";
    const string categoryTrauma = "Trauma";

    const string infoNoPivot = "The pivot will default to this GameObject Transform";
    const string errorNoPossibleShake = "No possible shake selected, the component will have no effect";
    
    public override void OnInspectorGUI()
    {
        // begin
        serializedObject.Update();

        // get vars
        Shaker2D shaker = target as Shaker2D;

        SerializedProperty shakePivotProp = serializedObject.FindProperty("_shakePivot");

        SerializedProperty possibleShakeProp = serializedObject.FindProperty("possibleShake");
        SerializedProperty defaultShakeProp = serializedObject.FindProperty("defaultShake");

        SerializedProperty maxShakeTranslateXprop = serializedObject.FindProperty("maxShakeTranslateX");
        SerializedProperty maxShakeTranslateYprop = serializedObject.FindProperty("maxShakeTranslateY");
        SerializedProperty maxShakeAngleProp = serializedObject.FindProperty("maxShakeAngle");
        SerializedProperty perlinNoiseMultiplierProp = serializedObject.FindProperty("perlinNoiseMultiplier");

        SerializedProperty traumaToShakePowerProp = serializedObject.FindProperty("traumaToShakePower");
        SerializedProperty traumaDecreasePerSecProp = serializedObject.FindProperty("traumaDecreasePerSec");

        SerializedProperty traumaXprop = serializedObject.FindProperty("traumaX");
        SerializedProperty traumaYprop = serializedObject.FindProperty("traumaY");
        SerializedProperty traumaRotProp = serializedObject.FindProperty("traumaRot");

        // shake pivot
        EditorGUILayout.LabelField(categoryPivot, EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
            Transform newPivot = EditorGUILayout.ObjectField(shakePivotProp.displayName, 
                shakePivotProp.objectReferenceValue, typeof(Transform), true) as Transform;
        if (EditorGUI.EndChangeCheck())
            shaker.shakePivot = newPivot;

        if (shakePivotProp.objectReferenceValue == null)
            EditorGUILayout.HelpBox(infoNoPivot, MessageType.Info);

        // shake type
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(categoryShakeType, EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(possibleShakeProp);
        EditorGUILayout.PropertyField(defaultShakeProp);

        Shaker2D.Type possibleShake = (Shaker2D.Type)possibleShakeProp.intValue;

        if (possibleShake == 0)
            EditorGUILayout.HelpBox(errorNoPossibleShake, MessageType.Error);

        // shake intensity
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(categoryShakeIntensity, EditorStyles.boldLabel);

        if ((possibleShake & Shaker2D.Type.TranslationX) != Shaker2D.Type.None)
            EditorGUILayout.PropertyField(maxShakeTranslateXprop);
        if ((possibleShake & Shaker2D.Type.TranslationY) != Shaker2D.Type.None)
            EditorGUILayout.PropertyField(maxShakeTranslateYprop);
        if ((possibleShake & Shaker2D.Type.Rotation) != Shaker2D.Type.None)
            EditorGUILayout.PropertyField(maxShakeAngleProp);
        EditorGUILayout.PropertyField(perlinNoiseMultiplierProp);

        // trauma
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(categoryTrauma, EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(traumaToShakePowerProp);
        EditorGUILayout.PropertyField(traumaDecreasePerSecProp);

        EditorGUILayout.Space();

        GUI.enabled = false;
        EditorGUILayout.PropertyField(traumaXprop);
        EditorGUILayout.PropertyField(traumaYprop);
        EditorGUILayout.PropertyField(traumaRotProp);
        GUI.enabled = true;

        // end
        serializedObject.ApplyModifiedProperties();
    }
}
