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

        [SerializeField] private float _sideAngleThresold;


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

            float angle = Vector3.Angle(collider.Manager.CharacterVisual.forward, _collider.bounds.center - collider.Manager.CharacterCollider.transform.position);
            bool isSideRebounceDirection = angle < _sideAngleThresold;

            Vector3 cameraForward = collider.Manager.CameraManager.Camera.transform.forward;
            cameraForward.y = 0;
            Vector3 upDirection = Vector3.up + cameraForward.normalized;
            Vector3 leftRightDirection = new Vector3(Mathf.Round(normalizedDirection.x), 0, 0); //isSideRebounceDirection ? new Vector3(Mathf.Round(normalizedDirection.x), 0, 0) : new Vector3(Mathf.Round(normalizedDirection.x), 0, 0) + collider.Manager.CameraManager.Camera.transform.forward;
            Vector3 forwardBackwardDirection = new Vector3(0, 0, Mathf.Round(normalizedDirection.z)); //isSideRebounceDirection ? new Vector3(0, 0, Mathf.Round(normalizedDirection.z)) : new Vector3(0, 0, Mathf.Round(normalizedDirection.z)) + collider.Manager.CameraManager.Camera.transform.forward;

            if (normalizedDirection.y > _upThreshold) normalizedDirection = upDirection; // UP
            else if (normalizedDirection.y < _downThreshold) normalizedDirection = Vector3.zero; // DOWN
            else if (Mathf.Abs(normalizedDirection.x) > Mathf.Abs(normalizedDirection.z)) normalizedDirection = leftRightDirection; // LEFT - RIGHT
            else normalizedDirection = forwardBackwardDirection; // FORWARD - BACKWARD

            collider.Manager.MovementManager.AddExternalForce(gameObject, normalizedDirection * _force, _forceAccel);

            //collider.Manager.Inputs.ResetLastPropulsionInputDirection();
            if (!collider.Manager.MovementManager.IsGrounded)
                collider.Manager.States.SwitchState(collider.Manager.States.PropulsionState);
            collider.Manager.AirManager.RefreshAir();
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