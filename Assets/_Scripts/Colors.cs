using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors : MonoBehaviour
{
    public ColorManagerScriptableObject colorManagerValues;

    public static Colors Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }
}
