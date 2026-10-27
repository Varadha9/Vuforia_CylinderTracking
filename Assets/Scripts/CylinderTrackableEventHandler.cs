using System;
using UnityEngine;
using UnityEngine.Video;
using Vuforia;

/// <summary>
/// CylinderTrackableEventHandler
/// Handles optical target acquisition and loss events for the Vuforia Cylinder Target.
/// Automatically starts/pauses augmented video playback and toggles augmented image cards.
/// Includes Editor simulation mode (press 'T' to toggle tracking in Unity Play Mode).
/// Student: Varad Mandhare (ADT23SOCB1574)
/// Subject: Immersive Application Development - Ex:5
/// </summary>
public class CylinderTrackableEventHandler : MonoBehaviour
{
    [Header("Vuforia Observer Reference")]
    [Tooltip("Reference to the ObserverBehaviour (Cylinder Target). If null, will search on this GameObject.")]
    public ObserverBehaviour mObserverBehaviour;

    [Header("Augmented Objects")]
    [Tooltip("The augmented video display GameObject containing the VideoPlayer.")]
    public GameObject augmentedVideoObject;

    [Tooltip("The augmented image overlay card GameObject.")]
    public GameObject augmentedImageCardObject;

    [Tooltip("3D holographic decorative elements (e.g. alignment rings).")]
    public GameObject augmented3DElements;

    [Header("Video Playback")]
    public VideoPlayer videoPlayer;

    [Header("Status Callbacks")]
    public static bool IsCylinderTracked { get; private set; } = false;
    public static string TrackingStatusMessage { get; private set; } = "Initializing Vuforia Engine...";

    private void Awake()
    {
        if (mObserverBehaviour == null)
        {
            mObserverBehaviour = GetComponent<ObserverBehaviour>();
        }

        if (videoPlayer == null && augmentedVideoObject != null)
        {
            videoPlayer = augmentedVideoObject.GetComponent<VideoPlayer>();
        }
    }

    private void Start()
    {
        if (mObserverBehaviour != null)
        {
            mObserverBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
        else
        {
            Debug.LogWarning("[CylinderTrackableEventHandler] No ObserverBehaviour found. Running in standalone/simulation mode.");
        }

        // Initially hide augmentations until marker is detected
        SetAugmentationsVisible(false);
        TrackingStatusMessage = "SEARCHING: Point camera at the Cylinder Target marker...";
    }

    private void OnDestroy()
    {
        if (mObserverBehaviour != null)
        {
            mObserverBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void Update()
    {
        // Editor simulation fallback: Press 'T' to simulate target detection
        if (Application.isEditor && Input.GetKeyDown(KeyCode.T))
        {
            if (IsCylinderTracked)
            {
                OnTrackingLost();
            }
            else
            {
                OnTrackingFound();
            }
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour observer, TargetStatus targetStatus)
    {
        Debug.Log($"[Vuforia] Cylinder Target '{observer.TargetName}' status: {targetStatus.Status} ({targetStatus.StatusInfo})");

        if (targetStatus.Status == Status.TRACKED || targetStatus.Status == Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }

    protected virtual void OnTrackingFound()
    {
        IsCylinderTracked = true;
        TrackingStatusMessage = "TARGET ACQUIRED: Cylinder Tracked (Augmented Video & Image Active)";
        Debug.Log("[CylinderTrackableEventHandler] Cylinder marker locked. Activating augmentations.");

        SetAugmentationsVisible(true);

        // Start augmented video playback
        if (videoPlayer != null)
        {
            if (!videoPlayer.isPlaying)
            {
                videoPlayer.Play();
            }
        }
    }

    protected virtual void OnTrackingLost()
    {
        IsCylinderTracked = false;
        TrackingStatusMessage = "SEARCHING: Point camera at the Cylinder Target marker...";
        Debug.Log("[CylinderTrackableEventHandler] Cylinder marker lost. Pausing video.");

        // Pause augmented video playback to save resources
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }

        SetAugmentationsVisible(false);
    }

    private void SetAugmentationsVisible(bool visible)
    {
        if (augmentedVideoObject != null)
        {
            augmentedVideoObject.SetActive(visible);
        }

        if (augmentedImageCardObject != null)
        {
            augmentedImageCardObject.SetActive(visible);
        }

        if (augmented3DElements != null)
        {
            augmented3DElements.SetActive(visible);
        }
    }

    /// <summary>
    /// Manually toggle tracking state (useful for UI testing button).
    /// </summary>
    public void SimulateToggleTracking()
    {
        if (IsCylinderTracked)
        {
            OnTrackingLost();
        }
        else
        {
            OnTrackingFound();
        }
    }
}
