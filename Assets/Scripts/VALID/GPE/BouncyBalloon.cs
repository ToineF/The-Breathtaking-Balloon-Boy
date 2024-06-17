using AntoineFoucault.Utilities;
using UnityEngine;
using DG.Tweening;
using BlownAway.Character;

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
        [SerializeField, Tooltip("The ejection force when the player doesn't have the balloon bounce")] private float _repelForce = 1;

        [Header("Visual")]
        [SerializeField] private GameObject _visual;
        [SerializeField] private float _scaleMultiplier = 1;
        [SerializeField] private float _scaleTime;



        //[Header("Timer")]
        private bool _isPlayerIn;


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
            CharacterManager manager = collider.Manager;


            Debug.Log("*BOING*");
            Vector3 direction = collider.transform.position - transform.position;
            Vector3 normalizedDirection = direction.normalized;

            Vector3 cameraForward = manager.CameraManager.Camera.transform.forward;
            cameraForward.y = 0;
            Vector3 upDirection = Vector3.up * _UpVectorMultiplier + cameraForward.normalized;
            Vector3 leftRightDirection = new Vector3(Mathf.Ceil(Mathf.Abs(normalizedDirection.x)) * Mathf.Sign(normalizedDirection.x), 0, 0); //isSideRebounceDirection ? new Vector3(Mathf.Round(normalizedDirection.x), 0, 0) : new Vector3(Mathf.Round(normalizedDirection.x), 0, 0) + collider.Manager.CameraManager.Camera.transform.forward;
            Vector3 forwardBackwardDirection = new Vector3(0, 0, Mathf.Ceil(Mathf.Abs(normalizedDirection.z)) * Mathf.Sign(normalizedDirection.z)); //isSideRebounceDirection ? new Vector3(0, 0, Mathf.Round(normalizedDirection.z)) : new Vector3(0, 0, Mathf.Round(normalizedDirection.z)) + collider.Manager.CameraManager.Camera.transform.forward;

            if (normalizedDirection.y > _upThreshold && collider.Manager.Data.PowerUpData.IsBalloonBounceAvailable)
            {
                normalizedDirection = upDirection; // UP
                if (_refreshPlayerAir)
                {
                    manager.AirManager.RefreshAir();
                    manager.MovementManager.StartDeriveTimer(manager);
                    manager.MovementManager.RefreshDashes(manager);
                }
            }
            else if (normalizedDirection.y < _downThreshold) normalizedDirection = Vector3.zero; // DOWN
            else if (Mathf.Abs(normalizedDirection.x) > Mathf.Abs(normalizedDirection.z)) normalizedDirection = leftRightDirection; // LEFT - RIGHT
            else normalizedDirection = forwardBackwardDirection; // FORWARD - BACKWARD

            float repelForce = collider.Manager.Data.PowerUpData.IsBalloonBounceAvailable ? 1 : _repelForce;


            if (manager.States.IsInState(manager.States.GroundPoundState)) // Ground Pound
            {
                manager.MovementManager.GroundPoundOnBalloon(manager);
            }
            else // Base Balloon Bounce
            {
                manager.MovementManager.AddExternalForce(gameObject, normalizedDirection * _force * repelForce, _forceAccel);
            }


            //manager.Inputs.ResetLastPropulsionInputDirection();

            if (!manager.MovementManager.IsMinGrounded)
                manager.States.SwitchState(manager.States.FloatingState);
            else
                manager.States.SwitchState(manager.States.IdleState);


            // Visual
            _visual.transform.DOComplete();
            _visual.transform.DOPunchScale(Vector3.up * _scaleMultiplier, _scaleTime);

            // Feedbacks
            manager.Feedbacks.PlayFeedback(manager.Data.FeedbacksData.BalloonBounceFeedback, _collider.bounds.top(), Quaternion.identity, null);

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