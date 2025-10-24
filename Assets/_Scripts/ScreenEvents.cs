using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class OnEnableEvent : UnityEvent {}

public class ScreenEvents : MonoBehaviour
{
    private Button _button;
    public bool nextButtonDependent;
    public bool nextButtonChecked;
    public ScreenChanger content;
    [SerializeField] private Button nextButton;

    [Space]
    public OnEnableEvent OnEnableEvent;

    private void Awake()
    {
        if (nextButtonDependent)
        {
            if (content != null)
            {
                _button = content.NextButton.GetComponent<Button>();
            }
            else
            {
                _button = nextButton;
            }
        }
            
    }
    
    void OnEnable()
    {
        OnEnableEvent.Invoke();
    }
    
    public void ActivateButton(bool isOn)
    {
        nextButtonChecked = isOn;
        _button.interactable = isOn;
        //_button.GetComponent<ButtonHelper>().text.alpha = isOn ? 0.6f : 1.0f;
    }
}
