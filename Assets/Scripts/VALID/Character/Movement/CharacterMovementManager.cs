using System;
using UnityEngine;
using System.Collections;
using BlownAway.Character.Inputs;
using BlownAway.Character.Movements.Data;

namespace BlownAway.Character.Movements
{

    public class CharacterMovementManager : MonoBehaviour // RANGER CE SCRIPT !!!
    {
        public Action OnGroundEnter;
        public Action OnGroundExit;

        // Idle & Walk Data
        [field: SerializeField] public CharacterLateralMovementsData LateralMovementData { get; private set; }

        // Fall Data
        [field: SerializeField] public CharacterFallingData FallData { get; private set; }

        // Propulsion Data
        [field: SerializeField] public CharacterPropulsionData PropulsionData { get; private set; }

        // Ground Detection Data
        [field: SerializeField] public CharacterGroundDetectionData GroundDetectionData { get; private set; }



        [Tooltip("The current global velocity of the character (movements, gravity, forces...)")] public Vector3 CurrentVelocity { get; private set; }



        // Lateral Inputs (Idle, Walk, WASD) - Have this as a generic version for other movements
        private float _currentDeplacementSpeed;
        private Vector3 _currentDeplacementDirection;
        private Coroutine _currentDeplacementCoroutine;

        // Fall
        private float _currentGravity { get; set; }
        private float _minGravity { get; set; }
        private float _maxGravity { get; set; }

        private Coroutine _currentFallCoroutine;

        // Propulsion Inputs (Propulsion) - Have this as a generic version for other movements
        private float _currentPropulsionSpeed;
        private Vector3 _currentPropulsionDirection;
        private Coroutine _currentPropulsionCoroutine;

        private float _currentPropulsionTakeOffSpeed;
        private Coroutine _currentPropulsionTakeOffSubCoroutine1;
        private Coroutine _currentPropulsionTakeOffSubCoroutine2;


        // Ground Detection
        [Tooltip("The raycast hits stocked while looking for ground")] public RaycastHit[] GroundHitResults { get; private set; }
        [ReadOnly] public bool IsGrounded;
        [HideInInspector] public RaycastHit LastGround;


        // Jump buffer
        public bool CanJumpBuffer { get; private set; }
        [Tooltip("The raycast hits stocked while looking for jump buffer")] public RaycastHit[] JumpBufferHitResults { get; private set; }


        /*
        [Header("Forces")]
        [ReadOnly] public Vector3 Force;
        [ReadOnly] public Vector3 CurrentForce;*/


        private void Start()
        {
            GroundHitResults = new RaycastHit[2];
            JumpBufferHitResults = new RaycastHit[2];
            SetGravityTo(GameManager.Instance.CharacterManager, FallData.BaseGravity, FallData.BaseMinGravity, FallData.BaseMaxGravity);
        }


        public void MoveAtSpeed(CharacterManager manager, float walkTurnSpeed, bool includesInputs = true)
        {
            Vector3 deplacementDirection = _currentDeplacementDirection;
            if (includesInputs) // Updates the Current Deplacement Value
            {
                deplacementDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)) * manager.Inputs.MoveInputDirection.z + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * manager.Inputs.MoveInputDirection.x).normalized;
                deplacementDirection = Vector3.Scale(deplacementDirection, new Vector3(1, 0, 1));
            }
            _currentDeplacementDirection = Vector3.Lerp(_currentDeplacementDirection, deplacementDirection, walkTurnSpeed);
            //SetAnimation(moveDirection);

            CurrentVelocity += _currentDeplacementDirection * _currentDeplacementSpeed * Time.deltaTime;
        }

        // Generalize this to be more reusable (DO THIS ON STATE START)
        public void LerpDeplacementSpeed(CharacterManager manager, float targetValue, float lerpSpeed, AnimationCurve curve)
        {
            if (_currentDeplacementCoroutine != null) StopCoroutine(_currentDeplacementCoroutine);
            _currentDeplacementCoroutine = StartCoroutine(LerpWithEase(_currentDeplacementSpeed, targetValue, lerpSpeed, curve, (result) => _currentDeplacementSpeed = result));
        }

        // HERE SORT AS A GENERIC THING
        private IEnumerator LerpWithEase(float value, float targetValue, float targetTime, AnimationCurve curve, Action<float> updateAction, IEnumerator endCoroutine = null)
        {
            float time = 0;

            while (time < targetTime)
            {
                time += Time.deltaTime;
                float weight = curve.Evaluate(time / targetTime);
                //Debug.Log(time / targetTime);
                value = Mathf.Lerp(value, targetValue, weight);
                updateAction?.Invoke(value);
                yield return null;
            }

            if (endCoroutine != null) StartCoroutine(endCoroutine);
        }

        private IEnumerator LerpWithEaseBack(float value, float targetValue, float inTime, float outTime, AnimationCurve inCurve, AnimationCurve outCurve, Action<float> updateAction, IEnumerator endCoroutine = null)
        {
            _currentPropulsionTakeOffSubCoroutine1 = StartCoroutine(LerpWithEase(value, targetValue, inTime, inCurve, updateAction, endCoroutine));

            yield return new WaitForSeconds(inTime);

            _currentPropulsionTakeOffSubCoroutine2 = StartCoroutine(LerpWithEase(value, value, outTime, outCurve, updateAction, endCoroutine));
        }


        public void ApplyVelocity(CharacterManager manager)
        {
            manager.CharacterRigidbody.velocity = CurrentVelocity;
        }

        public void ResetVelocity()
        {
            CurrentVelocity = Vector3.zero;
        }

        public void CheckIfGrounded(CharacterManager manager, bool isPropulsing = false)
        {
            var lastGrounded = IsGrounded;
            IsGrounded = Physics.SphereCastNonAlloc(manager.CharacterTransform.position, GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, GroundHitResults, GroundDetectionData.GroundCheckDistance, GroundDetectionData.GroundLayer) > 0;
            CanJumpBuffer = Physics.SphereCastNonAlloc(manager.CharacterTransform.position, GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, JumpBufferHitResults, GroundDetectionData.JumpBufferCheckDistance, GroundDetectionData.GroundLayer) > 0;
            if (lastGrounded != IsGrounded)
            {
                if (IsGrounded) // On Ground Enter
                {
                    LastGround = GroundHitResults[0];
                    OnGroundEnter?.Invoke();
                    _currentGravity = FallData.BaseGravity;
                    manager.States.SwitchState(manager.States.IdleState);
                }
                else // On Ground Leave
                {
                    OnGroundExit?.Invoke();
                    if (isPropulsing)
                    {
                        manager.States.SwitchState(manager.States.FloatingState); // FLOATING & PROPLUSION
                    }
                    else
                    {
                        manager.States.SwitchState(manager.States.FallingState); // IDLE, WALK & FALL
                    }
                }
            }
        }

        public void UpdateGravity(CharacterManager manager)
        {
            if (!manager.MovementManager.IsGrounded)
            {
                manager.MovementManager._currentGravity = Mathf.Clamp(manager.MovementManager._currentGravity + manager.MovementManager.FallData.GravityIncreaseByFrame, manager.MovementManager._minGravity, manager.MovementManager._maxGravity);
            }

            /*Vector3 additionalForces = Vector3.zero;
            foreach (var force in _additionnalForces)
            {
                additionalForces += force.Value.CurrentForce;
                force.Value.CurrentForce = Vector3.Lerp(force.Value.CurrentForce, force.Value.TargetForce, force.Value.ForceLerp);
            }*/
            Vector3 gravity = -manager.MovementManager._currentGravity * Vector3.up;
            //Vector3 allForces = CharacterManager.Instance + additionalForces + gravity;

            //_characterController.Move(allForces * Time.deltaTime);

            manager.MovementManager.CurrentVelocity += gravity * Time.deltaTime;
            //CharacterManager.Instance.Force = Vector3.Lerp(CharacterManager.Instance.Force, CharacterManager.Instance.CurrentGravity, _lerpValue);
        }

        public void SetGravityTo(CharacterManager manager, float targetGravity, float minGravity, float maxGravity)
        {
            manager.MovementManager._currentGravity = targetGravity;
            SetGravityMinMax(manager, targetGravity, minGravity, maxGravity);
        }

        public void SetGravityMinMax(CharacterManager manager, float targetGravity, float minGravity, float maxGravity)
        {
            manager.MovementManager._minGravity = minGravity;
            manager.MovementManager._maxGravity = maxGravity;
        }

        public void LerpGravityTo(CharacterManager manager, float targetGravity, float minGravity, float maxGravity, float time, AnimationCurve curve)
        {
            if (_currentFallCoroutine != null) StopCoroutine(_currentFallCoroutine);
            SetGravityMinMax(manager, targetGravity, minGravity, maxGravity);
            _currentFallCoroutine = StartCoroutine(LerpWithEase(_currentGravity, targetGravity, time, curve, (result) => _currentGravity = result));
        }

        public void CheckForFloatCancel(CharacterManager manager)
        {
            if (!manager.Inputs.StartedFalling) return;

            manager.Inputs.ResetLastPropulsionInputDirection();

            manager.States.SwitchState(manager.States.FallingState);
        }


        // Float & Propulsion
        public void LerpPropulsionSpeed(CharacterManager manager, float targetValue, float lerpSpeed, AnimationCurve curve)
        {
            if (_currentPropulsionCoroutine != null) StopCoroutine(_currentPropulsionCoroutine);
            _currentPropulsionCoroutine = StartCoroutine(LerpWithEase(_currentPropulsionSpeed, targetValue, lerpSpeed, curve, (result) => _currentPropulsionSpeed = result));
        }

        public void LerpPropulsionTakeOffSpeed(CharacterManager manager, float startTargetValue, float startLerpSpeed, AnimationCurve startCurve, float endTargetValue, float endLerpSpeed, AnimationCurve endCurve)
        {
            if (_currentPropulsionTakeOffSubCoroutine1 != null)
            {
                _currentPropulsionTakeOffSpeed = 0;
                StopCoroutine(_currentPropulsionTakeOffSubCoroutine1);
            }
            if (_currentPropulsionTakeOffSubCoroutine2 != null) StopCoroutine(_currentPropulsionTakeOffSubCoroutine2);

            StartCoroutine(LerpWithEaseBack(_currentPropulsionTakeOffSpeed, startTargetValue, startLerpSpeed, endLerpSpeed, startCurve, endCurve, (result) => _currentPropulsionTakeOffSpeed = result));
        }

        public void CheckForPropulsionStartOnGround(CharacterManager manager)
        {
            if (manager.Inputs.PropulsionType.HasFlag(PropulsionDirection.Up) || manager.Inputs.PropulsionType.HasFlag(PropulsionDirection.Lateral))
            {
                manager.AirManager.RefreshAir();
                PropulsionStart(manager);
                LerpPropulsionTakeOffSpeed(manager, PropulsionData.PropulsionTakeOffSpeed, PropulsionData.PropulsionTakeOffAccelTime, PropulsionData.PropulsionTakeOffAccelCurve, 0, PropulsionData.PropulsionTakeOffDecelTime, PropulsionData.PropulsionTakeOffDecelCurve);
            }
        }

        public void CheckForPropulsionStartOnAir(CharacterManager manager)
        {
            if (CanJumpBuffer && !manager.AirManager.AirIsFull)
            {
                CheckForPropulsionStartOnGround(manager);
                return;
            }

            if (manager.AirManager.AirIsEmpty) return;

            if (manager.Inputs.PropulsionType != 0)
            {
                PropulsionStart(manager);
            }
        }

        private void PropulsionStart(CharacterManager manager)
        {
            manager.States.SwitchState(manager.States.PropulsionState);

        }

        public void CheckForPropulsionEnd(CharacterManager manager)
        {
            if (manager.Inputs.PropulsionType == 0)
            {
                manager.States.SwitchState(manager.States.FloatingState);
            }
        }

        public void UpdatePropulsionMovement(CharacterManager manager, bool includesInputs = true)
        {
            UpdateContinuousPropulsionMovement(manager, includesInputs);
            UpdateTakeOffPropulsionMovement(manager);
        }

        private void UpdateContinuousPropulsionMovement(CharacterManager manager, bool includesInputs)
        {
            PropulsionDirection propulsionType = manager.Inputs.PropulsionType;
            Vector3 propulsionDirection = Vector3.zero;
            Vector3 lateralMoveInput = manager.Inputs.LastMoveInputDirection != Vector3.zero ? manager.Inputs.LastMoveInputDirection : Vector3.forward;
            Vector3 lateralMoveDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized * lateralMoveInput.z + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * lateralMoveInput.x).normalized;


            if (propulsionType.HasFlag(PropulsionDirection.Up)) propulsionDirection += Vector3.up;
            if (propulsionType.HasFlag(PropulsionDirection.Down)) propulsionDirection += Vector3.down;
            if (propulsionType.HasFlag(PropulsionDirection.Lateral)) propulsionDirection += lateralMoveDirection;
            propulsionDirection.Normalize();

            if (!includesInputs) propulsionDirection = _currentPropulsionDirection;
            _currentDeplacementDirection = Vector3.Lerp(_currentDeplacementDirection, propulsionDirection, PropulsionData.PropulsionDirectionTurnSpeed);


            float horizontalSpeed = _currentPropulsionSpeed * PropulsionData.HorizontalPropulsionSpeed;
            float verticalSpeed = _currentPropulsionSpeed * PropulsionData.VerticalPropulsionSpeed;

            Vector3 propulsionMovement = new Vector3(_currentDeplacementDirection.x * horizontalSpeed, _currentDeplacementDirection.y * verticalSpeed, _currentDeplacementDirection.z * horizontalSpeed);
            CurrentVelocity += propulsionMovement * Time.deltaTime;
        }

        public void UpdateTakeOffPropulsionMovement(CharacterManager manager)
        {
            CurrentVelocity += Vector3.up * _currentPropulsionTakeOffSpeed * Time.deltaTime;
        }

        public void FallfAirEmpty(CharacterManager manager)
        {
            if (!manager.AirManager.AirIsEmpty) return;

            manager.States.SwitchState(manager.States.FallingState);
        }

    }

}
