using BlownAway.Character.Inputs;
using BlownAway.Character.States;
using System;
using UnityEngine;

namespace BlownAway.Character.Movements
{

    public class CharacterMovementManager : MonoBehaviour
    {
        public Action OnGroundEnter;
        public Action OnGroundExit;

        // Idle Data
        /// /////////////////////////////////////////////////////////// PUT IN A SCRIPTABLE OBJECT
        [field: SerializeField, Tooltip("The walking speed the character starts moving at")] public float BaseWalkSpeed { get; set; }
        [field: SerializeField, Tooltip("The lateral speed the character moves at while falling")] public float FallDeplacementSpeed { get; set; }
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

        [Header("Ground Check")]
        [Tooltip("The distance offset of the ground detection check from the character")] public float GroundCheckDistance;
        [Tooltip("The radius of the ground detection sphere collider")] public float GroundDetectionSphereRadius;
        [Tooltip("The layer of the ground the character can walk on")] public LayerMask GroundLayer;
        [Tooltip("The raycast hits stocked while looking for ground")] public RaycastHit[] GroundHitResults { get; set; }
        [ReadOnly] public bool IsGrounded;
        [HideInInspector] public RaycastHit LastGround;

        [Header("Propulsion")]
        [Tooltip("The base speed the character moves at while propulsing")] public float BasePropulsionSpeed;

        /*
        [Header("Forces")]
        [ReadOnly] public Vector3 Force;
        [ReadOnly] public Vector3 CurrentForce;*/


        private void Start()
        {
            GroundHitResults = new RaycastHit[2];
            SetGravityTo(BaseGravity, BaseMaxGravity);
        }

        
        public void MoveAtSpeed(float moveSpeed)
        {
            Vector3 moveDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)) * CharacterManager.Instance.Inputs.MoveInputDirection.z + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * CharacterManager.Instance.Inputs.MoveInputDirection.x).normalized;
            moveDirection = Vector3.Scale(moveDirection, new Vector3(1, 0, 1));
            //SetAnimation(moveDirection);
            CurrentVelocity += moveDirection * moveSpeed * Time.deltaTime;
        }

        public void ApplyVelocity()
        {
            CharacterManager.Instance.CharacterRigidbody.velocity = CurrentVelocity;
        }

        public void ResetVelocity()
        {
            CurrentVelocity = Vector3.zero;
        }

        public void CheckIfGrounded(CharacterStatesManager manager)
        {
            var lastGrounded = IsGrounded;
            IsGrounded = Physics.SphereCastNonAlloc(CharacterManager.Instance.CharacterTransform.position, GroundDetectionSphereRadius, Vector3.down, GroundHitResults, GroundCheckDistance, GroundLayer) > 0;
            if (lastGrounded != IsGrounded)
            {
                if (IsGrounded) // On Ground Enter
                {
                    LastGround = GroundHitResults[0];
                    OnGroundEnter?.Invoke();
                    CurrentGravity = BaseGravity;
                    manager.SwitchState(manager.IdleState);
                }
                else // On Ground Leave
                {
                    OnGroundExit?.Invoke();
                    manager.SwitchState(manager.FallingState); // HERE DISSOCIATE FALLING FROM PROPULSION
                }
            }
        }

        public void UpdateGravity()
        {
            if (!CharacterManager.Instance.MovementManager.IsGrounded)
            {
                CharacterManager.Instance.MovementManager.CurrentGravity = Mathf.Clamp(CharacterManager.Instance.MovementManager.CurrentGravity + CharacterManager.Instance.MovementManager.GravityIncreaseByFrame, CharacterManager.Instance.MovementManager.MinGravity, CharacterManager.Instance.MovementManager.MaxGravity);
            }

            /*Vector3 additionalForces = Vector3.zero;
            foreach (var force in _additionnalForces)
            {
                additionalForces += force.Value.CurrentForce;
                force.Value.CurrentForce = Vector3.Lerp(force.Value.CurrentForce, force.Value.TargetForce, force.Value.ForceLerp);
            }*/
            Vector3 gravity = -CharacterManager.Instance.MovementManager.CurrentGravity * Vector3.up;
            //Vector3 allForces = CharacterManager.Instance + additionalForces + gravity;

            //_characterController.Move(allForces * Time.deltaTime);

            CharacterManager.Instance.MovementManager.CurrentVelocity += gravity * Time.deltaTime;
            //CharacterManager.Instance.Force = Vector3.Lerp(CharacterManager.Instance.Force, CharacterManager.Instance.CurrentGravity, _lerpValue);
        }

        public void SetGravityTo(float targetGravity, float maxGravity)
        {
            CharacterManager.Instance.MovementManager.CurrentGravity = targetGravity;
            CharacterManager.Instance.MovementManager.MinGravity = targetGravity;
            CharacterManager.Instance.MovementManager.MaxGravity = maxGravity;
        }



        // Float & Propulsion
        public void CheckForPropulsionStart(CharacterStatesManager manager)
        {
            if (CharacterManager.Instance.Inputs.PropulsionType != 0)
            {
                manager.SwitchState(manager.PropulsionState);
            }
        }

        public void CheckForPropulsionEnd(CharacterStatesManager manager)
        {
            if (CharacterManager.Instance.Inputs.PropulsionType == 0)
            {
                manager.SwitchState(manager.FloatingState);
            }
        }

        public void UpdatePropulsionMovement()
        {
            PropulsionDirection propulsionType = CharacterManager.Instance.Inputs.PropulsionType;
            Vector3 propulsionDirection = Vector3.zero;
            // Case LastMoveInputDirection is Vector3.zero (if player never moved)
            Vector3 lateralMoveDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized * CharacterManager.Instance.Inputs.LastMoveInputDirection.z + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * CharacterManager.Instance.Inputs.LastMoveInputDirection.x).normalized;


            if (propulsionType.HasFlag(PropulsionDirection.Up)) propulsionDirection += Vector3.up;
            if (propulsionType.HasFlag(PropulsionDirection.Down)) propulsionDirection += Vector3.down;
            if (propulsionType.HasFlag(PropulsionDirection.Lateral)) propulsionDirection += lateralMoveDirection;
            propulsionDirection.Normalize();


            Vector3 propulsionMovement = propulsionDirection * BasePropulsionSpeed;
            CurrentVelocity += propulsionMovement;

        }

    }

}
