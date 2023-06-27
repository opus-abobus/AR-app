using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class UIScript : MonoBehaviour
{
    public static UIScript instance;

    [SerializeField] TextMeshProUGUI textFPS, textCamPosInOrigin, textOriginPosInCam, textTrackingOrigMode, textNotTrackingReason;
    [SerializeField] ARSession arSession;
    [SerializeField] XROrigin xrOrigin;
    [SerializeField] ARPointCloudManager cloudManager;

    private void Start() {
        instance = this;
    }

    private void Update() {
        textFPS.text = "FPS: " + arSession.frameRate;
        textCamPosInOrigin.text = "CamPosOrig: " + xrOrigin.CameraInOriginSpacePos;
        textOriginPosInCam.text = "OriginPosInCam: " + xrOrigin.OriginInCameraSpacePos;
        textTrackingOrigMode.text = "TrackingOrigMode: " + xrOrigin.RequestedTrackingOriginMode;
        textNotTrackingReason.text = "NotTrackingReason: " + ARSession.notTrackingReason;
    }

    public void ToggleTrackingOriginMode() {
        if (xrOrigin.RequestedTrackingOriginMode == XROrigin.TrackingOriginMode.NotSpecified) {
            xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        }
        else if (xrOrigin.RequestedTrackingOriginMode == XROrigin.TrackingOriginMode.Floor) {
            xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        }
        else {
            xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.NotSpecified;
        }
    }
}
