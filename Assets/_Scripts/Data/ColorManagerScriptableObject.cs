using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ColorManagerScriptableObject", order = 1)]
public class ColorManagerScriptableObject : ScriptableObject
{
    public Color toggledText;
    public Color untoggledText;
    public Color borderButtonText;
}
