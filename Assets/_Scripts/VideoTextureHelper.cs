using _Scripts;
using UnityEngine;
using UnityEngine.Video;

public class VideoTextureHelper : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    // Use this to stop video's last frame from appearing
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        
        RenderTexture.active = videoPlayer.targetTexture;
        
        // Make starting frame be purple
        GL.Clear(true, true, Colors.Instance.colorManagerValues.toggledText);
        RenderTexture.active = null;
    }
    
    private void OnEnable()
    {
        while (videoPlayer.targetTexture.IsCreated() == false)
        {
            videoPlayer.targetTexture.Create();
        }

        videoPlayer.Play();

        PlaybackControls.OnPlaybackPressed += VideoTimeChanged;
    }

    private void OnDisable()
    {
        videoPlayer.Stop();
        videoPlayer.targetTexture.Release();
        videoPlayer.targetTexture.DiscardContents();
        PlaybackControls.OnPlaybackPressed -= VideoTimeChanged;
    }

    private void VideoTimeChanged(bool play)
    {
        if (play)
        {
            videoPlayer.Play();
        }
        else
        {
            videoPlayer.Pause();
        }
    }
}
