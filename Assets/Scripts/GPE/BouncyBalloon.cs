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

        [Header("Directions")]
        [SerializeField] [Range(0, 1)] private float _upThreshold;
        [SerializeField] [Range(-1, 0)] private float _downThreshold;


        [Header("Timer")]
        private bool _isPlayerIn;

        [Header("Visual")]
        [SerializeField] private float _scaleMultiplier = 1;
        [SerializeField] private float _scaleTime;

        [Header("Sounds")]
        [SerializeField] private AudioClip _collisionSound;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out CharacterControllerTest characterController)) return;
            if (_isPlayerIn) return;
            //if (characterController.GetComponent<BalloonStateManager>().IsHammerFalling) return;
            _isPlayerIn = true;
            Vector3 direction = characterController.transform.position - transform.position;
            Vector3 normalizedDirection = direction.normalized;

            if (normalizedDirection.y > _upThreshold) normalizedDirection = Vector3.up; // UP
            else if (normalizedDirection.y < _downThreshold) normalizedDirection = Vector3.down; // DOWN
            else if (Mathf.Abs(normalizedDirection.x) > Mathf.Abs(normalizedDirection.z)) normalizedDirection = new Vector3(Mathf.Round(normalizedDirection.x),0,0); // LEFT - RIGHT
            else normalizedDirection = new Vector3(0, 0, Mathf.Round(normalizedDirection.z)); // FORWARD - BACKWARD

            characterController.AddAdditionalForce(gameObject, normalizedDirection * _force, _forceAccel);
            //characterController.SetForce(_force, _forceAccel);
            transform.DOComplete();
            transform.DOPunchScale(normalizedDirection * _scaleMultiplier, _scaleTime, 0, 0);

            // Sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayClip(_collisionSound);
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
