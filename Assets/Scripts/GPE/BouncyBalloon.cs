using UnityEngine;
using DG.Tweening;

public class BouncyBalloon : MonoBehaviour
{
    [Header("Force")]
    [SerializeField] private Vector3 _force;
    [SerializeField] [Range(0, 1)] private float _forceAccel;
    [SerializeField] [Range(0, 1)] private float _forceDecel;
    [SerializeField] private Vector3 _vector3Up;

    [Header("Timer")]
    private bool _isPlayerIn;

    [Header("Visual")]
    [SerializeField] private float _scaleMultiplier = 1;
    [SerializeField] private float _scaleTime;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out CharacterControllerTest characterController)) return;
        if (_isPlayerIn) return;
        if (characterController.GetComponent<BalloonStateManager>().IsHammerFalling) return;
        Debug.Log("C");
        _isPlayerIn = true;
        characterController.SetForce(_force, _forceAccel);
        transform.DOComplete();
        transform.DOPunchScale(_vector3Up * _scaleMultiplier, _scaleTime, 0, 0);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out CharacterControllerTest characterController)) return;
        if (!_isPlayerIn) return;
        if (characterController.GetComponent<BalloonStateManager>().IsHammerFalling) return;

        Debug.Log("D");

        _isPlayerIn = false;
        characterController.SetForce(Vector3.zero, _forceDecel);

    }
}
