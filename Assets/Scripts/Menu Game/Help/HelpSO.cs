using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Help_", menuName = "Scriptable Object/Help")]
public class HelpSO : ScriptableObject
{
    public string titleHelp;
    public Sprite imageHelp;
    [TextArea]
    public string descHelp;
}
