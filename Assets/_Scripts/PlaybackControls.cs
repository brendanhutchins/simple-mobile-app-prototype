using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace _Scripts
{
    // Originally written for Impacto by Elwin, edited for Aliento by Brendan
    public class PlaybackControls : MonoBehaviour
    {
        public GameObject controls;
    
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Image playbackButton;
        [SerializeField] private Sprite pauseButtonSprite;
        [SerializeField] private Sprite playButtonSprite;

        [Space] [SerializeField] private Slider timeProgressBar;
        [SerializeField] private TextMeshProUGUI currentTimeText;
        [SerializeField] private TextMeshProUGUI endTimeText;

        [Space] [SerializeField] private GameObject captionsObject;
        [SerializeField] private TextMeshProUGUI captionsText;

        private TimeSpan currentTime;
        private TimeSpan endTime;
        private bool isReplayingContent;

        private float highestTime = 0f;
        private bool scrubbing = false;

        public static event Action<bool> OnPlaybackPressed;
        public static event Action<float> OnPlaybackTimeChanged;

        private void OnEnable()
        {
            controls.SetActive(true);
            SetPlaybackSprite();
        }

        private void OnDisable()
        {
            timeProgressBar.onValueChanged.RemoveAllListeners();
        }

        private void Update()
        {
            SetPlaybackSprite();
        }

        public void OnPlaybackButtonPressed()
        {
            if (audioSource.isPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }

        public void Play()
        {
            audioSource.Play();
            OnPlaybackPressed?.Invoke(true);
        }

        public void Pause()
        {
            audioSource.Pause();
            OnPlaybackPressed?.Invoke(false);
        }

        public void SetCurrentTime(TimeSpan time)
        {
            currentTime = time;
            currentTimeText.text = currentTime.ToString("mm':'ss");

            if (currentTime.TotalSeconds > highestTime)
                highestTime = (float) currentTime.TotalSeconds;
            
            SetTimeProgressBar();
        }

        public void SetEndTime(TimeSpan time)
        {
            endTime = time;
            endTimeText.text = endTime.ToString("mm':'ss");
        }

        public void OnRewindButtonPressed()
        {
             float progressBarTime = timeProgressBar.value * (float) endTime.TotalSeconds;
             float rewindTime = Mathf.Clamp(progressBarTime - 10f, 0f, (float) endTime.TotalSeconds);
            
            OnPlaybackTimeChanged?.Invoke(rewindTime);
        }

        public void SetCaptions(string s)
        {
            captionsText.text = s;
        }

        public void OnPlaybackSliderChanged(float val)
        {
            currentTime = TimeSpan.FromSeconds((double) new decimal(val * (float) endTime.TotalSeconds));
            currentTimeText.text = currentTime.ToString("mm':'ss");
            
            OnPlaybackTimeChanged?.Invoke(val * (float)endTime.TotalSeconds);
        }

        public void Reset()
        {
            //Debug.Log("Reset working!");
            captionsText.text = "";
            currentTimeText.text = "00:00";

            highestTime = 0f;
            timeProgressBar.value = 0f;
            scrubbing = false;
        }

        public void ProgressBarPressed(BaseEventData eventData)
        {
            if (isReplayingContent == false)
            {
#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
                float progressBarTime = timeProgressBar.value * (float)endTime.TotalSeconds;
                if (progressBarTime > highestTime)
                {
                    SetCurrentTime(TimeSpan.FromSeconds(highestTime));
                    return;
                }
#endif
            }

            scrubbing = true;
            timeProgressBar.onValueChanged.AddListener(OnPlaybackSliderChanged);

            audioSource.Pause();
            OnPlaybackPressed?.Invoke(false);
        }

        public void ProgressBarDrag(BaseEventData eventData)
        {
            if (isReplayingContent == false)
            {
#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
            if (scrubbing)
            {
                float progressBarTime = timeProgressBar.value * (float)endTime.TotalSeconds;
                if (progressBarTime > highestTime)
                {
                    SetCurrentTime(TimeSpan.FromSeconds(highestTime));
                }
            }
#endif
            }
        }

        public void ProgressBarUp(BaseEventData eventData)
        {
            if (scrubbing)
            {
                if (isReplayingContent == false)
                {
#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
                    float progressBarTime = timeProgressBar.value * (float)endTime.TotalSeconds;
                    if (progressBarTime > highestTime)
                    {
                    SetCurrentTime(TimeSpan.FromSeconds(highestTime));
                    }
#endif
                }
                timeProgressBar.onValueChanged.RemoveAllListeners();
                OnPlaybackSliderChanged(timeProgressBar.value);
                
                audioSource.Play();
                OnPlaybackPressed?.Invoke(true);
            }
        }

        private void SetPlaybackSprite()
        {
            if (audioSource.isPlaying)
            {
                playbackButton.sprite = pauseButtonSprite;
            }
            else
            {
                playbackButton.sprite = playButtonSprite;
            }
        }

        private void SetTimeProgressBar()
        {
            timeProgressBar.value = Mathf.Clamp01((float) currentTime.TotalSeconds / (float) endTime.TotalSeconds);
        }
    }
}
