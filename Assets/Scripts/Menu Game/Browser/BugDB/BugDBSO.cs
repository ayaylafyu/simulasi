using UnityEngine;

[CreateAssetMenu(fileName = "BugDBSO", menuName = "Scriptable Object/Bug DB")]
public class BugDBSO : ScriptableObject
{
    [System.Serializable]
    public class BugData
    {
        public string serviceName;
        public string versionNumber;
        public string downloadButtonText;
    }

    public BugData[] bugs;

    public BugData GetBugData(int index)
    {
        if (index >= 0 && index < bugs.Length)
        {
            return bugs[index];
        }
        else
        {
            Debug.LogError("Invalid index for BugDBSO");
            return null;
        }
    }
}
