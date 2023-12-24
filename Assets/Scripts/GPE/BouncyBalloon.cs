using UnityEngine;
using DG.Tweening;
using BlownAway.Player;

namespace BlownAway.GPE
{
    public class BouncyBalloon : MonoBehaviour
    {
        [Header("Force")]
        [SerializeField] private float _force;
        [SerializeField] private Vector3 _direction;
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
            //if (characterController.GetComponent<BalloonStateManager>().IsHammerFalling) return;
            _isPlayerIn = true;
            Vector3 direction = characterController.transform.position - transform.position;
            Vector3 normalizedDirection = direction.normalized;
            normalizedDirection = new Vector3(Mathf.Round(normalizedDirection.x), Mathf.Round(normalizedDirection.y), Mathf.Round(normalizedDirection.z));
            characterController.AddAdditionalForce(gameObject, normalizedDirection * _force, _forceAccel);
            //characterController.SetForce(_force, _forceAccel);
            transform.DOComplete();
            transform.DOPunchScale(_vector3Up * _scaleMultiplier, _scaleTime, 0, 0);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out CharacterControllerTest characterController)) return;
            if (!_isPlayerIn) return;
            //if (characterController.GetComponent<BalloonStateManager>().IsHammerFalling) return;


            _isPlayerIn = false;
            characterController.AddAdditionalForce(gameObject, Vector3.zero, _forceDecel);
            //characterController.SetForce(Vector3.zero, _forceDecel);

        }
    }
}
