using BlownAway.Character.States;
using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using BlownAway.Camera;

namespace BlownAway.Character
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
        [field:SerializeField, Tooltip("The lateral speed the character moves at while falling")] public float FallDeplacementSpeed { get; set; }
        public Vector3 MoveInputDirection { get; set; }
        public Vector3 CurrentVelocity { get; set; }

        // Camera Data
        [SerializeField] private EntityCamera CameraParams;
        private Vector2 CameraMoveVector;
        private bool IsMouse;
        private Vector2 _currentCameraAngle;


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
        public void SetCameraTypeMouse(InputAction.CallbackContext context)
        {
            IsMouse = true;
            CameraMoveVector = context.ReadValue<Vector2>();
        }

        public void SetCameraTypeController(InputAction.CallbackContext context)
        {
            Debug
                .Log
                    ('d')
                        ;
            IsMouse = false;
            CameraMoveVector = context.ReadValue<Vector2>();
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
        public void MoveAtSpeed(float moveSpeed)
        {
            Vector3 moveDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)) * MoveInputDirection.z + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * MoveInputDirection.x).normalized;
            moveDirection = Vector3.Scale(moveDirection, new Vector3(1, 0, 1));
            //SetAnimation(moveDirection);
            CurrentVelocity += moveDirection * moveSpeed * Time.deltaTime;
            //UpdateCamera();
        }

        public void ApplyVelocity()
        {
            CharacterRigidbody.velocity = CurrentVelocity;
        }

        public void ResetVelocity()
        {
            CurrentVelocity = Vector3.zero;
        }

        public void UpdateCamera() // IN update
        {
            float YPosition = CameraParams.YOffset + _currentCameraAngle.y;
            Vector3 cameraVector = new Vector3((float)Math.Cos(_currentCameraAngle.x), YPosition, (float)Math.Sin(_currentCameraAngle.x)).normalized * int.MaxValue;
            Vector3 newPosition = transform.position + cameraVector;
            CameraParams.transform.position = newPosition;

            CameraParams.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = CameraParams.PositionOffset;
            CameraParams.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = CameraParams.CameraDistance;
        }

        public void MoveCamera() // in late update
        {
            if (Time.timeScale == 0) return;

            float sensitivity = IsMouse ? CameraParams.MouseSensitivity : CameraParams.ControllerSensitivity;
            float xSign = CameraParams.IsXInverted ? -1 : 1;
            float ySign = CameraParams.IsYInverted ? -1 : 1;
            _currentCameraAngle += new Vector2(CameraMoveVector.x * xSign, CameraMoveVector.y * ySign) * sensitivity;
            _currentCameraAngle.y = Math.Clamp(_currentCameraAngle.y, -CameraParams.YDeadZone, CameraParams.YDeadZone);

        }


        // HERE REMOVE START/UPDATE...  (SHOULD ONLY CONTAINS INFORMATIONS)
        private void Start()
        {
            GroundHitResults = new RaycastHit[2];
            CurrentGravity = BaseGravity;
        }

    }
}