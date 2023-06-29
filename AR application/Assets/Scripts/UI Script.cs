using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Google.XR.ARCoreExtensions.Internal;
using Google.XR.ARCoreExtensions;
using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;

public class UIScript : MonoBehaviour
{
    public static UIScript instance;

    [SerializeField] TextMeshProUGUI textFPS, textCamPosInOrigin, textOriginPosInCam, textTrackingOrigMode, textNotTrackingReason, textLastPlaneClassification, textVPSAvailability, textDebugInfo;
    [SerializeField] ARSession arSession;
    [SerializeField] XROrigin xrOrigin;
    [SerializeField] ARPointCloudManager cloudManager;
    [SerializeField] ARPlaneManager planeManager;
    [SerializeField] ARCoreExtensions arCoreExtensions;
    [SerializeField] ARAnchorManager anchorManager;
    [SerializeField] AREarthManager arEarthManager;

    //ARCoreExtensionsConfig config;

    private void Start() {
        instance = this;
        //config = arCoreExtensions.GetComponent<ARCoreExtensionsConfig>();
        StartCoroutine(TestVPS());
        
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
        //textFPS.text = "FPS: " + arSession.frameRate;
        textCamPosInOrigin.text = "CamPosOrig: " + xrOrigin.CameraInOriginSpacePos;
        textOriginPosInCam.text = "OriginPosInCam: " + xrOrigin.OriginInCameraSpacePos;
        textTrackingOrigMode.text = "TrackingOrigMode: " + xrOrigin.RequestedTrackingOriginMode;
        textNotTrackingReason.text = "NotTrackingReason: " + ARSession.notTrackingReason;
        textDebugInfo.text = "Tracking state = " + arEarthManager.EarthTrackingState + "\nSupported? = " + arEarthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
        
    }

    IEnumerator TestVPS() {
        while (true) {
            yield return new WaitForSeconds(1);
            GeospatialPose geospatialPose = arEarthManager.CameraGeospatialPose;
            VpsAvailabilityPromise promise = AREarthManager.CheckVpsAvailabilityAsync(geospatialPose.Latitude, geospatialPose.Longitude);
            textVPSAvailability.text = "VPS Availability " + promise.Result;
            yield return null;
        }
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
    public void PlaceAnchor() {
        GeospatialPose geospatialPose = arEarthManager.CameraGeospatialPose;
        ARGeospatialAnchor anchor = ARAnchorManagerExtensions.AddAnchor(anchorManager, geospatialPose.Latitude, geospatialPose.Longitude, geospatialPose.Altitude, geospatialPose.EunRotation);
        textFPS.text = anchor.trackingState.ToString() + geospatialPose.ToString();
    }
}
