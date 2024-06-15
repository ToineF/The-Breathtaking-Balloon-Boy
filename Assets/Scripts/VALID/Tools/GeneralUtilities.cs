using AntoineFoucault.Utilities;
using BlownAway.Character;
using System;
using UnityEngine;

public class GeneralUtilities : MonoBehaviour
{
    private Transform _copiedTransform;

    public void PastePosition(Transform transform)
    {
        GeneralPasteBlueprint(transform, () =>
        {
            transform.position = _copiedTransform.position;
        }); 
    }

    public void PasteScale(Transform transform)
    {
        GeneralPasteBlueprint(transform, () =>
        {
            transform.localScale = _copiedTransform.localScale;
        });
    }

    public void PasteRotation(Transform transform)
    {
        GeneralPasteBlueprint(transform, () =>
        {
            transform.rotation = _copiedTransform.rotation;
        });
    }

    public void PasteRotationPlayer(CharacterManager manager)
    {
        manager.MovementManager.SetCurrentDeplacementDirection(_copiedTransform);
    }


    //public void PasteRigidbodyTorque(Rigidbody rigidbody)
    //{
    //rigidbody.TORQ
    //rigidbody.AddTorque(Vector3.up * Vector3.Dot(Vector3.forward, _copiedTransform.transform.forward));
    //}

    public void PasteTransform(Transform transform)
    {
        if (_copiedTransform == null) return;

        PastePosition(transform);
        PasteRotation(transform);
        PasteScale(transform);
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
    public void ResetTransform(Transform transform)
    {
        transform.ResetTransform();
    }

    public void ResetPosition(Transform transform)
    {
        transform.ResetPosition();
    }

    public void ResetRotation(Transform transform)
    {
         transform.ResetRotation();
    }

    public void ResetEulerAngles(Transform transform)
    {
        transform.localEulerAngles = Vector3.zero;
    }

    public void ResetScale(Transform transform)
    {
        transform.ResetScale();
    }
}
