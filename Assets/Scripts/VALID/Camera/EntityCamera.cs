using UnityEngine;
using Cinemachine;

namespace BlownAway.Camera
{

    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class EntityCamera : MonoBehaviour
    {
        public CinemachineFramingTransposer FramingTransposer { get; private set; }

        [Header("Camera")]
        [Tooltip("The distance from the focus point")] public float CameraDistance;
        [Tooltip("The added height of the camera")] public float YOffset;
        [Tooltip("The offset of the camera's focus")] public Vector3 PositionOffset;

        [Header("General Inputs")]
        [Tooltip("The range of Y movements allowed with the mouse")] public float YDeadZone;

        [Header("Mouse")]
        [Tooltip("The influence of the mouse on the camera speed")] public float MouseSensitivity = 1;
        [Tooltip("Is the X-Axis Inverted")] public bool IsMouseXInverted;
        [Tooltip("Is the Y-Axis Inverted")] public bool IsMouseYInverted;

        [Header("Controller")]
        [Tooltip("The influence of the controller on the camera speed")] public float ControllerSensitivity = 1;
        [Tooltip("Is the X-Axis Inverted")] public bool IsControllerXInverted;
        [Tooltip("Is the Y-Axis Inverted")] public bool IsControllerYInverted;

        private void Awake()
        {
            FramingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }
}