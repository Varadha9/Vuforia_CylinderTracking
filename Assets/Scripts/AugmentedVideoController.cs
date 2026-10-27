using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// AugmentedVideoController
/// Controls the playback of the augmented video projected on or alongside the cylinder.
/// Handles loop settings, audio synchronization, and orbital animation of AR graphics.
/// Student: Varad Mandhare (ADT23SOCB1574)
/// Subject: Immersive Application Development - Ex:5
/// </summary>
[RequireComponent(typeof(VideoPlayer))]
public class AugmentedVideoController : MonoBehaviour
{
    [Header("Video Player Settings")]
    public VideoPlayer videoPlayer;
    public VideoClip videoClip;
    public bool playOnAwake = false;
    public bool isLooping = true;

    [Header("Visual Feedback & Orbital Animation")]
    [Tooltip("Optional transform of decorative AR rings or holographic models to rotate around the cylinder.")]
    public Transform rotatingHoloRings;
    public float rotationSpeed = 30f;

    [Header("Audio")]
    public AudioSource audioSource;
    private bool isMuted = false;

    private void Awake()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        ConfigureVideoPlayer();
    }

    private void ConfigureVideoPlayer()
    {
        videoPlayer.playOnAwake = playOnAwake;
        videoPlayer.isLooping = isLooping;
        videoPlayer.renderMode = VideoRenderMode.MaterialOverride;

        if (videoClip != null)
        {
            videoPlayer.clip = videoClip;
        }

        // Setup audio output
        if (audioSource != null)
        {
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoPlayer.EnableAudioTrack(0, true);
            videoPlayer.SetTargetAudioSource(0, audioSource);
        }
        else
        {
            videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
        }
    }

    private void Update()
    {
        // Animate decorative holographic rings if assigned
        if (rotatingHoloRings != null && CylinderTrackableEventHandler.IsCylinderTracked)
        {
            rotatingHoloRings.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    public void PlayVideo()
    {
        if (videoPlayer != null && !videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }

    public void PauseVideo()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
    }

    public void RestartVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        if (audioSource != null)
        {
            audioSource.mute = isMuted;
        }
        else if (videoPlayer != null)
        {
            videoPlayer.SetDirectAudioMute(0, isMuted);
        }
    }
}
