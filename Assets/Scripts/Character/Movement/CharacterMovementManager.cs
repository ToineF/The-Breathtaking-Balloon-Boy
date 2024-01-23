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
        public Vector3 CurrentVelocity { get; set; }

        [Header("Gravity")] // ADD TOOLTIPS
        [ReadOnly] public float CurrentGravity;
        [ReadOnly] public float MinGravity;
        [ReadOnly] public float MaxGravity;
        public float BaseGravity;
        public float BaseMaxGravity;
        public float GravityIncreaseByFrame;
        public float FloatingGravity;
        public float FloatingMaxGravity;

        [Header("Ground Check")]
        public float GroundCheckDistance;
        public float GroundDetectionSphereRadius;
        public LayerMask GroundLayer;
        public RaycastHit[] GroundHitResults { get; set; }

        [Header("Forces")]
        [ReadOnly] public Vector3 Force;
        [ReadOnly] public Vector3 CurrentForce;
        [ReadOnly] public bool IsGrounded;
        [HideInInspector] public RaycastHit LastGround;

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
            if (CharacterManager.Instance.Inputs.PropulsionDirection != 0)
            {
                manager.SwitchState(manager.PropulsionState);
            }
        }

        public void CheckForPropulsionEnd(CharacterStatesManager manager)
        {
            if (CharacterManager.Instance.Inputs.PropulsionDirection == 0)
            {
                manager.SwitchState(manager.FloatingState);
            }
        }

    }

}
