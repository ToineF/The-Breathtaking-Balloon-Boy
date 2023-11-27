using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class EntityCamera : MonoBehaviour
{
    [Header("Camera")]
    [Tooltip("The distance from the focus point")] public float CameraDistance;
    [Tooltip("The added height of the camera")] public float YOffset;
    [Tooltip("The offset of the camera's focus")] public Vector3 PositionOffset;

    [Header("Mouse")]
    [Tooltip("Is the X-Axis Inverted")] public bool IsXInverted;
    [Tooltip("Is the Y-Axis Inverted")] public bool IsYInverted;
    [Tooltip("The influence of the mouse on the camera speed")] public float MouseSpeed = 1;
    [Tooltip("The range of Y movements allowed with the mouse")] public float YDeadZone;
}
