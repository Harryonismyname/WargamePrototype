using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] LayerMask targetMask;
    public static Action<Vector3> OnClick;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && ScreenToWorldPoint(out Vector3 point, targetMask))
        {
            OnClick?.Invoke(point);
        }
    }
    bool ScreenToWorldPoint(out Vector3 point, LayerMask mask = default)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, mask))
        {
            point = hit.point;
            return true;
        }
        point = Vector3.zero;
        return false;
    }
}
