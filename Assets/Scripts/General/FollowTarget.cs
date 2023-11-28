using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntoineFoucault.Utilities;

public class FollowTarget : MonoBehaviour
{
    private void Update()
    {
        transform.SetTransform(Camera.main.transform);
    }
}
