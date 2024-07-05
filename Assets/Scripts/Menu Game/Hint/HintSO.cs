using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hint_", menuName = "Scriptable Object/Hint")]
public class HintSO : ScriptableObject
{
    public string titleHint;
    public Sprite imageHint;
    [TextArea]
    public string descHint;
}
