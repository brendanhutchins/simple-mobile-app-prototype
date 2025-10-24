using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;

public class MultiLanguage : MonoBehaviour
{
    private void Awake()
    {
        LocalizationManager.Read();
        // Debug purposes uncomment below
        //switch (Application.systemLanguage)
        //{
        //    case SystemLanguage.Spanish:
        //        LocalizationManager.Language = "Spanish";
        //        break;
        //    default:
        //        LocalizationManager.Language = "English";
        //        break;
        //}
        LocalizationManager.Language = "Spanish";
    }

    public void Language(string language)
    {
        LocalizationManager.Language = language;
    }
}
