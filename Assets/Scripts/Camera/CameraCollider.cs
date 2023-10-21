using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private void OnTriggerEnter(Collider collision)
    {
        _virtualCamera.gameObject.SetActive(false);
        _virtualCamera.gameObject.SetActive(true);
    }
}
