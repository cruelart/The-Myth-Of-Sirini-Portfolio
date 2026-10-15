using UnityEditor;
using UnityEngine;

// 이 스크립트는 QuestData 클래스의 커스텀 에디터를 커스텀하기 위해 만들어봄

[CustomEditor(typeof(QuestData))]
public class QuestDataEditor : Editor
{
    private SerializedProperty questID;
    private SerializedProperty questName;
    private SerializedProperty questDescription;
    private SerializedProperty requirements;

    private SerializedProperty startNpcName;
    private SerializedProperty startNpcImage;
    private SerializedProperty startNpcTransform;

    private SerializedProperty endNpcName;
    private SerializedProperty endNpcImage;
    private SerializedProperty endNpcTransform;

    private void OnEnable()
    {
        questID = serializedObject.FindProperty("questID");
        questName = serializedObject.FindProperty("questName");
        questDescription = serializedObject.FindProperty("questDescription");
        requirements = serializedObject.FindProperty("requirements");

        startNpcName = serializedObject.FindProperty("StartNpcName");
        startNpcImage = serializedObject.FindProperty("StartNpcImage");
        startNpcTransform = serializedObject.FindProperty("StartNpcTransform");

        endNpcName = serializedObject.FindProperty("EndNpcName");
        endNpcImage = serializedObject.FindProperty("EndNpcImage");
        endNpcTransform = serializedObject.FindProperty("EndNpcTransform");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(questID);
        EditorGUILayout.PropertyField(questName);
        EditorGUILayout.PropertyField(questDescription);

        EditorGUILayout.PropertyField(startNpcName);
        EditorGUILayout.PropertyField(startNpcImage);
        EditorGUILayout.PropertyField(startNpcTransform);

        EditorGUILayout.PropertyField(endNpcName);
        EditorGUILayout.PropertyField(endNpcImage);
        EditorGUILayout.PropertyField(endNpcTransform);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Quest Requirements", EditorStyles.boldLabel);

        for (int i = 0; i < requirements.arraySize; i++)
        {
            SerializedProperty requirement = requirements.GetArrayElementAtIndex(i);

            SerializedProperty type =  requirement.FindPropertyRelative("requireType");

            SerializedProperty targetID = requirement.FindPropertyRelative("targetID");

            SerializedProperty targetType = requirement.FindPropertyRelative("targetType");

            SerializedProperty requiredCount = requirement.FindPropertyRelative("requiredCount");

            SerializedProperty questText = requirement.FindPropertyRelative("questText");


            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField($"Requirement {i + 1}", EditorStyles.boldLabel);


            EditorGUILayout.PropertyField(type);


            QuestRequirementType requirementType = (QuestRequirementType)type.enumValueIndex;


            switch (requirementType)
            {
                case QuestRequirementType.Kill:

                    EditorGUILayout.PropertyField(targetType, new GUIContent("Target Type"));

                    EditorGUILayout.PropertyField(targetID, new GUIContent("Target ID"));

                    EditorGUILayout.PropertyField(requiredCount, new GUIContent("Required Kill Count"));

                    EditorGUILayout.PropertyField(questText, new GUIContent("Quest Text"));

                    break;


                case QuestRequirementType.CollectItem:

                    EditorGUILayout.PropertyField(targetID,new GUIContent("Item ID"));

                    EditorGUILayout.PropertyField(requiredCount, new GUIContent("Required Item Count"));

                    EditorGUILayout.PropertyField(questText, new GUIContent("Quest Text"));

                    break;


                case QuestRequirementType.TalkNpc:

                    EditorGUILayout.PropertyField(targetID, new GUIContent("NPC ID"));

                    EditorGUILayout.PropertyField(questText, new GUIContent("Quest Text"));

                    //얘는 굳이 갯수가 필요하지 않으니까 안보이게 설정

                    break;
            }


            if (GUILayout.Button("Remove Requirement"))
            {
                requirements.DeleteArrayElementAtIndex(i);

                EditorGUILayout.EndVertical();
                break;
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
        }


        if (GUILayout.Button("+ Add Requirement"))
        {
            int index = requirements.arraySize;

            requirements.InsertArrayElementAtIndex(index);

            SerializedProperty newRequirement =
                requirements.GetArrayElementAtIndex(index);

            newRequirement.FindPropertyRelative("requireType").enumValueIndex = (int)QuestRequirementType.Kill;

            newRequirement.FindPropertyRelative("targetID").intValue = 0;
            newRequirement.FindPropertyRelative("requiredCount").intValue = 1;
        }


        serializedObject.ApplyModifiedProperties();
    }
}
