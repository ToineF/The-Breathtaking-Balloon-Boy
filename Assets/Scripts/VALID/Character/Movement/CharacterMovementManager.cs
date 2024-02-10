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

        // Slopes Data
        [field: SerializeField] public CharacterSlopesData SlopeData { get; private set; }


        [Tooltip("The current global velocity of the character (movements, gravity, forces...)")] public Vector3 CurrentVelocity { get; private set; }



        // Lateral Inputs (Idle, Walk, WASD) - Have this as a generic version for other movements
        private float _currentDeplacementSpeed;
        private Vector3 _currentDeplacementDirection;
        private Coroutine _currentDeplacementCoroutine;

        // Fall
        public float CurrentGravity { get; private set; }
        public float MinGravity { get; private set; }
        public float MaxGravity { get; private set; }
        public float CurrentGravityIncreaseByFrame { get; private set; }
        public float CurrentGravityIncreaseDeceleration { get; private set; }

        private Coroutine _currentFallCoroutine;

        // Propulsion Inputs (Propulsion) - Have this as a generic version for other movements
        private float _currentPropulsionSpeed;
        private Vector3 _currentPropulsionDirection;
        private Coroutine _currentPropulsionCoroutine;

        private float _currentPropulsionTakeOffSpeed;
        private Coroutine _currentPropulsionTakeOffSubCoroutine1;
        private Coroutine _currentPropulsionTakeOffSubCoroutine2;
        public float CurrentPropulsionIncreaseByFrame { get; private set; }


        // Ground Detection
        [Tooltip("The raycast hits stocked while looking for ground")] public RaycastHit[] GroundHitResults { get; private set; }
        [ReadOnly] public bool IsGrounded;
        public RaycastHit LastGround { get; private set; }


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
            SetGravityTo(GameManager.Instance.CharacterManager, FallData.BaseGravity, FallData.BaseMinGravity, FallData.BaseMaxGravity, FallData.BaseGravityIncreaseByFrame, FallData.BaseGravityIncreaseDecelerationByFrame);
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
            manager.CharacterRigidbody.velocity = Vector3.zero;

            manager.CharacterRigidbody.velocity += CurrentVelocity;
        }

        public void ResetVelocity()
        {
            CurrentVelocity = Vector3.zero;
        }

        #region Deplacement
        public void MoveAtSpeed(CharacterManager manager, float walkTurnSpeed, bool includesInputs = true)
        {
            UpdateSlopes(manager);

            Vector3 deplacementDirection = _currentDeplacementDirection;
            if (includesInputs) // Updates the Current Deplacement Value
            {
                deplacementDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)) * manager.Inputs.MoveInputDirection.z + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * manager.Inputs.MoveInputDirection.x).normalized;
                deplacementDirection = Vector3.Scale(deplacementDirection, new Vector3(1, 0, 1));
            }
            _currentDeplacementDirection = Vector3.Lerp(_currentDeplacementDirection, deplacementDirection, walkTurnSpeed);
            //SetAnimation(moveDirection);

            CurrentVelocity += _currentDeplacementDirection * _currentDeplacementSpeed;
        }

        // Generalize this to be more reusable (DO THIS ON STATE START)
        public void LerpDeplacementSpeed(CharacterManager manager, float targetValue, float lerpSpeed, AnimationCurve curve)
        {
            if (_currentDeplacementCoroutine != null) StopCoroutine(_currentDeplacementCoroutine);
            _currentDeplacementCoroutine = StartCoroutine(LerpWithEase(_currentDeplacementSpeed, targetValue, lerpSpeed, curve, (result) => _currentDeplacementSpeed = result));
        }
        #endregion

        #region Gravity
        public void CheckIfGrounded(CharacterManager manager, bool isPropulsing = false)
        {
            var lastGrounded = IsGrounded;
            CanJumpBuffer = Physics.SphereCastNonAlloc(manager.CharacterRigidbody.position, GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, JumpBufferHitResults, GroundDetectionData.JumpBufferCheckDistance, GroundDetectionData.GroundLayer) > 0;
            IsGrounded = Physics.SphereCastNonAlloc(manager.CharacterRigidbody.position, GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, GroundHitResults, GroundDetectionData.GroundCheckDistance, GroundDetectionData.GroundLayer) > 0;

            //if (IsGrounded)
            LastGround = GroundHitResults[0];

            if (lastGrounded != IsGrounded)
            {
                if (IsGrounded) // On Ground Enter
                {
                    OnGroundEnter?.Invoke();
                    CurrentGravity = FallData.BaseGravity;
                    manager.States.SwitchState(manager.States.IdleState);
                }
                else // On Ground Leave
                {
                    OnGroundExit?.Invoke();
                    if (!isPropulsing)
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
                manager.MovementManager.CurrentGravityIncreaseByFrame = Mathf.Max(manager.MovementManager.CurrentGravityIncreaseByFrame - manager.MovementManager.CurrentGravityIncreaseDeceleration, 0);
                manager.MovementManager.CurrentGravity = Mathf.Clamp(manager.MovementManager.CurrentGravity + manager.MovementManager.CurrentGravityIncreaseByFrame, manager.MovementManager.MinGravity, manager.MovementManager.MaxGravity);
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

            manager.MovementManager.CurrentVelocity += gravity;
            //CharacterManager.Instance.Force = Vector3.Lerp(CharacterManager.Instance.Force, CharacterManager.Instance.CurrentGravity, _lerpValue);
        }

        public void SetGravityTo(CharacterManager manager, float targetGravity, float minGravity, float maxGravity, float gravityIncreaseByFrame, float gravityIncreaseDeceleration)
        {
            manager.MovementManager.CurrentGravity = targetGravity;
            SetGravityMinMax(manager, targetGravity, minGravity, maxGravity);
            SetGravityIncrease(manager, gravityIncreaseByFrame, gravityIncreaseDeceleration);
        }

        private void SetGravityMinMax(CharacterManager manager, float targetGravity, float minGravity, float maxGravity)
        {
            manager.MovementManager.MinGravity = minGravity;
            manager.MovementManager.MaxGravity = maxGravity;
        }

        private void SetGravityIncrease(CharacterManager manager, float gravityIncreaseByFrame, float gravityIncreaseDeceleration)
        {
            manager.MovementManager.CurrentGravityIncreaseByFrame = gravityIncreaseByFrame;
            manager.MovementManager.CurrentGravityIncreaseDeceleration = gravityIncreaseDeceleration;

        }

        public void LerpGravityTo(CharacterManager manager, float targetGravity, float minGravity, float maxGravity, float gravityIncreaseByFrame, float gravityIncreaseDeceleration, float time, AnimationCurve curve)
        {
            if (_currentFallCoroutine != null) StopCoroutine(_currentFallCoroutine);
            SetGravityMinMax(manager, targetGravity, minGravity, maxGravity);
            SetGravityIncrease(manager, gravityIncreaseByFrame, gravityIncreaseDeceleration);
            _currentFallCoroutine = StartCoroutine(LerpWithEase(CurrentGravity, targetGravity, time, curve, (result) => CurrentGravity = result));
        }
        #endregion

        #region Propulsion
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
            CurrentPropulsionIncreaseByFrame = PropulsionData.PropulsionIncreaseByFrame;

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

            if (includesInputs)
            {
                // Increase speed over time
                CurrentPropulsionIncreaseByFrame = Math.Max(CurrentPropulsionIncreaseByFrame - PropulsionData.PropulsionIncreaseDeceleration, 0);
                _currentPropulsionSpeed = Math.Min(_currentPropulsionSpeed + CurrentPropulsionIncreaseByFrame / 100, PropulsionData.MaxPropulsionSpeed);
            }
            else
            {
                propulsionDirection = _currentPropulsionDirection;
            }
            _currentPropulsionDirection = Vector3.Lerp(_currentPropulsionDirection, propulsionDirection, PropulsionData.PropulsionDirectionTurnSpeed);


            float horizontalSpeed = _currentPropulsionSpeed * PropulsionData.HorizontalPropulsionMultiplier;
            float verticalSpeed = _currentPropulsionSpeed * PropulsionData.VerticalPropulsionMultiplier;

            Vector3 propulsionMovement = new Vector3(_currentPropulsionDirection.x * horizontalSpeed, _currentPropulsionDirection.y * verticalSpeed, _currentPropulsionDirection.z * horizontalSpeed);
            CurrentVelocity += propulsionMovement;
        }

        public void UpdateTakeOffPropulsionMovement(CharacterManager manager)
        {
            CurrentVelocity += Vector3.up * _currentPropulsionTakeOffSpeed;
        }

        public void CheckForFloatCancel(CharacterManager manager)
        {
            if (!manager.Inputs.StartedFalling) return;

            manager.Inputs.ResetLastPropulsionInputDirection();

            manager.States.SwitchState(manager.States.FallingState);
        }
        #endregion

        #region Air
        public void FallfAirEmpty(CharacterManager manager)
        {
            if (!manager.AirManager.AirIsEmpty) return;

            manager.States.SwitchState(manager.States.FallingState);
        }
        #endregion

        #region Slopes
        // Slopes

        private void UpdateSlopes(CharacterManager manager)
        {
            if (!IsGrounded) return;

            var height = manager.CharacterCollider.bounds.center.y;
            var radius = manager.CharacterCollider.bounds.extents.magnitude;
            //var SphereCastVerticalOffset = height / 2 - radius;
            var castOrigin = manager.CharacterRigidbody.position;

            //IsGrounded = Physics.SphereCastNonAlloc(manager.CharacterRigidbody.position, GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, GroundHitResults, GroundDetectionData.GroundCheckDistance, GroundDetectionData.GroundLayer) > 0;

            //if (Physics.SphereCast(castOrigin, GroundDetectionData.GroundDetectionSphereRadius - 0.01f, Vector3.down, out var hit, GroundDetectionData.GroundCheckDistance + 1f, GroundDetectionData.GroundLayer, QueryTriggerInteraction.Ignore))
            //{
            //    var collider = hit.collider;
            //    var angle = Vector3.Angle(Vector3.up, hit.normal);
            //    Debug.DrawLine(hit.point, hit.point + hit.normal, Color.black, 3f);
            //}


            var collider = LastGround.collider;
            var angle = Vector3.Angle(Vector3.up, LastGround.normal);
            //Debug.DrawLine(LastGround.point, LastGround.point + LastGround.normal, Color.black, 3f);
            Debug.DrawLine(castOrigin, castOrigin + LastGround.normal, Color.black, 3f);

            Debug.LogWarning(LastGround.point);

        }

        private bool OnSlope()
        {
            if (LastGround.collider == null) return false;

            float angle = Vector3.Angle(Vector3.up, LastGround.normal);
            //Debug.Log(angle);
            return angle < SlopeData.MaxSlopeAngle && angle != 0;
        }

        private Vector3 GetSlopeMoveDirection()
        {
            return Vector3.ProjectOnPlane(_currentDeplacementDirection, LastGround.normal).normalized;
        }


        // REMOVE THIS
        private void OnDrawGizmos()
        {
            if (GameManager.Instance == null) return;

            Gizmos.color = Color.white;

            if (OnSlope())
            {
                Gizmos.color = Color.red;
                //Debug.Log(LastGround.normal);
                if (LastGround.collider == null) Gizmos.color = Color.green;
            }

            Vector3 direction = GetSlopeMoveDirection();
            Vector3 position = GameManager.Instance.CharacterManager.CharacterRigidbody.position;
            Gizmos.DrawLine(position, position + direction);

            //if (Physics.SphereCast(transform.position, 0.5f, -transform.up, out hit, 0.25f))
            //Gizmos.DrawWireSphere(transform.position - transform.up * 0.25f, 0.5f);
            //
            //Physics.SphereCastNonAlloc(manager.CharacterVisual.position, GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, GroundHitResults, GroundDetectionData.GroundCheckDistance, GroundDetectionData.GroundLayer) > 0;
            Gizmos.DrawWireSphere(GameManager.Instance.CharacterManager.CharacterVisual.position + Vector3.down * GroundDetectionData.GroundCheckDistance, GroundDetectionData.GroundDetectionSphereRadius);
        }
        #endregion

    }

}
