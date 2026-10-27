# 🥫 Ex:5 Marker-Based Cylinder Object Tracking with Vuforia

**Subject:** Immersive Application Development  
**Student Name:** Varad Mandhare  
**Enrollment No:** ADT23SOCB1574  
**Unity Version:** Unity 6 (`6000.5.9f1`)  
**Vuforia Engine Version:** `11.4.4`  

---

## 📌 Project Overview
This project implements **6-DOF Marker-Based Augmented Reality (AR) tracking of a 3D cylindrical object** (such as a beverage can or cylinder) using PTC Vuforia Engine in Unity 6.

Upon optical target detection, the system triggers:
1. **Augmented Video Playback**: An animated video player (`AugmentedDemoVideo.mp4`) mapped directly onto the front face of the cylinder.
2. **Holographic Augmented Image HUD Card**: A floating telemetry card showing target lock status, cylinder dimensions, and coordinate metrics.
3. **3D Holographic Orbit Rings**: Rotating cyan energy rings encircling the cylinder.
4. **On-Screen Academic Status HUD**: Displays live tracking status ("TRACKING ACTIVE" vs "SEARCHING"), FPS counter, and simulation controls.

---

## 🚀 Quick Start Guide

### 1. Open in Unity Hub
1. Launch **Unity Hub**.
2. Under the **Projects** tab, you will see **`Ex5_Vuforia_CylinderTracking`** (already registered with Editor version `6000.5.9f1`).
3. Click to open the project.

### 2. Run the Scene
1. In the Project window, navigate to `Assets/Scenes/`.
2. Double-click **`CylinderTrackingScene.unity`**.
3. Click the **Play (▶)** button at the top toolbar.

### 3. Testing Optical Tracking
- **Physical Camera Scan**:
  - Open `CYLINDER_MARKER_TO_SCAN.png` (also available on your Desktop at `CYLINDER_MARKER_EX5.png`) on your phone or wrap it around a real can/glass.
  - Point your laptop or USB webcam at the marker.
  - Vuforia will lock the 6-DOF pose and instantly trigger video and image augmentations!
- **Editor Keyboard Simulation (No Camera Required)**:
  - While in Play Mode, simply press **`T`** on your keyboard or click the on-screen **`Simulate [Tracked]`** button.
  - This simulates target detection, allowing you to test video playback and visual augmentations instantly!

---

## 📁 Project Directory Structure
```
Ex5_Vuforia_CylinderTracking/
├── Assets/
│   ├── Materials/               # Pre-compiled AR materials
│   │   ├── AugmentedImage_Card_Mat.mat
│   │   ├── AugmentedVideo_Mat.mat
│   │   ├── CylinderLabel_Mat.mat
│   │   └── HolographicGlow_Mat.mat
│   ├── Scenes/
│   │   └── CylinderTrackingScene.unity  # Primary AR scene
│   ├── Scripts/
│   │   ├── CylinderTrackableEventHandler.cs  # Vuforia observer events
│   │   ├── AugmentedVideoController.cs       # Video playback & loop
│   │   ├── ARStatusUI.cs                     # Academic on-screen HUD
│   │   └── Editor/SceneSetupHelper.cs        # Automated scene builder
│   ├── StreamingAssets/Vuforia/
│   │   ├── CylinderDB.xml                    # Vuforia cylinder dataset
│   │   └── CylinderDB.dat                    # Dataset binary descriptor
│   ├── Textures/
│   │   ├── CylinderMarker_Label.png          # Unrolled cylindrical label
│   │   ├── AugmentedImage_Card.png           # Floating HUD card
│   │   └── Cylinder_3D_Preview.png           # 3D wrap preview
│   └── Videos/
│       └── AugmentedDemoVideo.mp4            # Optimized course AR video
├── Packages/
│   ├── com.ptc.vuforia.engine/               # Vuforia SDK 11.4.4
│   └── manifest.json                         # Package dependencies
├── Screenshots/                              # 4 high-res figures
│   ├── vuforia_target_manager_cylinder.png
│   ├── cylinder_unrolling_geometry.png
│   ├── unity_scene_hierarchy_cylinder.png
│   └── ar_cylinder_tracking_result.png
├── CYLINDER_MARKER_TO_SCAN.png               # Printable/Scannable marker
└── Experiment5_Vuforia_Cylinder_Tracking.docx # Official Lab Report
```

---

## 📄 Lab Reports Generated
The official lab report matching your previous submissions (Experiments 1–4) has been generated and saved to:
1. `file:///home/varad/projects/Ex5_Vuforia_CylinderTracking/Experiment5_Vuforia_Cylinder_Tracking.docx`
2. `file:///home/varad/Downloads/iadl/Experiment5_Vuforia_Cylinder_Tracking.docx`
