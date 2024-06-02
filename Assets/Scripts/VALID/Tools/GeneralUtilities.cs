using System;
using UnityEngine;

public class GeneralUtilities : MonoBehaviour
{
    private Transform _copiedTransform;

    public void MoveObjectToCopiedPosition(Transform transform)
    {
        GeneralPasteBlueprint(transform, () =>
        {
            transform.position = _copiedTransform.position;
        }); 
    }

    public void ScaleObjectToCopiedScale(Transform transform)
    {
        GeneralPasteBlueprint(transform, () =>
        {
            transform.localScale = _copiedTransform.localScale;
        });
    }

    public void RotateObjectToCopiedRotation(Transform transform)
    {
        GeneralPasteBlueprint(transform, () =>
        {
            transform.rotation = _copiedTransform.rotation;
        });
    }

    public void PasteTransform(Transform transform)
    {
        if (_copiedTransform == null) return;

        MoveObjectToCopiedPosition(transform);
        RotateObjectToCopiedRotation(transform);
        ScaleObjectToCopiedScale(transform);
    }

    public void CopyTransform(Transform transform)
    {
        _copiedTransform = transform;
    }
    private void GeneralPasteBlueprint(Transform transform, Action action)
    {
        if (_copiedTransform == null) return;
        bool isActive = transform.gameObject.activeInHierarchy;
        if (transform.gameObject.TryGetComponent(out Rigidbody rb)) transform.gameObject.SetActive(false);

        action?.Invoke();


        if (rb != null) transform.gameObject.SetActive(isActive);
    }
}
