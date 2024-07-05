
using UnityEngine;
using System.Collections.Generic;

public enum QuestType
{
    DownloadFile,
    SendMessage,
    InputCode,
    ClickButton,
    // Tambahkan tipe quest lainnya jika diperlukan
}

[CreateAssetMenu(fileName = "Quest_", menuName = "Scriptable Object/Quest")]
[System.Serializable]
public class QuestSO : ScriptableObject
{
    [System.Serializable]
    public class Quest
    {
        public string requiredFile;
        public string requiredInput;
        public string userToFind;
        public string requiredButtonTag;
        public QuestType questType;
        //public string activeQuestText;
        //public int stepIndex;
    }

    public string questID;
    public string questName;
    public string questChat;
    public int expReward;
    public string overviewChat;
    //public string activeQuestText;
    public string kirimChat;
    public string balasChat;

   
    public List<Quest> questSteps = new List<Quest>();
    [TextArea]public List<string> activeQuestText = new List<string>();
    //public QuestType currentQuestStep;
}
/*
#if UNITY_EDITOR
[CustomEditor(typeof(QuestSO))]
public class QuestSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        QuestSO quest = target as QuestSO;

        // Menampilkan properti umum
        EditorGUILayout.PropertyField(serializedObject.FindProperty("questName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("questDescription"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("expReward"));

        // Menampilkan langkah-langkah quest
        EditorGUILayout.PropertyField(serializedObject.FindProperty("questSteps"), true);

        // Iterasi melalui setiap langkah quest dan menyesuaikan tampilan berdasarkan questType
        for (int i = 0; i < quest.questSteps.Count; i++)
        {
            QuestSO.Quest step = quest.questSteps[i];

            EditorGUI.indentLevel++;

            step.questType = (QuestType)EditorGUILayout.EnumPopup("Quest Type", step.questType);

            switch (step.questType)
            {
                case QuestType.DownloadFile:
                    step.requiredFile = EditorGUILayout.TextField("Required File", step.requiredFile);
                    break;

                case QuestType.SendMessage:
                    step.userToFind = EditorGUILayout.TextField("User To Find", step.userToFind);
                    break;

                case QuestType.InputCode:
                    step.requiredInput = EditorGUILayout.TextField("Required Input", step.requiredInput);
                    break;

                    // Tambahkan tipe quest lainnya jika diperlukan
            }

            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif*/