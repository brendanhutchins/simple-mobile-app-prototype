using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHelper : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Sprite toggledImage;
    [SerializeField] private Button button;

    void Awake()
    {
        if (!button)
            button = GetComponent<Button>();
    }

    private void Update()
    {
        if (button.interactable)
            text.alpha = 1.0f;
        else
            text.alpha = 0.4f;
    }
}
