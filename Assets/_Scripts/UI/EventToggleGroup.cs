using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ToggleGroup))]
public class EventToggleGroup : MonoBehaviour
{
    [System.Serializable]
    public class ToggleEvent : UnityEvent<Toggle> { }
    
    [System.Serializable]
    public class UntoggleEvent : UnityEvent { }

    
    [SerializeField] private Toggle[] toggles;
    [SerializeField] public ToggleEvent onActiveTogglesChanged;
    [SerializeField] public UntoggleEvent onTogglesOff;
    [SerializeField] private bool multipleChoice;
    public Questionnaire questionnaire;
    public string[] checkedToggles;
    private ToggleGroup _toggleGroup;
    private bool _isPrefilled;

    private void Awake()
    {
        checkedToggles = new string[toggles.Length];
        _toggleGroup = GetComponent<ToggleGroup>();
        
        string toggleNum = PlayerPrefs.GetString(questionnaire.ToString(), "");
        
        // Multiple choice
        if (toggleNum != "" && multipleChoice)
        {
            char[] charArray = toggleNum.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                string s = charArray[i].ToString();
                checkedToggles[int.Parse(s)] = s;
            }

            for (int i = 0; i < toggles.Length; i++)
            {
                if (checkedToggles[i] != "" && checkedToggles[i] != null && checkedToggles[i] == i.ToString())
                {
                    toggles[i].isOn = true;
                    _isPrefilled = true;
                }
            }
        }
        // Only one toggle at a time
        else if (toggleNum != "")
        {
            int i = 0;
            foreach (Toggle toggle in toggles)
            {
                if (toggle.group != null && toggle.group != _toggleGroup)
                {
                    Debug.LogError("EventToggleGroup is trying to register a Toggle that is a member of another group.");
                }

                toggle.group = _toggleGroup;
            
                if (int.Parse(toggleNum) == i && toggleNum != "")
                {
                    toggle.isOn = true;
                    _isPrefilled = true;
                }

                i++;
            }
        }
    }

    void OnEnable()
    {
        foreach (Toggle toggle in toggles)
        {
            // Fire HandleToggleValueChanged Event everytime a toggle is checked/unchecked
            toggle.onValueChanged.AddListener(HandleToggleValueChanged);
        }
    }

    private void Start()
    {
        if (_isPrefilled)
            HandleToggleValueChanged(true);
    }

    void HandleToggleValueChanged(bool isOn)
    {
        // Active toggle event is fired if Next button is turned on
        if (isOn)
        {
            // Execute events
            onActiveTogglesChanged?.Invoke(_toggleGroup.ActiveToggles().FirstOrDefault());
            _isPrefilled = true;
            
            // See if toggle is on, if so, set it's i value to be in checkedToggles[]
            for (int i = 0; i < toggles.Length; i++)
            {
                if (toggles[i].isOn)
                {
                    checkedToggles[i] = i.ToString();
                }
                else
                {
                    checkedToggles[i] = "";
                }
            }
        }
        // If toggles are turned off
        
        // Multiple choice
        if (multipleChoice)
        {
            bool hasOnToggle = false;
            int i = 0;
            foreach (Toggle toggle in toggles)
            {
                if (toggle.isOn)
                    hasOnToggle = true;
                else
                    checkedToggles[i] = "";
                i++;
            }

            if (!hasOnToggle)
            {
                onTogglesOff.Invoke();
                _isPrefilled = false;
            }
        }
        // Single toggle
        else
        {
            if (!_toggleGroup.AnyTogglesOn())
            {
                onTogglesOff.Invoke();
                _isPrefilled = false;
                for (int i = 0; i < checkedToggles.Length; i++)
                {
                    checkedToggles[i] = "";
                }
            }
        }
    }

    void OnDisable()
    {
        if (_isPrefilled)
        {
            string s = String.Join(null, checkedToggles);
            SaveData.Instance.SaveResponse(questionnaire.ToString(), s);
        }

        foreach (Toggle toggle in _toggleGroup.ActiveToggles())
        {
            toggle.onValueChanged.RemoveListener(HandleToggleValueChanged);
            toggle.group = null;
        }
    }
}
