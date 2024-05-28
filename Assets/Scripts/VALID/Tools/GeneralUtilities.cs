using UnityEngine;

public class GeneralUtilities : MonoBehaviour
{
    private Transform _copiedTransform;

    public void MoveObjectToCopiedPosition(Transform transform)
    {
        if (_copiedTransform == null) return;
        transform.position = _copiedTransform.position;
    }

    public void ScaleObjectToCopiedScale(Transform transform)
    {
        if (_copiedTransform == null) return;
        transform.localScale = _copiedTransform.localScale;
    }

    public void RotateObjectToCopiedRotation(Transform transform)
    {
        if (_copiedTransform == null) return;
        transform.rotation = _copiedTransform.rotation;
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
}
