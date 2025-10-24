using System;
using _Scripts.Data;
using UnityEngine;
using UnityEngine.Video;

namespace _Scripts
{
    public class AudioFiles : MonoBehaviour
    {
        [SerializeField] private AudioClip[] clips;
        [SerializeField] private ScreenData[] captions;
        [SerializeField] private ScreenChanger screenChanger;
        [SerializeField] private AudioSource audioSource;
        
        [Space] [SerializeField] private PlaybackControls playbackControls;
        [SerializeField] private GameObject playbackButton;
        [SerializeField] private int currentCaptionsDataIndex = 0;

        [Space] [Header("Changeable Audio, Captions, and Screens")] [Space]
        [SerializeField] private AudioClip[] changeableClips;
        [SerializeField] private ScreenData[] changeableCaptions;
        [SerializeField] private GameObject[] screens;

        [SerializeField] private VideoPlayer[] videoPlayers;
        
        private bool _playContent = false;
        private float _timer = 0f;
        private TimeSpan _totalTime;

        private int _currentIndex = 0;
        private CaptionsData[] _captionsData;

        private void OnEnable()
        {
            if (screenChanger != null)
            {
                _currentIndex = screenChanger.StartingScreen;
            }
            PlayAudio(_currentIndex);
            
            PlaybackControls.OnPlaybackTimeChanged += AudioTimeChanged;
        }

        private void OnDisable()
        {
            PlaybackControls.OnPlaybackTimeChanged -= AudioTimeChanged;
        }
        public void PlayAudio(int index)
        {
            if (clips[index] != null)
            {
                audioSource.clip = clips[index];
                
                _totalTime = TimeSpan.FromSeconds((double) new decimal(audioSource.clip.length));
                currentCaptionsDataIndex = 0;

                // Set captions data
                if (captions[index] != null)
                {
                    _captionsData = captions[index].captionsData;
                
                    playbackControls.gameObject.SetActive(true);
                    playbackControls.Reset();
                    playbackControls.SetEndTime(_totalTime);
                    playbackControls.SetCaptions(_captionsData[currentCaptionsDataIndex].bodyText);
                    
                    SetAudioToTimestamp(_captionsData[currentCaptionsDataIndex].TimeStamp);
                    _playContent = true;
                }
                
                audioSource.Play();

                // If this is the last audio clip (or intake questionnaire)
                if ((index == clips.Length - 1 || clips.Length == 1) && playbackButton != null)
                {
                    playbackButton.SetActive(false);
                    playbackControls.gameObject.GetComponent<RectTransform>().localPosition = new Vector2(0f, -390f);
                }
                // Reset position at first screen and hide captions
                else if (index == 0 && playbackButton != null)
                {
                    playbackButton.SetActive(true);
                    playbackControls.gameObject.GetComponent<RectTransform>().localPosition = new Vector2(0f, -321.4f);
                    
                    if (captions[index] == null)
                    {
                        playbackControls.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                audioSource.Stop();
                playbackControls.gameObject.SetActive(false);
            }

            _currentIndex = index;
        }

        private void Update()
        {
            if (!_playContent) return;
            if (audioSource.isPlaying)
            {
                if (_timer < _totalTime.TotalSeconds)
                {
                    playbackControls.SetCurrentTime(TimeSpan.FromSeconds(_timer));

                    if (currentCaptionsDataIndex < _captionsData.Length)
                    {
                        if (_timer > _captionsData[currentCaptionsDataIndex].TimeStamp.TotalSeconds)
                        {
                            playbackControls.SetCaptions(_captionsData[currentCaptionsDataIndex].bodyText);

                            currentCaptionsDataIndex++;
                        }
                    }

                    _timer += Time.deltaTime;
                }
                else
                {
                    EndScreenContent();
                }
            }
            else
            {
                if (_timer >= _totalTime.TotalSeconds)
                {
                    EndScreenContent();
                }
            }
        }

        public void EndScreenContent()
        {
            PlaybackControls.OnPlaybackTimeChanged -= AudioTimeChanged;
            
            _playContent = false;
            playbackControls.SetCurrentTime(TimeSpan.FromSeconds(_timer));
            audioSource.Stop();
        }

        // This is for Lesson 2 screen 3 bar graphs
        public void SetBarGraphScreen()
        {
            var s = PlayerPrefs.GetString(Questionnaire.Sex.ToString());
            switch (s)
            {
                case "0":
                    clips[2] = changeableClips[0];
                    captions[2] = changeableCaptions[0];
                    screens[0].SetActive(true);
                    screens[1].SetActive(false);
                    break;
                case "1":
                    clips[2] = changeableClips[1];
                    captions[2] = changeableCaptions[1];
                    screens[1].SetActive(true);
                    screens[0].SetActive(false);
                    break;
            }
        }

        private void SetAudioToTimestamp(TimeSpan timeStamp)
        {
            _timer = (float) _captionsData[currentCaptionsDataIndex].TimeStamp.TotalSeconds;
            audioSource.time = _timer;
        }

        private void AudioTimeChanged(float val)
        {
            _timer = val;

            for (int i = _captionsData.Length - 1; i >= 0; i--)
            {
                if (_timer > _captionsData[i].TimeStamp.TotalSeconds)
                {
                    currentCaptionsDataIndex = i;
                    _timer = (float) _captionsData[i].TimeStamp.TotalSeconds;
                    break;
                }
            }

            if (videoPlayers.Length != 0)
            {
                if (videoPlayers[_currentIndex] != null)
                {
                    videoPlayers[_currentIndex].time = _timer;
                }
            }
            
            audioSource.time = _timer;
        }
    }
}
