using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BackwardsId : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        // Give ID backwards
        string s = PlayerPrefs.GetString("Id");

        char[] stringArray = s.ToCharArray();
        Array.Reverse(stringArray);
        string reversedStr = new string(stringArray);
        text.text = reversedStr;
    }
}
