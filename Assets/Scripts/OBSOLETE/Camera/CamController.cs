using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CamController : MonoBehaviour
{
    [Header("Camera values")]
    [SerializeField] private float mouseSensitivity = 6.0f;
    [SerializeField] private float controllerSensitivity = 6.0f;
    [SerializeField] private float verticalAngleMin = -89.9f;
    [SerializeField] private float verticalAngleMax = 89.9f;
    [Header("Spring arm values")]
    [SerializeField] private Vector3 springArmEnd;
    [SerializeField] private float hitOffset;
    [Header("References")]
    [SerializeField] private LayerMask solidsMask;
    [SerializeField] private Camera cam;

    private PlayerInputs inputs;
    private Vector2 moveVector;
    private Vector2 camAngle;
    private bool _isMouse;

    private void OnEnable() {
        inputs = new PlayerInputs();
        inputs.Enable();
        inputs.Player.CameraMoveMouse.performed += SetMouse;
        inputs.Player.CameraMoveMouse.canceled += SetMouse;
        inputs.Player.CameraMoveController.performed += SetController;
        inputs.Player.CameraMoveController.canceled += SetController;
    }

    private void Start() {
        camAngle = Vector2.zero;
        //Cursor lock
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate() {
        UpdateAngle();

        //Rotation
        transform.rotation = Quaternion.Euler(camAngle.y, camAngle.x, 0.0f);
        //Spring arm
        Vector3 absoluteSpringArmEnd = transform.position + (transform.rotation * springArmEnd);
        if (Physics.Linecast(transform.position, absoluteSpringArmEnd, out RaycastHit hit, solidsMask, QueryTriggerInteraction.Ignore))
            cam.transform.position = hit.point + (transform.position - absoluteSpringArmEnd).normalized * hitOffset;
        else 
            cam.transform.position = absoluteSpringArmEnd;
    }

    void UpdateAngle() {
        float sensitivity = _isMouse ? mouseSensitivity : controllerSensitivity;
        camAngle += Time.deltaTime * sensitivity * moveVector;

        camAngle.y = Mathf.Clamp(camAngle.y, verticalAngleMin, verticalAngleMax);

        if (camAngle.x < 0.0f)
            camAngle.x += 360.0f;
        else if (camAngle.x >= 360.0f)
            camAngle.x -= 360.0f;
    }

    private void SetMouse(InputAction.CallbackContext ctx)
    {
        _isMouse = true;
        moveVector = ctx.ReadValue<Vector2>();
    }

    private void SetController(InputAction.CallbackContext ctx)
    {
        _isMouse = false;
        moveVector = ctx.ReadValue<Vector2>();
    }

#if UNITY_EDITOR
    private void OnValidate() {
        if (cam == null) return;

        cam.transform.position = transform.position + (transform.rotation * springArmEnd);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.rotation * springArmEnd);
    }
#endif
}
