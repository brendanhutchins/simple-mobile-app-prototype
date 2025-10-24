using System;
using System.Collections;
using System.Globalization;
using _Scripts.Firebase;
using UnityEngine;

namespace _Scripts
{
    public class TimeTracking : MonoBehaviour
    {
        [SerializeField] private GameObject reviewStars;
        public GameObject onBoardingScreen;
        public GameObject mainMenuScreen;
        [SerializeField] private MenuButtons[] menuButtons;
        
        private string _lastDate;
        private float startingTotalTime;

        private void Awake()
        {
            if (PlayerPrefs.HasKey("totalTime"))
            {
                startingTotalTime = PlayerPrefs.GetFloat("totalTime");
            }

            // Set total days at beginning, and total time at application quit
            SaveTotalDays();
        }

        private void Start()
        {
            // If the player has already completed the full app
            if (PlayerPrefs.HasKey("didCompleteApp"))
            {
                onBoardingScreen.SetActive(false);

                for (int i = 0; i < menuButtons.Length; i++)
                {
                    // Review Button
                    if (i == menuButtons.Length - 1)
                    {
                        menuButtons[i].ReviewButton(true);
                    }
                    else
                    {
                        menuButtons[i].ActivateButton(true);
                    }
                }
            }
        }

        // Account for if application is closed not via quitting and instead user just pauses
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                // App is paused

                // Set total time when app is paused
                SaveTotalTime();
                FirebaseManager.Instance.SaveSessionMetrics();
                Debug.Log("App is paused!");
            }
            else
            {
                // App resumed

                // Set total days at beginning
                SaveTotalDays();
            }
        }

        // Set total time on application quit
        void OnApplicationQuit()
        {
            SaveTotalTime();
            
            // Check if user finished the whole application
            if (reviewStars.activeSelf)
            {
                PlayerPrefs.SetInt("didCompleteApp", 1);
            }
            FirebaseManager.Instance.SaveSessionMetrics();
        }

        // Make sure participant quits the app in order to correctly save total time spent
        private float TotalTime
        {
            get
            {
                // Check if last totalTime is more than previous totalTime on system pause event
                // We need to save to TotalTime when app is paused in case user closes app via
                // system tray or the home button, but we also need to account for user opening app
                // again after clicking home button.

                // On Quit
                float totalTime = Time.time;

                if (PlayerPrefs.HasKey("totalTime"))
                {
                    //fullTime = totalTime + startingTotalTime;

                    // If the saved total time and the total time when the application started are not equal
                    // (in other words, if it was saved already by pausing)
                    if (Math.Abs(PlayerPrefs.GetFloat("totalTime") - startingTotalTime) > 0.1f)
                    {
                        float zeroedValue = PlayerPrefs.GetFloat("totalTime") - startingTotalTime;

                        Debug.Log("Zeroed value: " + zeroedValue);
                        Debug.Log("Total time (time.time): " + totalTime);

                        totalTime += (PlayerPrefs.GetFloat("totalTime") - zeroedValue);
                    }
                    // On application quit (saved total time and starting total time are the same)
                    else
                    {
                        totalTime += PlayerPrefs.GetFloat("totalTime");
                        Debug.Log("Application closed/paused first time! Final time: " + totalTime);
                    }
                }

                return totalTime;
            }
        }

        private int TotalDays
        {
            get
            {
                // Always initialize with one to signify starting day
                // Ask client if they want to always have this be one or zero
                int totalDays = 0;
                if (PlayerPrefs.HasKey("totalDays"))
                {
                    totalDays += PlayerPrefs.GetInt("totalDays");
                    // If today is not the same as the last saved date, add to total days
                    if (_lastDate != DateTime.Now.Date.ToString(CultureInfo.InvariantCulture))
                    {
                        totalDays++;
                    }
                }
                else
                {
                    totalDays++;
                }

                // Set Last Date to be the date of today
                PlayerPrefs.SetString("lastDate", DateTime.Now.Date.ToString(CultureInfo.InvariantCulture));
                return totalDays;
            }
        }

        private void SaveTotalDays()
        {
            _lastDate = PlayerPrefs.GetString("lastDate");
            Debug.Log(_lastDate);
            PlayerPrefs.SetInt("totalDays", TotalDays);
            Debug.Log(PlayerPrefs.GetInt("totalDays"));
        }

        private void SaveTotalTime()
        {
            PlayerPrefs.SetFloat("totalTime", TotalTime);
            Debug.Log(PlayerPrefs.GetFloat("totalTime"));
        }
        
        // Debugging purposes
        // private void OnApplicationFocus(bool pauseStatus)
        // {
        //     if (pauseStatus)
        //     {
        //         // your app is NO LONGER in the background
        //         // Set total days at beginning
        //         SaveTotalDays();
        //     }
        //     else
        //     {
        //         // your app is now in the background
        //         // Set total time when app is paused
        //         SaveTotalTime();
        //         Debug.Log("App is paused!");
        //     }
        // }
    }

}
