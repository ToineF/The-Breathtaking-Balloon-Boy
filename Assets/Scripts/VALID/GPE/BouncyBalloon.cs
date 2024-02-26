using AntoineFoucault.Utilities;
using UnityEngine;

namespace BlownAway.GPE
{
    public class BouncyBalloon : SphereTrigger
    {
        [Header("Force")]
        [SerializeField] private float _force;
        [SerializeField][Range(0, 1)] private float _forceAccel;
        [SerializeField][Range(0, 1)] private float _forceDecel;

        [Header("Directions")]
        [SerializeField][Range(0, 1)] private float _upThreshold;
        [SerializeField][Range(-1, 0)] private float _downThreshold;


        //[Header("Timer")]
        //private bool _isPlayerIn;

        //[Header("Visual")]
        //[SerializeField] private float _scaleMultiplier = 1;
        //[SerializeField] private float _scaleTime;

        //[Header("Sounds")]
        //[SerializeField] private AudioClip _collisionSound;


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

            // OLD VALUES
            //if (normalizedDirection.y > _upThreshold) normalizedDirection = Vector3.up; // UP
            //else if (normalizedDirection.y < _downThreshold) normalizedDirection = Vector3.down; // DOWN
            //else if (Mathf.Abs(normalizedDirection.x) > Mathf.Abs(normalizedDirection.z)) normalizedDirection = new Vector3(Mathf.Round(normalizedDirection.x), 0, 0); // LEFT - RIGHT
            //else normalizedDirection = new Vector3(0, 0, Mathf.Round(normalizedDirection.z)); // FORWARD - BACKWARD

            if (normalizedDirection.y > _upThreshold) normalizedDirection = Vector3.up + collider.Manager.CharacterVisual.forward; // UP
            else if (normalizedDirection.y < _downThreshold) normalizedDirection = Vector3.zero; // DOWN
            else if (Mathf.Abs(normalizedDirection.x) > Mathf.Abs(normalizedDirection.z)) normalizedDirection = new Vector3(Mathf.Round(normalizedDirection.x), 0, 0); // LEFT - RIGHT
            else normalizedDirection = new Vector3(0, 0, Mathf.Round(normalizedDirection.z)); // FORWARD - BACKWARD

            collider.Manager.MovementManager.AddExternalForce(gameObject, normalizedDirection * _force, _forceAccel);
        }

        private void StopPlayerBounce()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            collider.Manager.MovementManager.AddExternalForce(gameObject, Vector3.zero, _forceDecel);
        }

        private new void OnDrawGizmos()
        {
            if (!_displayGizmos) return;
            if (_showOnlyWhileSelected) return;

            base.OnDrawGizmos();

            DrawBalloonGizmos();
        }

        private new void OnDrawGizmosSelected()
        {
            if (!_displayGizmos) return;

            base.OnDrawGizmosSelected();

            DrawBalloonGizmos();
        }

        private void DrawBalloonGizmos()
        {
            _sphereCollider ??= GetComponent<SphereCollider>();

            Vector3 position = transform.position;

            Vector3 upOffset = Vector3.up * _sphereCollider.bounds.extents.y * _upThreshold;
            Vector3 downOffset = Vector3.up * _sphereCollider.bounds.extents.y * _downThreshold;

            GizmoExtensions.DrawCircle(position + upOffset, Vector3.up, _sphereCollider.radius, 0);
            GizmoExtensions.DrawCircle(position + downOffset, Vector3.up, _sphereCollider.radius, 0);
        }
    }
}