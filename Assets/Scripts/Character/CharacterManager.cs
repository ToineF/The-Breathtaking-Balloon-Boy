using Character.States;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class CharacterManager : Singleton<CharacterManager> /// CLEAN UP THIS MESS OF A CLASS
    {
        public Action OnGroundEnter;
        public Action OnGroundExit;

        [field: SerializeField] public Rigidbody CharacterRigidbody { get; set; }
        [field: SerializeField] public Transform CharacterTransform { get; set; }


        // Idle Data
        /// /////////////////////////////////////////////////////////// PUT IN A SCRIPTABLE OBJECT
        [field:SerializeField, Tooltip("The walking speed the character starts moving at")] public float BaseWalkSpeed { get; set; }
        public Vector3 MoveInputDirection { get; set; }

        [Header("Gravity")]
        [ReadOnly] public float CurrentGravity;
        public float BaseGravity;
        public float MaxGravity;
        public float FloatingGravity;
        public float GravityIncreaseByFrame;

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

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            float xPosition = context.ReadValue<Vector2>().x;
            float zPosition = context.ReadValue<Vector2>().y;
            MoveInputDirection = new Vector3(xPosition, 0, zPosition);
        }

        public void CheckIfGrounded(CharacterStatesManager manager)
        {
            var lastGrounded = IsGrounded;
            IsGrounded = Physics.SphereCastNonAlloc(CharacterTransform.position, GroundDetectionSphereRadius, Vector3.down, GroundHitResults, GroundCheckDistance, GroundLayer) > 0;
            if (lastGrounded != IsGrounded)
            {
                if (IsGrounded) // On Ground Enter
                {
                    LastGround = GroundHitResults[0];
                    OnGroundEnter?.Invoke();
                    CurrentGravity = BaseGravity;
                    manager.SwitchState(manager.IdleState);
                } else // On Ground Leave
                {
                    OnGroundExit?.Invoke();
                    manager.SwitchState(manager.FallingState); // HERE DISCOSSIATE FALLING FROM PROPULSION
                }
            }
        }

        // HERE REMOVE START AND UPDATE  (SHOULD ONLY CONTAINS INFORMATIONS)
        private void Start()
        {
            GroundHitResults = new RaycastHit[2];
            CurrentGravity = BaseGravity;
        }
        private void Update()
        {
            Debug.Log(IsGrounded);
        }

    }
}