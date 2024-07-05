using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Apreciate_", menuName = "Scriptable Object/Apreciate")]
public class ApreciateSO : ScriptableObject
{
    public string titleApre;
    public Sprite imageApre;
    [TextArea]
    public string descApre;
}
