using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolearmStudios.Utils;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] LayerMask targetMask;
    public static Action<Vector3> OnClick;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Utilities.ScreenToWorldPoint(out Vector3 point, targetMask))
        {
            OnClick?.Invoke(point);
        }
    }
}
