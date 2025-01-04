using UnityEditor;
using UnityEngine;

public enum EmotionType
{
    Joy,
    Sadness,
    Anger
}

[CreateAssetMenu(fileName = "NewCharacterEmotion", menuName = "Character Emotion")]
public class CharacterEmotion : ScriptableObject 
{
    public CustomInspectorObjectsForEmotions customInsObjEmotions;

    public void PerformSpecialAction()
    {
        if (customInsObjEmotions.emotionType == EmotionType.Joy)
        {
        }
        else if (customInsObjEmotions.emotionType == EmotionType.Sadness)
        {
        }
        else if (customInsObjEmotions.emotionType == EmotionType.Anger)
        {
        }
    }
}

[System.Serializable]
public class CustomInspectorObjectsForEmotions
{
    public EmotionType emotionType;
    //[HideInInspector] public GameObject weaponPrefab; 
    //[HideInInspector] public float attackCooldown;

    [HideInInspector] public float dashCoolDown;
    [HideInInspector] public float dashDuration;
    [HideInInspector] public float dashSpeed;

    [HideInInspector] public float dashDistance;

    [HideInInspector] public float angerDashTimeScale;
    [HideInInspector] public float angerMaxDashDistance;
}

[CustomEditor(typeof(CharacterEmotion))]
public class CharacterEmotionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CharacterEmotion characterEmotion = (CharacterEmotion)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        characterEmotion.customInsObjEmotions.dashCoolDown = EditorGUILayout.FloatField("Dash Cooldown", characterEmotion.customInsObjEmotions.dashCoolDown);
        characterEmotion.customInsObjEmotions.dashDuration = EditorGUILayout.FloatField("Dash Duration", characterEmotion.customInsObjEmotions.dashDuration);

        if (characterEmotion.customInsObjEmotions.emotionType == EmotionType.Joy)
        {
            EditorGUILayout.LabelField("Joy Specific Settings", EditorStyles.boldLabel);
            characterEmotion.customInsObjEmotions.dashSpeed = EditorGUILayout.FloatField("Dash Speed", characterEmotion.customInsObjEmotions.dashSpeed);
            characterEmotion.customInsObjEmotions.dashDistance = EditorGUILayout.FloatField("Dash Distance", characterEmotion.customInsObjEmotions.dashDistance);
            characterEmotion.customInsObjEmotions.emotionType = EmotionType.Joy;
        }
        else if (characterEmotion.customInsObjEmotions.emotionType == EmotionType.Sadness)
        {
            EditorGUILayout.LabelField("Sadness Specific Settings", EditorStyles.boldLabel);
            characterEmotion.customInsObjEmotions.dashSpeed = EditorGUILayout.FloatField("Dash Speed", characterEmotion.customInsObjEmotions.dashSpeed);
            characterEmotion.customInsObjEmotions.dashDistance = EditorGUILayout.FloatField("Dash Distance", characterEmotion.customInsObjEmotions.dashDistance);
            characterEmotion.customInsObjEmotions.emotionType = EmotionType.Sadness;
        }
        else if (characterEmotion.customInsObjEmotions.emotionType == EmotionType.Anger)
        {
            EditorGUILayout.LabelField("Anger Specific Settings", EditorStyles.boldLabel);
            characterEmotion.customInsObjEmotions.angerMaxDashDistance = EditorGUILayout.FloatField("Max Dash Distance", characterEmotion.customInsObjEmotions.angerMaxDashDistance);
            characterEmotion.customInsObjEmotions.angerDashTimeScale = EditorGUILayout.FloatField("Dash Time Scale", characterEmotion.customInsObjEmotions.angerDashTimeScale);
            characterEmotion.customInsObjEmotions.emotionType = EmotionType.Anger;
        }


        if (GUI.changed)
        {
            EditorUtility.SetDirty(characterEmotion);
        }
    }
}




