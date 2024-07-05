using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "LogMisi_", menuName = "Scriptable Object/Log Misi")]
public class LogSO : ScriptableObject
{
    public string nameLog;
    [TextArea]
    public string descLog;
}
