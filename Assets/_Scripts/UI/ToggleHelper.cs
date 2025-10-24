using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHelper : MonoBehaviour
{
    public TextMeshProUGUI number;
    public Toggle toggle;

    private void Start()
    {
        if (number != null)
            toggle.onValueChanged.AddListener(value => { ChangeText(value); });
    }

    private void ChangeText(bool isOn)
    {
        var colors = Colors.Instance.colorManagerValues;
        number.color = isOn ? colors.toggledText : colors.untoggledText;
    }
}
