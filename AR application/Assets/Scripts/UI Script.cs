using System;
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

    [SerializeField] TextMeshProUGUI textFPS, textCamPosInOrigin, textOriginPosInCam, textTrackingOrigMode, textNotTrackingReason, textLastPlaneClassification;
    [SerializeField] ARSession arSession;
    [SerializeField] XROrigin xrOrigin;
    [SerializeField] ARPointCloudManager cloudManager;
    [SerializeField] ARPlaneManager planeManager;

    private void Start() {
        instance = this;
    }

    private void OnEnable() {
        planeManager.planesChanged += OnPlanesChanged;
    }
    private void OnDisable() {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args) {
        print("count = " + planeManager.trackables.count);
        ARTrackable planeTrackable;
        foreach (var plane in planeManager.trackables) {
            planeTrackable = plane;
        }
        //textLastPlaneClassification.text = "LastPlaneClass: " + planeTrackable;
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
