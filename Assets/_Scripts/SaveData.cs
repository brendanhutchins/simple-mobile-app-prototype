using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance { get; set; }

    void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }

    public void SaveResponse(string key, string response)
    {
        PlayerPrefs.SetString(key, response);
        PlayerPrefs.Save();
        //Debug.Log(response);
    }
    
    public void SaveResponse(string key, int response)
    {
        PlayerPrefs.SetInt(key, response);
        PlayerPrefs.Save();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
