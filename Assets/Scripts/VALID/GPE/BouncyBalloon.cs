using BlownAway.Character;
using UnityEngine;

namespace BlownAway.GPE
{
    public class BouncyBalloon : SphereTrigger
    {
        [Header("Force")]
        [SerializeField] private float _force;
        [SerializeField] private Vector3 _direction;
        [SerializeField][Range(0, 1)] private float _forceAccel;
        [SerializeField][Range(0, 1)] private float _forceDecel;
        [SerializeField] private Vector3 _vector3Up;

        [Header("Directions")]
        [SerializeField][Range(0, 1)] private float _upThreshold;
        [SerializeField][Range(-1, 0)] private float _downThreshold;


        [Header("Timer")]
        private bool _isPlayerIn;

        [Header("Visual")]
        [SerializeField] private float _scaleMultiplier = 1;
        [SerializeField] private float _scaleTime;

        [Header("Sounds")]
        [SerializeField] private AudioClip _collisionSound;


        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += MakePlayerBounce;
            OnExitTrigger += StopPlayerBounce;
        }

        private void MakePlayerBounce()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            Debug.Log("*BOING*");
            Vector3 direction = collider.transform.position - transform.position;
            Vector3 normalizedDirection = direction.normalized;

            if (normalizedDirection.y > _upThreshold) normalizedDirection = Vector3.up; // UP
            else if (normalizedDirection.y < _downThreshold) normalizedDirection = Vector3.down; // DOWN
            else if (Mathf.Abs(normalizedDirection.x) > Mathf.Abs(normalizedDirection.z)) normalizedDirection = new Vector3(Mathf.Round(normalizedDirection.x), 0, 0); // LEFT - RIGHT
            else normalizedDirection = new Vector3(0, 0, Mathf.Round(normalizedDirection.z)); // FORWARD - BACKWARD

            collider.Manager.MovementManager.AddExternalForce(gameObject, normalizedDirection * _force);

        }

        private void StopPlayerBounce()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            collider.Manager.MovementManager.AddExternalForce(gameObject, Vector3.zero);


        }
    }
}