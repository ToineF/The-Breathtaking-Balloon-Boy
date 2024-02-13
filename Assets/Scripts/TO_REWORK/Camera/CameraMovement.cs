using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntoineFoucault.Utilities;

public class CameraMovement : MonoBehaviour
{
    public GameObject Player;
    public GameObject CameraCenter;
    public float yOffset = 1f;
    public float sensitivity = 3f;
    public Camera Camera;
    public LayerMask PlayerLayer;

    public float scrollSensitivity = 5f;
    public float scrollDampening = 6f;

    public float zoomMin = 3.5f;
    public float zoomMax = 15f;
    public float zoomDefault = 10f;
    public float zoomDistance;

    public float collisionSensitivity = 4.5f;

    public float yUpLimit = 89.9f;
    public float yDownLimit = -89.9f;

    private RaycastHit _camHit;
    private Vector3 _camDist;

    private void Start()
    {
        _camDist = Camera.transform.localPosition;
        zoomDistance = zoomDefault;
        _camDist.z = -zoomDistance;

        //Cursor.visible = false;
    }

    private void LateUpdate()
    {
        CameraCenter.transform.position = new Vector3(Player.transform.position.x,
            Player.transform.position.y + yOffset, Player.transform.position.z);

        float xAngle = CameraCenter.transform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity / 2;
        float yAngle = CameraCenter.transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;

        var rotation = Quaternion.Euler(
            MathExtentions.ClampAngle(xAngle, yDownLimit, yUpLimit),
            yAngle,
            CameraCenter.transform.rotation.eulerAngles.z);

        CameraCenter.transform.rotation = rotation;

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            var scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
            scrollAmount *= zoomDistance * 0.3f;
            zoomDistance -= scrollAmount;
            zoomDistance = Mathf.Clamp(zoomDistance, zoomMin, zoomMax);
        }

        if (_camDist.z != -zoomDistance)
        {
            _camDist.z = Mathf.Lerp(_camDist.z, -zoomDistance, Time.deltaTime * scrollDampening);
        }

        Camera.transform.localPosition = _camDist;

        GameObject obj = new GameObject();
        obj.transform.SetParent(Camera.transform.parent);
        obj.transform.localPosition = new Vector3(Camera.transform.localPosition.x, Camera.transform.localPosition.y,
            Camera.transform.localPosition.z - collisionSensitivity);

        Vector3 direction = obj.transform.position - Camera.transform.position;
        direction.Normalize();
        Debug.DrawLine(Camera.transform.position, CameraCenter.transform.position + direction * collisionSensitivity, Color.red);

        if (Physics.Linecast(CameraCenter.transform.position + direction * collisionSensitivity, Camera.transform.position, out _camHit, ~PlayerLayer, QueryTriggerInteraction.Ignore))
        {
            Camera.transform.position = _camHit.point;

            var localPosition = new Vector3(Camera.transform.localPosition.x, Camera.transform.localPosition.y,
                Camera.transform.localPosition.z + collisionSensitivity);

            Camera.transform.localPosition = localPosition;

        }

        Destroy(obj);

        if (Camera.transform.localPosition.z > -1f)
        {
            Camera.transform.localPosition = new Vector3(Camera.transform.localPosition.x, Camera.transform.localPosition.y, -1f);
        }

    }
}
