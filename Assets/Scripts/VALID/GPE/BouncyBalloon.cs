using AntoineFoucault.Utilities;
using UnityEngine;

namespace BlownAway.GPE
{
    public class BouncyBalloon : SphereTrigger
    {
        [Header("Force")]
        [SerializeField] private float _force;
        [SerializeField] [Range(0, 1)] private float _forceAccel;
        [SerializeField] [Range(0, 1)] private float _forceDecel;

        [Header("Directions")]
        [SerializeField] [Range(-1, 1)] private float _upThreshold;
        [Range(-1, 0)] private float _downThreshold;

        [SerializeField] private float _UpVectorMultiplier;
        [SerializeField] private bool _refreshPlayerAir;
        [SerializeField] private bool _needPlayerInput;




        //[Header("Timer")]
        private bool _isPlayerIn;

        //[Header("Visual")]
        //[SerializeField] private float _scaleMultiplier = 1;
        //[SerializeField] private float _scaleTime;

        //[Header("Sounds")]
        //[SerializeField] private AudioClip _collisionSound;


        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += StartPlayerBounce;
            OnExitTrigger += StopPlayerBounce;
        }

        private void Start()
        {
            _downThreshold = -1;
        }

        private void StartPlayerBounce()
        {
            _isPlayerIn = true;

            if (_needPlayerInput) return;

            MakePlayerBounce();
        }

        private void MakePlayerBounce()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;
            //if (collider.Manager.States.IsInState(collider.Manager.States.GroundPoundState)) return;
            //if (_isPlayerIn) return;


            Debug.Log("*BOING*");
            Vector3 direction = collider.transform.position - transform.position;
            Vector3 normalizedDirection = direction.normalized;

            Vector3 cameraForward = collider.Manager.CameraManager.Camera.transform.forward;
            cameraForward.y = 0;
            Vector3 upDirection = Vector3.up * _UpVectorMultiplier + cameraForward.normalized;
            Vector3 leftRightDirection = new Vector3(Mathf.Ceil(Mathf.Abs(normalizedDirection.x)) * Mathf.Sign(normalizedDirection.x), 0, 0); //isSideRebounceDirection ? new Vector3(Mathf.Round(normalizedDirection.x), 0, 0) : new Vector3(Mathf.Round(normalizedDirection.x), 0, 0) + collider.Manager.CameraManager.Camera.transform.forward;
            Vector3 forwardBackwardDirection = new Vector3(0, 0, Mathf.Ceil(Mathf.Abs(normalizedDirection.z)) * Mathf.Sign(normalizedDirection.z)); //isSideRebounceDirection ? new Vector3(0, 0, Mathf.Round(normalizedDirection.z)) : new Vector3(0, 0, Mathf.Round(normalizedDirection.z)) + collider.Manager.CameraManager.Camera.transform.forward;

            if (normalizedDirection.y > _upThreshold && collider.Manager.Data.PowerUpData.IsBalloonBounceAvailable)
            {
                normalizedDirection = upDirection; // UP
                if (_refreshPlayerAir)
                {
                    collider.Manager.AirManager.RefreshAir();
                    collider.Manager.MovementManager.StartDeriveTimer(collider.Manager);
                    collider.Manager.MovementManager.RefreshDashes(collider.Manager);
                }
            }
            else if (normalizedDirection.y < _downThreshold) normalizedDirection = Vector3.zero; // DOWN
            else if (Mathf.Abs(normalizedDirection.x) > Mathf.Abs(normalizedDirection.z)) normalizedDirection = leftRightDirection; // LEFT - RIGHT
            else normalizedDirection = forwardBackwardDirection; // FORWARD - BACKWARD


            if (collider.Manager.States.IsInState(collider.Manager.States.GroundPoundState)) // Ground Pound
            {
                collider.Manager.MovementManager.GroundPoundOnBalloon(collider.Manager);
            }
            else // Base Balloon Bounce
            {
                collider.Manager.MovementManager.AddExternalForce(gameObject, normalizedDirection * _force, _forceAccel);
            }


            //collider.Manager.Inputs.ResetLastPropulsionInputDirection();

            if (!collider.Manager.MovementManager.IsGrounded)
                collider.Manager.States.SwitchState(collider.Manager.States.FloatingState);
            else
                collider.Manager.States.SwitchState(collider.Manager.States.IdleState);



        }

        //private void CheckForPlayerInput()
        //{
        //    if (!_needPlayerInput) return;
        //    if (!_isPlayerIn) return;
        //    if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;
        //    if (!collider.Manager.MovementManager.CanBalloonBounce) return;

        //    MakePlayerBounce();

        //    _isPlayerIn = false;
        //}

        private void StopPlayerBounce()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            _isPlayerIn = false;

            collider.Manager.MovementManager.AddExternalForce(gameObject, Vector3.zero, _forceDecel);
        }

        //private void Update()
        //{
        //    CheckForPlayerInput();
        //}

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
            //GizmoExtensions.DrawCircle(position + downOffset, Vector3.up, _sphereCollider.radius, 0);
        }
    }
}