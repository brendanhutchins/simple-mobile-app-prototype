using UnityEngine;

public class DebugButtons : MonoBehaviour
{
    [SerializeField] private GameObject button1;
    [SerializeField] private GameObject button2;
    private bool _isButtonOn;

    public void ShowHideButtons()
    {
        if (!_isButtonOn)
        {
            button1.SetActive(true);
            button2.SetActive(true);
            _isButtonOn = true;
        }
        else
        {
            button1.SetActive(false);
            button2.SetActive(false);
            _isButtonOn = false;
        }
    }
}
