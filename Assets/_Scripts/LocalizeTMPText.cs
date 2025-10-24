using Assets.SimpleLocalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizeTMPText : MonoBehaviour
    {
        public string localizationKey;

        public void SetLocalizationKey(string key)
        {
            localizationKey = key;
            Localize();
            LocalizationManager.LocalizationChanged += Localize;
        }
        
        public void Start()
        {
            Localize();
            LocalizationManager.LocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            LocalizationManager.LocalizationChanged -= Localize;
        }

        protected virtual void Localize()
        {
            string s = LocalizationManager.Localize(localizationKey).Replace("\\n", System.Environment.NewLine);
            GetComponent<TextMeshProUGUI>().text = s;
            //Debug.Log(GetComponent<TextMeshProUGUI>().text);
        }
    }
}
