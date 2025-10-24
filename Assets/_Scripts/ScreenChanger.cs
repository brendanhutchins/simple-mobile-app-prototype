using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts
{
    public class ScreenChanger : MonoBehaviour
    {
        internal RectTransform _screensContainer;
        internal int _screens = 1;
        internal int _currentPage;
        internal int _previousPage;
    
        [Serializable]
        public class SelectionPageChangedEvent : UnityEvent<int> { }

        [Tooltip("The container that holds all of our panels to switch to.")]
        public RectTransform Content;
    
        [Tooltip("The screen / page to start the control on\n*Note, this is a 0 indexed array")]
        [SerializeField]
        public int StartingScreen = 0;
    
        [Tooltip("The gameobject that contains toggles which suggest pagination. (optional)")]
        public GameObject Pagination;

        public TextMeshProUGUI PaginationText;
    
        [Tooltip("Button to go to the previous page. (optional)")]
        public GameObject PrevButton;

        [Tooltip("Button to go to the next page. (optional)")]
        public GameObject NextButton;

        public int CurrentPage
        {
            get { return _currentPage; }

            set
            {
                if ((value != _currentPage && value >= 0 && value < _screensContainer.childCount) ||
                    (value == 0 && _screensContainer.childCount == 0))
                {
                    _previousPage = _currentPage;
                    _currentPage = value;
                    UpdateVisible();
                    ScreenChange();
                    OnCurrentScreenChange(_currentPage);
                }
            }
        }
    
        [Tooltip("Scroll Snap children. (optional)\nEither place objects in the scene as children OR\nPrefabs in this array, NOT BOTH")]
        public GameObject[] ChildObjects;
    
        [SerializeField]
        [Tooltip("Event fires as the page changes, while dragging or jumping")]
        private SelectionPageChangedEvent m_OnSelectionPageChangedEvent = new SelectionPageChangedEvent();
        public SelectionPageChangedEvent OnSelectionPageChangedEvent { get { return m_OnSelectionPageChangedEvent; } set { m_OnSelectionPageChangedEvent = value; } }
    
        // Use this for initialization
        void Awake()
        {
            if (StartingScreen < 0)
            {
                StartingScreen = 0;
            }

            try
            {
                _screensContainer = Content;
            }
            catch
            {
                Debug.LogError("No content has been set in the screen changer!");
                return;
            }
        
            InitializeChildObjects();
        }

        void Start()
        {
            _currentPage = StartingScreen;
            UpdateLayout();
        
            // Used to be in awake function, move back if issues arise
            if (NextButton)
                NextButton.GetComponent<Button>().onClick.AddListener(() => { NextScreen(); });
        
            if (PrevButton)
                PrevButton.GetComponent<Button>().onClick.AddListener(() => { PreviousScreen(); });
        }

        internal void InitializeChildObjects()
        {
            // If the childObjects array has any children in it
            if (ChildObjects != null && ChildObjects.Length > 0)
            {
                if (_screensContainer.transform.childCount > 0)
                {
                    Debug.LogError("Content has children, this is not supported when using managed Child Objects\n " +
                                   "Either remove the Content children or clear the ChildObjects arrary");
                    return;
                }
                // Add this later if needed
                //InitializeChildObjectsFromArray();
            }
            else
            {
                InitializeChildObjectsFromScene();
            }
        }

        internal void InitializeChildObjectsFromScene()
        {
            int childCount = _screensContainer.childCount;
            ChildObjects = new GameObject[childCount];
            for (int i = 0; i < childCount; i++)
            {
                ChildObjects[i] = _screensContainer.transform.GetChild(i).gameObject;
            
                if (ChildObjects[i].activeSelf)// && i != CurrentPage)
                {
                    ChildObjects[i].SetActive(false);
                }
            }
        }

        internal void UpdateVisible()
        {
            // If there are no objects in the scene, exit
            if (ChildObjects == null || ChildObjects.Length < 1 || _screensContainer.childCount < 1)
            {
                return;
            }

            for (int i = 0; i < ChildObjects.Length; i++)
            {
                if (i == CurrentPage)
                {
                    ChildObjects[i].SetActive(true);
                }
                else
                {
                    ChildObjects[i].SetActive(false);
                }
            }
        }
    
        // Function for switching to the next screen
        public void NextScreen()
        {
            if (_currentPage < _screens - 1)
            {
                CurrentPage = _currentPage + 1;
                ScreenChange();
            }
            else if (_currentPage == _screens - 1)
            {
                CurrentPage = 0;
            }
        }
    
        // Function for switching to the previous screen
        public void PreviousScreen()
        {
            if (_currentPage > 0)
            {
                CurrentPage = _currentPage - 1;
                ScreenChange();
            }
        }
    
        /// <summary>
        /// notifies pagination indicator and navigation buttons of a screen change
        /// </summary>
        internal void OnCurrentScreenChange(int currentScreen)
        {
            ChangeBulletsInfo(currentScreen);
            ToggleNavigationButtons(currentScreen);
            //ToggleGroup toggleGroup;
        }
    
        /// <summary>
        /// changes the bullets on the bottom of the page - pagination
        /// </summary>
        /// <param name="targetScreen"></param>
        private void ChangeBulletsInfo(int targetScreen)
        {
            if (Pagination)
            {
                for (int i = 0; i < Pagination.transform.childCount; i++)
                {
                    Pagination.transform.GetChild(i).GetComponent<Toggle>().isOn = (targetScreen == i) ? true : false;
                }
            }

            // subtract two to account for title screen & badge screen
            if (PaginationText)
            {
                PaginationText.text = CurrentPage + "/" + (_screens - 2);
            }
        }
    
        /// <summary>
        /// disables the page navigation buttons when at the first or last screen
        /// </summary>
        /// <param name="targetScreen"></param>
        private void ToggleNavigationButtons(int targetScreen)
        {
            if (PrevButton)
            {
                Button prevButton = PrevButton.GetComponent<Button>();
                TextMeshProUGUI prevButtonText = PrevButton.GetComponent<ButtonHelper>().text;
                
                prevButton.interactable = targetScreen > 0;
                
                // Disable button on badge screen
                bool isActive = targetScreen < _screensContainer.transform.childCount - 1;
                prevButton.gameObject.SetActive(isActive);
                
                
                if (prevButton.interactable)
                    prevButtonText.alpha = 1.0f;
                else
                {
                    //prevButtonText.color = Colors.Instance.colorManagerValues.borderButtonText;
                    prevButtonText.alpha = 0.4f;
                }
            }

            if (NextButton)
            {
                // Get simplified components
                Button nextButton = NextButton.GetComponent<Button>();
                TextMeshProUGUI nextButtonText = NextButton.GetComponent<ButtonHelper>().text;
                ScreenEvents screenEvents = ChildObjects[CurrentPage].GetComponent<ScreenEvents>();
            
                // Turn off button on last screen, else enable and set active
                bool isActive = targetScreen < _screensContainer.transform.childCount - 1;
                nextButton.interactable = isActive;
                nextButton.gameObject.SetActive(isActive);
                
                // Switch text to say complete when at the screen before the badge
                if (targetScreen == _screensContainer.transform.childCount - 2)
                {
                    nextButton.GetComponentInChildren<LocalizeTMPText>().SetLocalizationKey("Text.Complete");
                }
                else
                {
                    nextButton.GetComponentInChildren<LocalizeTMPText>().SetLocalizationKey("Text.Next");
                }

                // If screen events exist and the answers activate the next button...
                if (screenEvents != null && screenEvents.nextButtonDependent)
                {
                    nextButton.interactable = screenEvents.nextButtonChecked;

                    // Button IS active
                    if (nextButton.interactable)
                        nextButtonText.alpha = 1.0f;
                    // Button IS NOT active
                    else
                    {
                        //nextButtonText.color = Colors.Instance.colorManagerValues.borderButtonText;
                        nextButtonText.alpha = 0.4f;
                    }
                }
            }
        }

        private void ScreenChange()
        {
            OnSelectionPageChangedEvent.Invoke(_currentPage);
        }

        /// <summary>
        /// used for changing / updating between screen resolutions
        /// </summary>
        public void UpdateLayout()
        {
            // from DistributePages()
            _screens = _screensContainer.childCount;
            UpdateVisible();
            OnCurrentScreenChange(_currentPage);
        }

        private void OnEnable()
        {
            InitializeChildObjectsFromScene();
            UpdateLayout();
            NextButton.SetActive(true);
            PrevButton.SetActive(true);
        }
    }
}
