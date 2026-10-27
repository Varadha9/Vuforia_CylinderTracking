using UnityEngine;

/// <summary>
/// ARStatusUI
/// Renders an on-screen academic HUD with real-time tracking status,
/// target specifications, FPS, and interactive simulation controls.
/// Student: Varad Mandhare (ADT23SOCB1574)
/// Subject: Immersive Application Development - Ex:5
/// </summary>
public class ARStatusUI : MonoBehaviour
{
    [Header("Controller Reference")]
    public CylinderTrackableEventHandler trackingHandler;
    public AugmentedVideoController videoController;

    private float deltaTime = 0.0f;
    private GUIStyle headerStyle;
    private GUIStyle statusStyle;
    private GUIStyle buttonStyle;
    private GUIStyle infoStyle;
    private bool stylesInitialized = false;

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void InitStyles()
    {
        if (stylesInitialized) return;

        headerStyle = new GUIStyle(GUI.skin.box)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 15,
            fontStyle = FontStyle.Bold
        };
        headerStyle.normal.textColor = Color.white;

        statusStyle = new GUIStyle(GUI.skin.box)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 14,
            fontStyle = FontStyle.Bold
        };

        buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 13,
            fontStyle = FontStyle.Bold
        };

        infoStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 12
        };
        infoStyle.normal.textColor = new Color(0.85f, 0.9f, 1f);

        stylesInitialized = true;
    }

    private void OnGUI()
    {
        InitStyles();

        float screenW = Screen.width;
        float screenH = Screen.height;

        // 1. Top Header Banner
        GUI.backgroundColor = new Color(0.05f, 0.1f, 0.2f, 0.9f);
        GUI.Box(new Rect(10, 10, screenW - 20, 65), GUIContent.none);

        GUI.Label(new Rect(20, 12, screenW - 40, 25),
            "Ex:5 - Marker-Based Cylinder Object Tracking with Vuforia", headerStyle);

        GUI.Label(new Rect(20, 40, screenW - 40, 25),
            "Student: Varad Mandhare | Enrollment No: ADT23SOCB1574 | Immersive Application Development", headerStyle);

        // 2. Tracking Status Badge
        bool isTracked = CylinderTrackableEventHandler.IsCylinderTracked;
        GUI.backgroundColor = isTracked ? new Color(0.1f, 0.7f, 0.3f, 0.95f) : new Color(0.9f, 0.6f, 0.1f, 0.95f);
        statusStyle.normal.textColor = Color.white;

        string statusText = isTracked
            ? "● TRACKING ACTIVE: Cylinder Target Detected (Augmented Image & Video Playing)"
            : "○ SEARCHING: Align physical cylinder / marker with camera view";

        GUI.Box(new Rect(10, 85, screenW - 20, 38), statusText, statusStyle);

        // 3. Telemetry & Instructions Panel (Bottom Left)
        float fps = 1.0f / Mathf.Max(0.0001f, deltaTime);
        GUI.backgroundColor = new Color(0.08f, 0.12f, 0.22f, 0.85f);
        GUI.Box(new Rect(10, screenH - 140, 360, 130), GUIContent.none);

        GUI.Label(new Rect(20, screenH - 135, 340, 20), $"Target Geometry: Cylinder (D: 6.6cm, H: 12.2cm)", infoStyle);
        GUI.Label(new Rect(20, screenH - 115, 340, 20), $"Target Database: CylinderDB (Vuforia Target Manager)", infoStyle);
        GUI.Label(new Rect(20, screenH - 95, 340, 20), $"Augmentation: Augmented Image HUD + Video Loop", infoStyle);
        GUI.Label(new Rect(20, screenH - 75, 340, 20), $"System Performance: {fps:0.} FPS | Zero-Drift PnP", infoStyle);
        GUI.Label(new Rect(20, screenH - 55, 340, 20), $"Editor Shortcut: Press [T] to Toggle Tracking Simulation", infoStyle);

        // 4. Interactive Control Buttons (Bottom Right)
        float btnW = 180;
        float btnH = 36;
        float startX = screenW - btnW - 15;
        float startY = screenH - 140;

        GUI.backgroundColor = new Color(0.2f, 0.4f, 0.8f, 0.9f);
        if (GUI.Button(new Rect(startX, startY, btnW, btnH), isTracked ? "Simulate [Lost]" : "Simulate [Tracked]", buttonStyle))
        {
            if (trackingHandler != null)
            {
                trackingHandler.SimulateToggleTracking();
            }
        }

        GUI.backgroundColor = new Color(0.2f, 0.6f, 0.5f, 0.9f);
        if (GUI.Button(new Rect(startX, startY + 42, btnW, btnH), "Play / Pause Video", buttonStyle))
        {
            if (videoController != null && videoController.videoPlayer != null)
            {
                if (videoController.videoPlayer.isPlaying)
                    videoController.PauseVideo();
                else
                    videoController.PlayVideo();
            }
        }

        GUI.backgroundColor = new Color(0.5f, 0.2f, 0.7f, 0.9f);
        if (GUI.Button(new Rect(startX, startY + 84, btnW, btnH), "Restart Video", buttonStyle))
        {
            if (videoController != null)
            {
                videoController.RestartVideo();
            }
        }
    }
}
