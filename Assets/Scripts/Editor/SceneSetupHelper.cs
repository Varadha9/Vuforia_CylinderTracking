using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Video;
using Vuforia;

/// <summary>
/// SceneSetupHelper
/// Automated Editor script to build and serialize the CylinderTrackingScene.unity scene.
/// Can be invoked directly from command-line batchmode via:
/// -executeMethod SceneSetupHelper.BuildScene
/// </summary>
public static class SceneSetupHelper
{
    [MenuItem("Vuforia/Build Cylinder Tracking Scene")]
    public static void BuildScene()
    {
        Debug.Log("[SceneSetupHelper] Creating new Cylinder Tracking Scene...");

        // Create new empty scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // 1. Directional Light
        var lightGo = new GameObject("Directional Light");
        var light = lightGo.AddComponent<Light>();
        light.type = LightType.Directional;
        light.color = new Color(1.0f, 0.98f, 0.95f);
        light.intensity = 1.2f;
        lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // 2. AR Camera
        var cameraGo = new GameObject("ARCamera");
        var camera = cameraGo.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.1f, 0.12f, 0.16f);
        camera.nearClipPlane = 0.05f;
        camera.farClipPlane = 2000f;
        cameraGo.tag = "MainCamera";
        cameraGo.AddComponent<AudioListener>();

        // Add VuforiaBehaviour to AR Camera
        var vuforiaBehaviour = cameraGo.AddComponent<VuforiaBehaviour>();
        cameraGo.transform.position = new Vector3(0, 0.05f, -0.35f);
        cameraGo.transform.rotation = Quaternion.Euler(10f, 0, 0);

        // 3. Cylinder Target GameObject
        var cylinderTargetGo = new GameObject("CylinderTarget");
        cylinderTargetGo.transform.position = Vector3.zero;
        cylinderTargetGo.transform.rotation = Quaternion.identity;

        // Try to add CylinderTargetBehaviour if available, else ObserverBehaviour
        var cylinderBehaviour = cylinderTargetGo.AddComponent<CylinderTargetBehaviour>();


        var trackingHandler = cylinderTargetGo.AddComponent<CylinderTrackableEventHandler>();
        var videoController = cylinderTargetGo.AddComponent<AugmentedVideoController>();

        // 4. Visual 3D Cylinder Mesh (Physical representation for reference in editor / scene)
        var cylinderRefMesh = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinderRefMesh.name = "PhysicalCylinderModel";
        cylinderRefMesh.transform.SetParent(cylinderTargetGo.transform, false);
        // Cylinder primitive default size is diameter=1, height=2.
        // Scale to 0.066 diameter, 0.122 height: scale = (0.066, 0.061, 0.066)
        cylinderRefMesh.transform.localScale = new Vector3(0.066f, 0.061f, 0.066f);
        cylinderRefMesh.transform.localPosition = Vector3.zero;

        // Apply label material to reference cylinder
        var labelTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/CylinderMarker_Label.png");
        if (labelTex != null)
        {
            var mat = new Material(Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Diffuse"));
            mat.mainTexture = labelTex;
            mat.name = "CylinderLabel_Mat";
            AssetDatabase.CreateAsset(mat, "Assets/Materials/CylinderLabel_Mat.mat");
            cylinderRefMesh.GetComponent<Renderer>().sharedMaterial = mat;
        }

        // 5. Augmented Video Display Quad (Oriented slightly offset from cylinder surface)
        var videoQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        videoQuad.name = "AugmentedVideoDisplay";
        videoQuad.transform.SetParent(cylinderTargetGo.transform, false);
        videoQuad.transform.localPosition = new Vector3(0, 0.01f, 0.038f); // On front face
        videoQuad.transform.localScale = new Vector3(0.06f, 0.09f, 1f);
        videoQuad.transform.localRotation = Quaternion.identity;

        var videoPlayer = videoQuad.AddComponent<VideoPlayer>();
        var videoClip = AssetDatabase.LoadAssetAtPath<VideoClip>("Assets/Videos/AugmentedDemoVideo.mp4");
        if (videoClip != null)
        {
            videoPlayer.clip = videoClip;
            videoController.videoClip = videoClip;
        }
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;
        videoPlayer.renderMode = VideoRenderMode.MaterialOverride;

        var videoMat = new Material(Shader.Find("Unlit/Texture") ?? Shader.Find("Standard"));
        videoMat.name = "AugmentedVideo_Mat";
        AssetDatabase.CreateAsset(videoMat, "Assets/Materials/AugmentedVideo_Mat.mat");
        videoQuad.GetComponent<Renderer>().sharedMaterial = videoMat;

        // 6. Augmented Image HUD Card (Floats tangential to the right of the cylinder)
        var imageCardQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        imageCardQuad.name = "AugmentedImageCard";
        imageCardQuad.transform.SetParent(cylinderTargetGo.transform, false);
        imageCardQuad.transform.localPosition = new Vector3(0.065f, 0.015f, 0.01f);
        imageCardQuad.transform.localScale = new Vector3(0.065f, 0.098f, 1f);
        imageCardQuad.transform.localRotation = Quaternion.Euler(0, -25f, 0);

        var cardTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/AugmentedImage_Card.png");
        if (cardTex != null)
        {
            var cardMat = new Material(Shader.Find("Unlit/Transparent") ?? Shader.Find("Standard"));
            cardMat.mainTexture = cardTex;
            cardMat.name = "AugmentedImage_Card_Mat";
            AssetDatabase.CreateAsset(cardMat, "Assets/Materials/AugmentedImage_Card_Mat.mat");
            imageCardQuad.GetComponent<Renderer>().sharedMaterial = cardMat;
        }

        // 7. Decorative Holographic Orbit Rings
        var holoRingsParent = new GameObject("HolographicRings");
        holoRingsParent.transform.SetParent(cylinderTargetGo.transform, false);

        var ring1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        ring1.name = "RingTop";
        ring1.transform.SetParent(holoRingsParent.transform, false);
        ring1.transform.localPosition = new Vector3(0, 0.062f, 0);
        ring1.transform.localScale = new Vector3(0.076f, 0.002f, 0.076f);

        var ring2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        ring2.name = "RingBottom";
        ring2.transform.SetParent(holoRingsParent.transform, false);
        ring2.transform.localPosition = new Vector3(0, -0.062f, 0);
        ring2.transform.localScale = new Vector3(0.076f, 0.002f, 0.076f);

        var holoMat = new Material(Shader.Find("Unlit/Color") ?? Shader.Find("Standard"));
        holoMat.color = new Color(0.0f, 0.9f, 1.0f, 0.8f);
        holoMat.name = "HolographicGlow_Mat";
        AssetDatabase.CreateAsset(holoMat, "Assets/Materials/HolographicGlow_Mat.mat");
        ring1.GetComponent<Renderer>().sharedMaterial = holoMat;
        ring2.GetComponent<Renderer>().sharedMaterial = holoMat;

        // Wire references in trackingHandler & videoController
        trackingHandler.mObserverBehaviour = cylinderBehaviour;
        trackingHandler.augmentedVideoObject = videoQuad;
        trackingHandler.augmentedImageCardObject = imageCardQuad;
        trackingHandler.augmented3DElements = holoRingsParent;
        trackingHandler.videoPlayer = videoPlayer;

        videoController.videoPlayer = videoPlayer;
        videoController.rotatingHoloRings = holoRingsParent.transform;

        // 8. AR HUD Manager
        var hudGo = new GameObject("AR_Status_HUD");
        var hudUI = hudGo.AddComponent<ARStatusUI>();
        hudUI.trackingHandler = trackingHandler;
        hudUI.videoController = videoController;

        // Save scene
        string scenePath = "Assets/Scenes/CylinderTrackingScene.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log($"[SceneSetupHelper] Successfully saved scene to {scenePath}");

        // Update EditorBuildSettings
        var scenes = new EditorBuildSettingsScene[]
        {
            new EditorBuildSettingsScene(scenePath, true)
        };
        EditorBuildSettings.scenes = scenes;
        Debug.Log("[SceneSetupHelper] Configured EditorBuildSettings with CylinderTrackingScene.");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
