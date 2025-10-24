using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject prevMenu;
    [SerializeField] private GameObject prevMenuStars;
    [SerializeField] private GameObject menuStars;
    [SerializeField] private Sprite activatedSprite;
    [SerializeField] private Sprite deactivatedSprite;

    [SerializeField] private GameObject outGoingScreen;
    [SerializeField] private GameObject outGoingButton;
    [SerializeField] private GameObject mainMenuScreen;

    public void ActivateButton(bool isRepeat = false)
    {
        GetComponent<Button>().interactable = true;
        
        // Check if current lesson is already completed, if not activate button
        if (!menuStars.activeSelf  && !isRepeat)
        {
            GetComponent<Button>().image.sprite = activatedSprite;
            text.color = Colors.Instance.colorManagerValues.toggledText;
        }

        // Check if previous lesson is completed, if so stop highlighting button
        if (!prevMenuStars.activeSelf)
        {
            prevMenu.GetComponent<Button>().image.sprite = deactivatedSprite;
            prevMenu.GetComponentInChildren<TextMeshProUGUI>().color = Colors.Instance.colorManagerValues.untoggledText;
            prevMenuStars.SetActive(true);
        }
    }

    public void ReviewButton(bool isStartup = false)
    {
        ActivateButton();
        
        GetComponent<Button>().image.sprite = deactivatedSprite;
        GetComponentInChildren<TextMeshProUGUI>().color = Colors.Instance.colorManagerValues.untoggledText;
        menuStars.SetActive(true);
        
        outGoingButton.SetActive(true);
        
        // If the app is starting up, skip this step
        if (isStartup) return;
        
        if (!PlayerPrefs.HasKey("didCompleteApp"))
        {
            outGoingScreen.SetActive(true);
        }
        else
        {
            mainMenuScreen.SetActive(true);
        }
    }
}
