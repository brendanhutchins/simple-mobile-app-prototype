using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class PageDots : MonoBehaviour
    {
        [SerializeField] private Image[] dots;

        [SerializeField] private Sprite selectedColor;
        [SerializeField] private Sprite unSelectedColor;

        private int _currentIndex = 0;

        private void OnEnable()
        {
            UpdateDots(_currentIndex);
        }

        public void UpdateDots(int index)
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].sprite = unSelectedColor;
                if (i == index)
                    dots[i].sprite = selectedColor;
            }
            
            _currentIndex = index;
        }
    }
}
