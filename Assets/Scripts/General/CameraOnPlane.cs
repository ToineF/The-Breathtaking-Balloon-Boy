using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntoineFoucault.Utilities;
using System.Runtime.CompilerServices;
using DG.Tweening;
using System;

[RequireComponent(typeof(Camera))]
public class CameraOnPlane : MonoBehaviour
{
    [SerializeField] private GameObject _image;
    [SerializeField] private bool _onUpdate;
    [SerializeField] private bool _takeScreenshot;

    private void Start()
    {
        SetTarget();

        if (!_onUpdate) Destroy(this);


        StartCoroutine(FreezeCam());
        _image.transform.DOScale(new Vector3(0,0,0),2).OnComplete(DestroySelf);
    }

    private void Update()
    {
        SetTarget();

    }

    private void SetTarget() => transform.SetTransform(Camera.main.transform);

    private IEnumerator FreezeCam()
    {
        GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
        yield return null;
        GetComponent<Camera>().cullingMask = 0;
    }

    private void DestroySelf()
    {
        TakeScreenshot();
        Destroy(gameObject);
    }

    private void TakeScreenshot()
    {
        if (!_takeScreenshot) return;
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screenshots/" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".png");
        //UnityEditor.AssetDatabase.Refresh();
    }
}
