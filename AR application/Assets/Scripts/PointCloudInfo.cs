using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PointCloudInfo : MonoBehaviour
{
    ARPointCloud pointCloud;
    GameObject canvasUI;
    TextMeshProUGUI textPoints;

    private void Start() {
        pointCloud = GetComponent<ARPointCloud>();
        pointCloud.updated += OnPointCloudChanged;
        canvasUI = UIScript.instance.gameObject;
        textPoints = canvasUI.GetNamedChild("Text Number of points").GetComponent<TextMeshProUGUI>();
    }


    void OnPointCloudChanged(ARPointCloudUpdatedEventArgs eventArgs) {
        if ((pointCloud.positions.HasValue) && ((pointCloud.identifiers.HasValue) && pointCloud.confidenceValues.HasValue)) {
            NativeSlice<Vector3> positions = pointCloud.positions.Value;
            NativeSlice<ulong> identifiers = pointCloud.identifiers.Value;
            NativeSlice<float> confidence = pointCloud.confidenceValues.Value;
            if (positions.Length > 0) {
                /*print("Number of points: " + positions.Length + "\nFirst Point: " + positions[0] + "\nID = " + identifiers[0] +
                    "confidence = " + confidence[0]);*/
                textPoints.text = "Number of points: " + positions.Length;
            }
        }
    }
}
