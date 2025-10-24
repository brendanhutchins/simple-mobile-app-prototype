using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptionsHelper : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform textRectTransform;

    void Update()
    {
        var hDelta = textRectTransform.sizeDelta.y;

        switch (hDelta)
        {
            // One line
            case 22.01f:
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -50.0f);
                break;
            
            // Two lines
            case 48.69f:
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -40.0f);
                break;
            
            // Three lines
            case 75.38f:
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -25.0f);
                break;
            
            // Four lines
            case 102.06f:
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -10.0f);
                break;

            // Five lines
            case 128.75f:
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0.0f);
                break;
            
            // Six lines
            case 155.44f:
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 15.0f);
                break;
            
            case 182.12f:
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 30.0f);
                break;
            // default:
            //     rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0.0f);
            //     break;
        }
    }
}
