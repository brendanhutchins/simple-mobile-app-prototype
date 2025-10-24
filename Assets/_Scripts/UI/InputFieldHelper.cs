using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class InputFieldHelper : MonoBehaviour
    {
        [SerializeField] private Questionnaire questionnaire;
        [SerializeField] private ScreenEvents screenEvents;
        [SerializeField] private TMP_InputField[] textMeshProField;
        public string[] responses;

        private void Awake()
        {
            responses = new string[textMeshProField.Length];

            string response = PlayerPrefs.GetString(questionnaire.ToString(), "");
            Debug.Log("Response: " + response);
            if (response != "")
                responses = response.Split('|');
            
            for (int i = 0; i < textMeshProField.Length; i++)
            {
                if (responses[i] != "" && responses[i] != null)
                {
                    textMeshProField[i].text = responses[i];
                    Debug.Log(responses[i]);
                }
            }
        }

        private void OnEnable()
        {
            foreach (TMP_InputField inputField in textMeshProField)
            {
                inputField.onEndEdit.AddListener(delegate { OnValueChanged(); });
            }
        }

        private void Start()
        {
            if (InputBoxesFull())
            {
                screenEvents.ActivateButton(true);
            }
        }

        private void OnValueChanged()
        {
            Debug.Log("Value is changed!");
            // Check if text is blank
            for (int i = 0; i < textMeshProField.Length; i++)
            {
                responses[i] = textMeshProField[i].text;
                screenEvents.ActivateButton(InputBoxesFull());
            }
        }

        // Check if all input boxes are full, if so, save. Helps with avoiding crashes/user exiting
        private bool InputBoxesFull()
        {
            for (int i = 0; i < textMeshProField.Length; i++)
            {
                if (responses[i] == "" || responses[i] == null)
                {
                    return false;
                }
            }

            return true;
        }

        private void OnDisable()
        {
            string response = String.Join("|", responses);
            
            if (InputBoxesFull())
                SaveData.Instance.SaveResponse(questionnaire.ToString(), response);

            foreach (TMP_InputField inputField in textMeshProField)
            {
                inputField.onValueChanged.RemoveListener(delegate { OnValueChanged(); });
            }
        }
    }
}
