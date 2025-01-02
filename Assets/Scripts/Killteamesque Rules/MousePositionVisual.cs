using PolearmStudios.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionVisual : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask allowedLayers;

    private void Update()
    {
        if(mainCamera == null) return;
        if (Utilities.ScreenToWorldPoint(out Vector3 point, allowedLayers))
        {
            transform.position = point;
        }
    }
}
