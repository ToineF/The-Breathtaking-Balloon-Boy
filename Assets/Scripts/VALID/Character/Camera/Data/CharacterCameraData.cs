using UnityEngine;

namespace BlownAway.Camera.Data
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "CharacterData/Camera")]
    public class CharacterCameraData : ScriptableObject
    {
        [field:Header("Mouse")]
        [field:SerializeField, Tooltip("Is the mouse cursor visible when the game plays?")] public bool SetCursorVisible { get; private set; }
        [field:SerializeField, Tooltip("The influence of the mouse on the camera speed")] public float MouseSensitivity { get; private set; } = 1;
        [field:SerializeField, Tooltip("Is the X-Axis Inverted")] public bool IsMouseXInverted { get; private set; }
        [field:SerializeField, Tooltip("Is the Y-Axis Inverted")] public bool IsMouseYInverted { get; private set; }

        [field:Header("Controller")]
        [field:SerializeField, Tooltip("The influence of the controller on the camera speed")] public float ControllerSensitivity { get; private set; } = 1;
        [field:SerializeField, Tooltip("Is the X-Axis Inverted")] public bool IsControllerXInverted { get; private set; }
        [field:SerializeField, Tooltip("Is the Y-Axis Inverted")] public bool IsControllerYInverted { get; private set; }

        [field:Header("Positions")]
        [field:SerializeField, Tooltip("The default zoom position at the start of the scene")] public float ZoomDefault { get; private set; } = 10f;
        [field:SerializeField, Tooltip("The offset in the Y direction from the focus point")] public float YOffset { get; private set; } = 1f;
        [field:SerializeField, Tooltip("The maximum angle the camera at which can turn")] public float YUpLimit { get; private set; } = 89.9f;
        [field:SerializeField, Tooltip("The minimum angle the camera at which can turn")] public float YDownLimit { get; private set; } = -89.9f;
        [field:SerializeField, Tooltip("The time the camera needs to follow the target"), Range(0,1)] public float CameraFollowLerpTime { get; private set; }
        [field:SerializeField, Tooltip("The time the camera needs to center itself")] public float CameraCenterLerpTime { get; private set; }
        [field:SerializeField, Tooltip("The time the camera needs to center itself in top down")] public float CameraTopDownStartLerpTime { get; private set; }
        [field:SerializeField, Tooltip("The time the camera needs to center itself")] public float CameraTopDownEndLerpTime { get; private set; }
        
        [field:Header("Scroll")]
        [field:SerializeField, Tooltip("Is the player able to scroll (useful for debug)")] public bool CanScroll { get; private set; } = false;
        [field:SerializeField, Tooltip("The influence of the scroll on the zoom speed")] public float ScrollSensitivity { get; private set; } = 5f;
        [field:SerializeField, Tooltip("The dampening of the zoom speed of the scroll")] public float ScrollDampening { get; private set; } = 6f;
        [field:SerializeField, Tooltip("The minimum scroll zoom distance from the focus point")] public float ZoomMin { get; private set; } = 3.5f;
        [field:SerializeField, Tooltip("The maximum scroll zoom distance from the focus point")] public float ZoomMax { get; private set; } = 15f;
        
        [field:Header("Collisions")]
        [field:SerializeField, Tooltip("The layer of the main character")] public LayerMask PlayerLayer;
        [field:SerializeField, Tooltip("The interpolation speed of the camera when collisioning")] public float CollisionSensitivity { get; private set; } = 0.1f;
        [field:SerializeField, Tooltip("The minimum thickness of a collider between the camera and the focus point to be taken into account")] public float MinCollisionThickness { get; private set; } = 1.5f;

    }
}