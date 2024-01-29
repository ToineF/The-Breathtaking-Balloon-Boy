using BlownAway.Character.Inputs;
using System;
using UnityEngine;
using DG.Tweening;
using AntoineFoucault.Utilities;

namespace BlownAway.Character.Movements
{

    public class CharacterMovementManager : MonoBehaviour
    {
        public Action OnGroundEnter;
        public Action OnGroundExit;

        // Idle Data
        /// /////////////////////////////////////////////////////////// PUT IN A SCRIPTABLE OBJECT
        [field: SerializeField, Tooltip("The walking speed the character starts moving at")] public float BaseWalkSpeed { get; set; }
        [field: SerializeField] public float BaseWalkTime { get; set; }
        [field: SerializeField] public Ease BaseWalkEase { get; set; }
        [field: SerializeField, Tooltip("The lateral speed the character moves at while falling")] public float FallDeplacementSpeed { get; set; }
        [field: SerializeField, Tooltip("The lateral speed the character moves at while floating")] public float FloatDeplacementSpeed { get; set; }
        [Tooltip("The current global velocity of the character (movements, gravity, forces...)")] public Vector3 CurrentVelocity { get; set; }

        [Header("Gravity")] // ADD TOOLTIPS

        [ReadOnly, Tooltip("The current gravity the character falls at")] public float CurrentGravity;
        [ReadOnly, Tooltip("The minimum gravity the character can fall at")] public float MinGravity;
        [ReadOnly, Tooltip("The maximum gravity the character can fall at")] public float MaxGravity;
        [Tooltip("The gravity the character falls at while not floating")] public float BaseGravity;
        [Tooltip("The maximum gravity the character can fall at while not floating")] public float BaseMaxGravity;
        [Tooltip("The increase of gravity added at each frame")] public float GravityIncreaseByFrame;

        [Tooltip("The gravity the character falls at while floating")] public float FloatingGravity;
        [Tooltip("The maximum gravity the character can fall at while floating")] public float FloatingMaxGravity;

        [Tooltip("The gravity the character falls at while propulsing")] public float PropulsionGravity;
        [Tooltip("The maximum gravity the character can fall at while propulsing")] public float PropulsionMaxGravity;

        [Header("Ground Check")]
        [Tooltip("The distance offset of the ground detection check from the character")] public float GroundCheckDistance;
        [Tooltip("The radius of the ground detection sphere collider")] public float GroundDetectionSphereRadius;
        [Tooltip("The layer of the ground the character can walk on")] public LayerMask GroundLayer;
        [Tooltip("The raycast hits stocked while looking for ground")] public RaycastHit[] GroundHitResults { get; set; }
        [ReadOnly] public bool IsGrounded;
        [HideInInspector] public RaycastHit LastGround;

        [Header("Propulsion")]
        [Tooltip("The base speed the character moves at while propulsing")] public float BasePropulsionSpeed;

        public float CurrentDeplacementSpeed;

        /*
        [Header("Forces")]
        [ReadOnly] public Vector3 Force;
        [ReadOnly] public Vector3 CurrentForce;*/


        private void Start()
        {
            GroundHitResults = new RaycastHit[2];
            SetGravityTo(GameManager.Instance.CharacterManager, BaseGravity, BaseMaxGravity);
        }

        
        public void MoveAtSpeed(CharacterManager manager, float moveSpeed)
        {
            Vector3 moveDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)) * manager.Inputs.MoveInputDirection.z + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * manager.Inputs.MoveInputDirection.x).normalized;
            moveDirection = Vector3.Scale(moveDirection, new Vector3(1, 0, 1));
            //SetAnimation(moveDirection);
            CurrentVelocity += moveDirection * moveSpeed * Time.deltaTime;
        }

        public void MoveAtLerpedSpeed(CharacterManager manager, float moveSpeed, float transitionTime, Ease ease)
        {
            // HERE TWEEN THE SPEED (ONLY ONCE)
            CurrentDeplacementSpeed.DOFloat(moveSpeed, transitionTime, ease);
            //Debug.Log(CurrentDeplacementSpeed);
            //MoveAtSpeed(manager, CurrentDeplacementSpeed);
        }

        public void ApplyVelocity(CharacterManager manager)
        {
            manager.CharacterRigidbody.velocity = CurrentVelocity;
        }

        public void ResetVelocity()
        {
            CurrentVelocity = Vector3.zero;
        }

        public void CheckIfGrounded(CharacterManager manager)
        {
            var lastGrounded = IsGrounded;
            IsGrounded = Physics.SphereCastNonAlloc(manager.CharacterTransform.position, GroundDetectionSphereRadius, Vector3.down, GroundHitResults, GroundCheckDistance, GroundLayer) > 0;
            if (lastGrounded != IsGrounded)
            {
                if (IsGrounded) // On Ground Enter
                {
                    LastGround = GroundHitResults[0];
                    OnGroundEnter?.Invoke();
                    CurrentGravity = BaseGravity;
                    manager.States.SwitchState(manager.States.IdleState);
                }
                else // On Ground Leave
                {
                    OnGroundExit?.Invoke();
                    manager.States.SwitchState(manager.States.FallingState); // HERE DISSOCIATE FALLING FROM PROPULSION
                }
            }
        }

        public void UpdateGravity(CharacterManager manager)
        {
            if (!manager.MovementManager.IsGrounded)
            {
                manager.MovementManager.CurrentGravity = Mathf.Clamp(manager.MovementManager.CurrentGravity + manager.MovementManager.GravityIncreaseByFrame, manager.MovementManager.MinGravity, manager.MovementManager.MaxGravity);
            }

            /*Vector3 additionalForces = Vector3.zero;
            foreach (var force in _additionnalForces)
            {
                additionalForces += force.Value.CurrentForce;
                force.Value.CurrentForce = Vector3.Lerp(force.Value.CurrentForce, force.Value.TargetForce, force.Value.ForceLerp);
            }*/
            Vector3 gravity = -manager.MovementManager.CurrentGravity * Vector3.up;
            //Vector3 allForces = CharacterManager.Instance + additionalForces + gravity;

            //_characterController.Move(allForces * Time.deltaTime);

            manager.MovementManager.CurrentVelocity += gravity * Time.deltaTime;
            //CharacterManager.Instance.Force = Vector3.Lerp(CharacterManager.Instance.Force, CharacterManager.Instance.CurrentGravity, _lerpValue);
        }

        public void SetGravityTo(CharacterManager manager, float targetGravity, float maxGravity)
        {
            manager.MovementManager.CurrentGravity = targetGravity;
            manager.MovementManager.MinGravity = targetGravity;
            manager.MovementManager.MaxGravity = maxGravity;
        }

        public void CheckForFloatCancel(CharacterManager manager)
        {
            if (!manager.Inputs.StartedFalling) return;

            manager.States.SwitchState(manager.States.FallingState);
        }


        // Float & Propulsion
        public void CheckForPropulsionStartOnGround(CharacterManager manager)
        {
            if (manager.Inputs.PropulsionType.HasFlag(PropulsionDirection.Up) || manager.Inputs.PropulsionType.HasFlag(PropulsionDirection.Lateral))
            {
                manager.States.SwitchState(manager.States.PropulsionState);
            }
        }

        public void CheckForPropulsionStartOnAir(CharacterManager manager)
        {
            if (manager.AirManager.AirIsEmpty) return;

            if (manager.Inputs.PropulsionType != 0)
            {
                manager.States.SwitchState(manager.States.PropulsionState);
            }
        }

        public void CheckForPropulsionEnd(CharacterManager manager)
        {
            if (manager.Inputs.PropulsionType == 0)
            {
                manager.States.SwitchState(manager.States.FloatingState);
            }
        }

        public void UpdatePropulsionMovement(CharacterManager manager)
        {
            PropulsionDirection propulsionType = manager.Inputs.PropulsionType;
            Vector3 propulsionDirection = Vector3.zero;
            Vector3 lateralMoveInput = manager.Inputs.LastMoveInputDirection != Vector3.zero ? manager.Inputs.LastMoveInputDirection : Vector3.forward;
            Vector3 lateralMoveDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized * lateralMoveInput.z + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * lateralMoveInput.x).normalized;


            if (propulsionType.HasFlag(PropulsionDirection.Up)) propulsionDirection += Vector3.up;
            if (propulsionType.HasFlag(PropulsionDirection.Down)) propulsionDirection += Vector3.down;
            if (propulsionType.HasFlag(PropulsionDirection.Lateral)) propulsionDirection += lateralMoveDirection;
            propulsionDirection.Normalize();


            Vector3 propulsionMovement = propulsionDirection * BasePropulsionSpeed;
            CurrentVelocity += propulsionMovement;

        }

        public void CheckIfAirEmpty(CharacterManager manager)
        {
            if (!manager.AirManager.AirIsEmpty) return;

            manager.States.SwitchState(manager.States.FallingState);
        }

    }

}
