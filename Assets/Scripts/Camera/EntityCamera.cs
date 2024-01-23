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

        [Header("Mouse")]
        [Tooltip("Is the X-Axis Inverted")] public bool IsXInverted;
        [Tooltip("Is the Y-Axis Inverted")] public bool IsYInverted;
        [Tooltip("The influence of the mouse on the camera speed")] public float MouseSensitivity = 1;
        [Tooltip("The influence of the controller on the camera speed")] public float ControllerSensitivity = 1;
        [Tooltip("The range of Y movements allowed with the mouse")] public float YDeadZone;

        private void Awake()
        {
            FramingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }
}