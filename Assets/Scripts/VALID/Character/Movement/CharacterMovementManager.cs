using System;
using UnityEngine;
using System.Collections;
using BlownAway.Character.Inputs;
using AntoineFoucault.Utilities;
using System.Collections.Generic;
using BlownAway.GPE;
using BlownAway.Character.Movements.Data;

namespace BlownAway.Character.Movements
{
    public class CharacterMovementManager : CharacterSubComponent // RANGER CE SCRIPT !!!
    {
        public Action<CharacterManager> OnGroundEnter { get; set; }
        public Action<CharacterManager> OnGroundExit { get; set; }

        [Tooltip("The current global velocity of the character (movements, gravity, forces...)")] public Vector3 CurrentVelocity { get; private set; }



        // Lateral Inputs (Idle, Walk, WASD) - Have this as a generic version for other movements
        private float _currentDeplacementSpeed;
        private Vector3 _currentDeplacementDirection;
        private float _currentDeplacemenTurnSpeed;
        private Coroutine _currentDeplacementCoroutine;

        // Fall
        public float CurrentGravity { get; private set; }
        public float MinGravity { get; private set; }
        public float MaxGravity { get; private set; }
        public float CurrentGravityIncreaseByFrame { get; private set; }
        public float CurrentGravityIncreaseDeceleration { get; private set; }

        private Coroutine _currentFallCoroutine;

        // Propulsion Inputs (Propulsion) - Have this as a generic version for other movements
        public bool IsJacketInflated { get; private set; }

        public float PropulsionTimer { get; private set; } // Minimum time after which the propulsion is allowed to end
        private float _currentPropulsionSpeed;
        private Vector3 _currentPropulsionDirection;
        private Coroutine _currentPropulsionCoroutine;

        private float _currentPropulsionTakeOffSpeed;
        private Coroutine _currentPropulsionTakeOffSubCoroutine1;
        private Coroutine _currentPropulsionTakeOffSubCoroutine2;
        public float CurrentPropulsionIncreaseByFrame { get; private set; }

        // Jump
        public float JumpTimer { get; private set; }
        public float JumpPropulsionTimer { get; private set; }
        private float _currentJumpSpeed;
        private float _currentJumpIncreaseByFrame;
        public CharacterJumpState.JumpState _currentJumpState { get; private set; }

        // Dash
        public float CurrentDashes { get; private set; }
        private float _dashTimer;
        private Vector3 _currentDashDirection;

        // Derive
        public float DeriveTimer { get; private set; }
        public float NormalizedDeriveAirAmount { get => DeriveTimer / Mathf.Max(Manager.Data.PropulsionData.DeriveTime,0.0001f); }


        // Ground Detection
        [Tooltip("The raycast hits stocked while looking for ground")] public RaycastHit[] GroundHitResults { get; private set; }
        public bool IsGrounded { get; private set; }
        public RaycastHit LastGround { get; private set; }

        // Slopes
        [Tooltip("The raycast hits stocked while looking for slopes")] public RaycastHit[] SlopesHitResults { get; private set; }



        // Jump buffer
        public bool CanJumpBuffer { get; private set; }
        [Tooltip("The raycast hits stocked while looking for jump buffer")] public RaycastHit[] JumpBufferHitResults { get; private set; }


        // Ground Pound
        public float GroundPoundTotalHeight { get; set; }
        public float GroundPoundCancelTime { get; set; }
        public RaycastHit[] GroundPoundHitResults { get; set; }
        public bool HasBalloonGroundPound { get; set; }

        private Transform parent;

        // External Forces
        public Dictionary<GameObject, ForceData> ExternalForces { get; private set; } = new Dictionary<GameObject, ForceData>();

        protected override void StartScript(CharacterManager manager)
        {
            parent = manager.CharacterCollider.Rigidbody.transform.parent;
            GroundHitResults = new RaycastHit[2];
            SlopesHitResults = new RaycastHit[2];
            JumpBufferHitResults = new RaycastHit[2];
            GroundPoundHitResults = new RaycastHit[2];
            SetGravityTo(manager, manager.Data.FallData.BaseData.BaseGravity, manager.Data.FallData.BaseData.MinGravity, manager.Data.FallData.BaseData.MaxGravity, manager.Data.FallData.BaseData.GravityIncreaseByFrame, manager.Data.FallData.BaseData.GravityIncreaseDecelerationByFrame);
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
            CurrentVelocity = ColliderAndSlide(manager, CurrentVelocity, Vector3.zero, 0, false, CurrentVelocity);
            manager.CharacterCollider.Rigidbody.velocity = CurrentVelocity;
        }

        public void ResetVelocity()
        {
            CurrentVelocity = Vector3.zero;
        }

        public void UpdateExternalForces()
        {
            Vector3 externalForces = GetExternalForces();
            CurrentVelocity += externalForces;
        }

        #region Deplacement
        public void MoveAtSpeed(CharacterManager manager)
        {
            Vector3 deplacementDirection = _currentDeplacementDirection;
            Vector3 groundDirection = new Vector3(1, 0, 1);

            deplacementDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)) * manager.Inputs.MoveInputDirection.z + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * manager.Inputs.MoveInputDirection.x).normalized;

            deplacementDirection = Vector3.Scale(deplacementDirection, new Vector3(1, 0, 1));

            //if (Vector3.Angle(_currentDeplacementDirection, deplacementDirection) > 170f) Debug.LogWarning("turn");
            //Debug.LogWarning(Vector3.Angle(_currentDeplacementDirection, deplacementDirection));
            //if (Vector3.Angle(_currentDeplacementDirection, deplacementDirection) > 170f) 
            //    deplacementDirection = Quaternion.AngleAxis(-70, Vector3.up) * deplacementDirection;

            //Debug.LogWarning(deplacementDirection);


            if (IsGrounded) deplacementDirection = GetSlopeMoveDirection(deplacementDirection);
            //Debug.LogWarning(deplacementDirection);

            _currentDeplacementDirection = Vector3.Lerp(_currentDeplacementDirection, deplacementDirection, _currentDeplacemenTurnSpeed);

            //if (IsGrounded && OnSlope())
            //     _currentDeplacementDirection = deplacementDirection; // HERE SEE SLOPES

            //Debug.LogWarning(_currentDeplacementDirection * _currentDeplacementSpeed);


            CurrentVelocity += _currentDeplacementDirection * _currentDeplacementSpeed;

            manager.MovementManager.CheckForStepClimb(manager);
        }

        public void StopMoving(CharacterManager manager)
        {
            _currentDeplacementDirection = Vector3.Lerp(_currentDeplacementDirection, Vector3.zero, _currentDeplacemenTurnSpeed);
            CurrentVelocity += _currentDeplacementDirection * _currentDeplacementSpeed;

        }

        public void ResetDeplacementDirection()
        {
            _currentDeplacementDirection = Vector3.zero;
        }

        // Generalize this to be more reusable (DO THIS ON STATE START)
        public void LerpDeplacementSpeed(CharacterManager manager, Data.LateralData data)
        {
            if (_currentDeplacementCoroutine != null) StopCoroutine(_currentDeplacementCoroutine);
            _currentDeplacementCoroutine = StartCoroutine(LerpWithEase(_currentDeplacementSpeed, data.DeplacementLateralSpeed, data.DeplacementTime, data.DeplacementCurve, (result) => _currentDeplacementSpeed = result));
            _currentDeplacemenTurnSpeed = data.DirectionTurnSpeed;
        }
        #endregion

        #region Gravity
        public void CheckIfGrounded(CharacterManager manager, bool isPropulsing = false)
        {
            var lastGrounded = IsGrounded;
            Vector3 colliderPosition = new Vector3(manager.CharacterCollider.Collider.bounds.center.x, manager.CharacterCollider.Collider.bounds.min.y, manager.CharacterCollider.Collider.bounds.center.z);
            CanJumpBuffer = Physics.SphereCastNonAlloc(colliderPosition, manager.Data.GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, JumpBufferHitResults, manager.Data.GroundDetectionData.JumpBufferCheckDistance, manager.Data.GroundDetectionData.GroundLayer) > 0;
            IsGrounded = Physics.SphereCastNonAlloc(colliderPosition, manager.Data.GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, GroundHitResults, manager.Data.GroundDetectionData.GroundCheckDistance, manager.Data.GroundDetectionData.GroundLayer) > 0;
            Physics.SphereCastNonAlloc(colliderPosition, manager.Data.GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, SlopesHitResults, manager.Data.GroundDetectionData.SlopesGroundCheckDistance, manager.Data.GroundDetectionData.GroundLayer);
            //bool a = Physics.SphereCastNonAlloc(colliderPositionn, manager.Data.GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, GroundHitResults, manager.Data.GroundDetectionData.GroundCheckDistance, manager.Data.GroundDetectionData.GroundLayer) > 0;
            //Collider[] hitColliders = Physics.OverlapSphere(colliderPosition + Vector3.down * manager.Data.GroundDetectionData.GroundCheckDistance, manager.Data.GroundDetectionData.GroundDetectionSphereRadius, manager.Data.GroundDetectionData.GroundLayer);
            //IsGrounded = hitColliders.Length > 0;
            //IsGrounded = Physics.CheckSphere(colliderPosition + Vector3.down * manager.Data.GroundDetectionData.JumpBufferCheckDistance, manager.Data.GroundDetectionData.GroundDetectionSphereRadius, manager.Data.GroundDetectionData.GroundLayer, QueryTriggerInteraction.Ignore);


            //if (IsGrounded)
            LastGround = SlopesHitResults[0];

            if (lastGrounded != IsGrounded)
            {
                if (IsGrounded) // On Ground Enter
                {
                    EnterGround(manager, colliderPosition);
                }
                else // On Ground Leave
                {
                    if (!isPropulsing)
                    {
                        manager.States.SwitchState(manager.States.FallingState); // IDLE, WALK & FALL
                    }
                    OnGroundExit?.Invoke(manager);
                    manager.CharacterCollider.Rigidbody.transform.SetParent(parent);
                }
            }

            // Others checks to be sure
            if (IsGrounded && manager.States.IsInState(manager.States.FallingState))
            {
                EnterGround(manager, colliderPosition);
            }
        }

        private void EnterGround(CharacterManager manager, Vector3 colliderPosition)
        {
            OnGroundEnter?.Invoke(manager);
            manager.States.SwitchState(manager.States.IdleState);
            manager.CharacterCollider.Rigidbody.transform.SetParent(LastGround.collider.transform);

            // VFX
            Instantiate(manager.Data.FeedbacksData.LandVFX, manager.CharacterCollider.Rigidbody.transform.position, manager.Data.FeedbacksData.LandVFX.transform.rotation);

            // Animation
            manager.AnimationManager.PlayAnimation(manager.AnimationManager.LandingAnim);

            //if (LastGround.collider != null)
            //{
            //    Vector3 downVector = Vector3.down * (LastGround.point.y - colliderPosition.y);
            //    Debug.LogError(downVector);
            //    CurrentVelocity += downVector;
            //}
        }

        public void UpdateGravity(CharacterManager manager, bool isnotGrounded = true)
        {
            if (isnotGrounded)
            {
                manager.MovementManager.CurrentGravityIncreaseByFrame = Mathf.Max(manager.MovementManager.CurrentGravityIncreaseByFrame - manager.MovementManager.CurrentGravityIncreaseDeceleration, 0);
            }
            else
            {
                manager.MovementManager.CurrentGravityIncreaseByFrame = 0;
            }
            manager.MovementManager.CurrentGravity = Mathf.Clamp(manager.MovementManager.CurrentGravity + manager.MovementManager.CurrentGravityIncreaseByFrame, manager.MovementManager.MinGravity, manager.MovementManager.MaxGravity);


            /*Vector3 additionalForces = Vector3.zero;
            foreach (var force in _additionnalForces)
            {
                additionalForces += force.Value.CurrentForce;
                force.Value.CurrentForce = Vector3.Lerp(force.Value.CurrentForce, force.Value.TargetForce, force.Value.ForceLerp);
            }*/
            Vector3 gravity = -manager.MovementManager.CurrentGravity * Vector3.up;
            //if (!isnotGrounded) gravity = Vector3.ProjectOnPlane(gravity, LastGround.normal);
            //Vector3 allForces = CharacterManager.Instance + additionalForces + gravity;

            //_characterController.Move(allForces * Time.deltaTime);

            //if (!(OnSlope() && IsGrounded))
            manager.MovementManager.CurrentVelocity += gravity;
            //CharacterManager.Instance.Force = Vector3.Lerp(CharacterManager.Instance.Force, CharacterManager.Instance.CurrentGravity, _lerpValue);
        }

        public void ResetGravity(CharacterManager manager)
        {
            manager.MovementManager.CurrentGravity = 0;
            manager.MovementManager.CurrentGravityIncreaseByFrame = 0;
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

        public void LerpGravityTo(CharacterManager manager, Data.FallData data)
        {
            if (_currentFallCoroutine != null) StopCoroutine(_currentFallCoroutine);
            SetGravityMinMax(manager, data.BaseGravity, data.MinGravity, data.MaxGravity);
            SetGravityIncrease(manager, data.GravityIncreaseByFrame, data.GravityIncreaseDecelerationByFrame);
            _currentFallCoroutine = StartCoroutine(LerpWithEase(CurrentGravity, data.BaseGravity, data.GravityTime, data.GravityAccel, (result) => CurrentGravity = result));
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
            //if (manager.Inputs.PropulsionType.HasFlag(PropulsionDirection.Up) || manager.Inputs.PropulsionType.HasFlag(PropulsionDirection.Lateral))
            if (manager.Inputs.StartPropulsion)
            {
                manager.AirManager.RefreshAir();
                PropulsionStart(manager);
                LerpPropulsionTakeOffSpeed(manager, manager.Data.PropulsionData.PropulsionTakeOffSpeed, manager.Data.PropulsionData.PropulsionTakeOffAccelTime, manager.Data.PropulsionData.PropulsionTakeOffAccelCurve, 0, manager.Data.PropulsionData.PropulsionTakeOffDecelTime, manager.Data.PropulsionData.PropulsionTakeOffDecelCurve);
            }
        }

        public void CheckForPropulsionStartOnAir(CharacterManager manager)
        {
            if (CanJumpBuffer) //  && !manager.AirManager.AirIsFull
            {
                CheckForJumpStart(manager);
                return;
            }

            if (manager.AirManager.AirIsEmpty) return;

            if (manager.Inputs.StartPropulsion)
            {
                PropulsionStart(manager);
            }
        }

        private void PropulsionStart(CharacterManager manager)
        {
            manager.States.SwitchState(manager.States.PropulsionState);
            CurrentPropulsionIncreaseByFrame = manager.Data.PropulsionData.PropulsionIncreaseByFrame;
            _currentPropulsionDirection = Vector3.zero;
            //_currentPropulsionDirection = _currentDeplacementDirection;
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
                CurrentPropulsionIncreaseByFrame = Math.Max(CurrentPropulsionIncreaseByFrame - manager.Data.PropulsionData.PropulsionIncreaseDeceleration, 0);
                _currentPropulsionSpeed = Math.Min(_currentPropulsionSpeed + (CurrentPropulsionIncreaseByFrame / 100), manager.Data.PropulsionData.MaxPropulsionSpeed);

            }
            else
            {
                propulsionDirection = _currentPropulsionDirection;
            }
            _currentPropulsionDirection = Vector3.Lerp(_currentPropulsionDirection, propulsionDirection, manager.Data.PropulsionData.PropulsionDirectionTurnSpeed);


            float horizontalSpeed = _currentPropulsionSpeed * manager.Data.PropulsionData.HorizontalPropulsionMultiplier;
            float verticalSpeed = _currentPropulsionSpeed * manager.Data.PropulsionData.VerticalPropulsionMultiplier;

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

        public void StartPropulsionTimer(CharacterManager manager)
        {
            PropulsionTimer = manager.Data.PropulsionData.MinimumPropulsionTime;
        }

        public void UpdatePropulsionTimer(CharacterManager manager)
        {
            PropulsionTimer -= Time.deltaTime;
        }

        public void CheckForJacketToggle(CharacterManager manager)
        {
            if (manager.Inputs.JacketInflateToggle)
            {
                IsJacketInflated = !IsJacketInflated;
            }
        }

        public void StartDeriveTimer(CharacterManager manager)
        {
            DeriveTimer = manager.Data.PropulsionData.DeriveTime;
        }

        private void UpdateDeriveTimer(CharacterManager manager)
        {
            DeriveTimer -= Time.deltaTime;
        }
        public void CheckForDeriveEnd(CharacterManager manager)
        {
            if (!manager.Data.AirData.DeriveStartsWhenAirIsEmpty || !manager.AirManager.AirIsEmpty) return;

            UpdateDeriveTimer(manager);

            if (DeriveTimer < 0)
            {
                ToggleJacketInflate(manager, false);
                manager.States.SwitchState(manager.States.FallingState);
            }
        }
        public void CheckForJacketInflated(CharacterManager manager)
        {
            if (IsJacketInflated) manager.States.SwitchState(manager.States.FloatingState);
        }
        public void CheckForJacketDeflated(CharacterManager manager)
        {
            if (!IsJacketInflated) manager.States.SwitchState(manager.States.FallingState);
        }

        public void ToggleJacketInflate(CharacterManager manager, bool value)
        {
            IsJacketInflated = value;
        }

        public void CheckForJumpStart(CharacterManager manager)
        {
            if (manager.Inputs.StartedJumping)
            {
                _currentJumpState = CharacterJumpState.JumpState.ASCENT;
                manager.AirManager.RefreshAir();
                RefreshDashes(manager);
                StartDeriveTimer(manager);
                manager.States.SwitchState(manager.States.JumpState);
            }
        }

        public void StartJump(CharacterManager manager)
        {
            JumpTimer = manager.Data.PropulsionData.MinimumJumpTime;
            JumpPropulsionTimer = manager.Data.PropulsionData.JumpBeforePropulsionTime;
            _currentJumpSpeed = manager.Data.PropulsionData.JumpForce;
            _currentJumpIncreaseByFrame = manager.Data.PropulsionData.JumpDecreaseByFrame;
        }
        public void UpdateJumpTimer(CharacterManager manager)
        {
            JumpTimer -= Time.deltaTime;
            JumpPropulsionTimer -= Time.deltaTime;
        }
        public void UpdateJumpMovement(CharacterManager manager)
        {
            _currentJumpIncreaseByFrame = Math.Max(_currentJumpIncreaseByFrame - manager.Data.PropulsionData.JumpDecreaseDeceleration, 0);
            _currentJumpSpeed = Math.Min(_currentJumpSpeed + (_currentJumpIncreaseByFrame / 100), manager.Data.PropulsionData.MaxPropulsionSpeed);

            Vector3 verticalMovement = Vector3.up * _currentJumpSpeed;

            CurrentVelocity += verticalMovement;
        }

        public void UpdateJumpState(CharacterManager manager)
        {
            if (_currentJumpState == CharacterJumpState.JumpState.DESCENT) return;

            float jumpVelocity = _currentJumpSpeed - CurrentGravity;
            if (jumpVelocity <= 0)
            {
                StartJumpDescent(manager);
            }
        }

        public void CheckIfJumpButtonReleased(CharacterManager manager)
        {
            if (!manager.Data.PropulsionData.VariableJumpHeightBaseOnInput) return;
            if (_currentJumpState == CharacterJumpState.JumpState.DESCENT) return;
            if (manager.Inputs.IsJumping) return;

            StartJumpDescent(manager);
        }

        private void StartJumpDescent(CharacterManager manager)
        {
            _currentJumpState = CharacterJumpState.JumpState.DESCENT;
            CurrentGravityIncreaseByFrame += manager.Data.FallData.JumpDescentData.GravityIncreaseByFrame;
            CurrentGravity += manager.Data.FallData.JumpDescentData.BaseGravity;
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.JumpDescentData);
        }

        public void CheckForDashStart(CharacterManager manager, bool refreshDashes = false)
        {
            if (!manager.Inputs.StartedDash) return;
            if (refreshDashes) RefreshDashes(manager);
            if (CurrentDashes < 1) return;

            manager.States.SwitchState(manager.States.DashState);
        }

        public void StartDash(CharacterManager manager)
        {
            CurrentDashes--;

            _dashTimer = manager.Data.PowerUpData.DashDuration;

            Vector3 lateralMoveInput = manager.Inputs.LastMoveInputDirection != Vector3.zero ? manager.Inputs.LastMoveInputDirection : Vector3.forward;
            Vector3 lateralMoveDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized * lateralMoveInput.z + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * lateralMoveInput.x).normalized;
            _currentDashDirection = lateralMoveDirection;

            manager.Feedbacks.PlayFeedback(manager.Data.FeedbacksData.DashFeedback, manager.CharacterCollider.Rigidbody.transform.position, Quaternion.LookRotation(lateralMoveDirection), null);


            if (manager.Data.PowerUpData.DashEmptiesAir) manager.AirManager.EmptyAir();
        }

        public void UpdateDashTimer(CharacterManager manager)
        {
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0)
            {
                if (IsGrounded) manager.States.SwitchState(manager.States.IdleState);
                else manager.States.SwitchState(manager.States.FloatingState);
            }
        }

        public void UpdateDashMovement(CharacterManager manager)
        {
            float timerPercentage = 1 - (_dashTimer / manager.Data.PowerUpData.DashDuration);

            float interpolation = manager.Data.PowerUpData.DashInterpolationCurve.Evaluate(timerPercentage);
            float horizontalSpeed = Mathf.Lerp(manager.Data.PowerUpData.DashStartSpeed, manager.Data.PowerUpData.DashEndSpeed, interpolation);

            Vector3 dashMovement = new Vector3(_currentDashDirection.x * horizontalSpeed, 0, _currentDashDirection.z * horizontalSpeed);
            CurrentVelocity += dashMovement;
        }

        public void RefreshDashes(CharacterManager manager)
        {
            CurrentDashes = manager.Data.PowerUpData.MaxDashes;
        }

        #endregion

        #region Air
        public void FallIfAirEmpty(CharacterManager manager)
        {
            if (!manager.AirManager.AirIsEmpty) return;

            manager.States.SwitchState(manager.States.FloatingState);
        }
        #endregion

        #region Slopes
        // Slopes

        private void UpdateSlopes(CharacterManager manager)
        {
            if (!IsGrounded) return;

            var height = manager.CharacterCollider.Collider.bounds.center.y;
            var radius = manager.CharacterCollider.Collider.bounds.extents.magnitude;
            //var SphereCastVerticalOffset = height / 2 - radius;
            var castOrigin = manager.CharacterCollider.Rigidbody.position;

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
            //Debug.DrawLine(castOrigin, castOrigin + LastGround.normal, Color.black, 3f);

            Debug.DrawLine(manager.CharacterCollider.Rigidbody.position, manager.CharacterCollider.Rigidbody.position + GetSlopeMoveDirection(_currentDeplacementDirection), Color.black, 3f);
        }

        private bool OnSlope()
        {
            if (SlopesHitResults[0].collider == null) return false;

            float angle = Vector3.Angle(Vector3.up, SlopesHitResults[0].normal);
            //Debug.LogWarning(angle);
            return angle < Manager.Data.GroundDetectionData.MaxSlopeAngle && angle != 0;

        }

        private Vector3 GetSlopeMoveDirection(Vector3 deplacementDirection)
        {
            return Vector3.ProjectOnPlane(deplacementDirection, SlopesHitResults[0].normal).normalized;
        }

        private Vector3 ColliderAndSlide(CharacterManager manager, Vector3 vel, Vector3 pos, int depth, bool gravityPass, Vector3 velInit)
        {
            if (depth >= manager.Data.GroundDetectionData.MaxBounces)
            {
                return Vector3.zero;
            }

            float dist = vel.magnitude + manager.Data.GroundDetectionData.CharacterColliderCheckOffset;

            RaycastHit hit;
            if (Physics.SphereCast(pos, manager.CharacterCollider.Collider.bounds.extents.x, vel.normalized, out hit, dist, manager.Data.GroundDetectionData.GroundLayer))
            {
                Vector3 snapToSurface = vel.normalized * (hit.distance - manager.Data.GroundDetectionData.CharacterColliderCheckOffset);
                Vector3 leftOver = vel - snapToSurface;
                float angle = Vector3.Angle(Vector3.up, hit.normal);

                if (snapToSurface.magnitude <= manager.Data.GroundDetectionData.CharacterColliderCheckOffset)
                {
                    snapToSurface = Vector3.zero;
                }

                // Walkable Ground / Slopes
                if (angle <= manager.Data.GroundDetectionData.MaxSlopeAngle)
                {
                    if (gravityPass)
                    {
                        return snapToSurface;
                    }
                    leftOver = VectorExtensions.ProjectAndScale(leftOver, hit.normal);
                }
                else // Unwalkable Slopes / Wall
                {
                    float scale = 1 -
                        Vector3.Dot(new Vector3(hit.normal.x, 0, hit.normal.z).normalized,
                        -new Vector3(velInit.x, 0, velInit.z).normalized);
                    if (IsGrounded && !gravityPass)
                    {
                        leftOver = VectorExtensions.ProjectAndScale(
                            new Vector3(leftOver.x, 0, leftOver.z),
                            new Vector3(hit.normal.x, 0, hit.normal.z));
                        leftOver *= scale;
                    }
                    else
                    {
                        leftOver = VectorExtensions.ProjectAndScale(leftOver, hit.normal) * scale;
                    }
                }

                return snapToSurface + ColliderAndSlide(manager, leftOver, pos + snapToSurface, ++depth, gravityPass, velInit);
            }

            return vel;
        }


        // REMOVE THIS
        private void OnDrawGizmos()
        {
            if (Manager == null) return;

            Gizmos.color = Color.white;

            if (OnSlope())
            {
                Gizmos.color = Color.red;
                //Debug.Log(LastGround.normal);
            }

            Vector3 direction = GetSlopeMoveDirection(_currentDeplacementDirection);
            Vector3 position = Manager.CharacterCollider.Rigidbody.position;
            Gizmos.DrawLine(position, position + direction);

            //if (Physics.SphereCast(transform.position, 0.5f, -transform.up, out hit, 0.25f))
            //Gizmos.DrawWireSphere(transform.position - transform.up * 0.25f, 0.5f);
            //
            //Physics.SphereCastNonAlloc(manager.CharacterVisual.position, GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, GroundHitResults, GroundDetectionData.GroundCheckDistance, GroundDetectionData.GroundLayer) > 0;

            Vector3 colliderPosition = new Vector3(Manager.CharacterCollider.Collider.bounds.center.x, Manager.CharacterCollider.Collider.bounds.min.y, Manager.CharacterCollider.Collider.bounds.center.z);

            GizmoExtensions.DrawSphereCast(colliderPosition, Manager.Data.GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, Manager.Data.GroundDetectionData.GroundCheckDistance);

            Gizmos.color = Color.blue;

            // Stairs
            Debug.DrawRay(Manager.CharacterCollider.LowerStepRaycast.transform.position, Quaternion.AngleAxis(0, Vector3.up) * Manager.CharacterVisual.transform.forward * Manager.Data.GroundDetectionData.LowerRaycastLength, Color.blue, 0.3f);
            Debug.DrawRay(Manager.CharacterCollider.LowerStepRaycast.transform.position, Quaternion.AngleAxis(45, Vector3.up) * Manager.CharacterVisual.transform.forward * Manager.Data.GroundDetectionData.LowerRaycastLength, Color.blue, 0.3f);
            Debug.DrawRay(Manager.CharacterCollider.LowerStepRaycast.transform.position, Quaternion.AngleAxis(-45, Vector3.up) * Manager.CharacterVisual.transform.forward * Manager.Data.GroundDetectionData.LowerRaycastLength, Color.blue, 0.3f);
            Debug.DrawRay(Manager.CharacterCollider.UpperStepRaycast.transform.position, Quaternion.AngleAxis(0, Vector3.up) * Manager.CharacterVisual.transform.forward * Manager.Data.GroundDetectionData.UpperRaycastLength, Color.blue, 0.3f);
            Debug.DrawRay(Manager.CharacterCollider.UpperStepRaycast.transform.position, Quaternion.AngleAxis(45, Vector3.up) * Manager.CharacterVisual.transform.forward * Manager.Data.GroundDetectionData.UpperRaycastLength, Color.blue, 0.3f);
            Debug.DrawRay(Manager.CharacterCollider.UpperStepRaycast.transform.position, Quaternion.AngleAxis(-45, Vector3.up) * Manager.CharacterVisual.transform.forward * Manager.Data.GroundDetectionData.UpperRaycastLength, Color.blue, 0.3f);
        }

        public void CheckForStepClimb(CharacterManager manager)
        {
            for (int i = -45; i <= 45; i += 45)
            {
                if (Physics.Raycast(manager.CharacterCollider.LowerStepRaycast.transform.position, Quaternion.AngleAxis(0, Vector3.up) * Manager.CharacterVisual.transform.forward, out RaycastHit lowerHit, manager.Data.GroundDetectionData.LowerRaycastLength, manager.Data.GroundDetectionData.GroundLayer))
                {
                    if (!Physics.Raycast(manager.CharacterCollider.UpperStepRaycast.transform.position, Quaternion.AngleAxis(0, Vector3.up) * Manager.CharacterVisual.transform.forward, manager.Data.GroundDetectionData.UpperRaycastLength, manager.Data.GroundDetectionData.GroundLayer))
                    {
                        CurrentVelocity += Vector3.up * manager.Data.GroundDetectionData.StepSmooth * Time.deltaTime;
                        break;
                    }
                }
            }
        }
        #endregion

        #region External Force
        public void AddExternalForce(GameObject go, Vector3 force, float lerp)
        {
            //if (go == gameObject)
            //{
            //    Debug.Log(force + " " + lerp);
            //}
            if (ExternalForces.ContainsKey(go))
            {
                ExternalForces[go].TargetForce = force;
                ExternalForces[go].ForceLerp = lerp;
            }
            else
            {
                ExternalForces.Add(go, new ForceData(force, lerp));
            }
        }

        public void RemoveExternalForce(GameObject go)
        {
            if (ExternalForces.ContainsKey(go))
            {
                ExternalForces.Remove(go);
            }
        }

        private Vector3 GetExternalForces()
        {
            if (ExternalForces.Count <= 0) return Vector3.zero;

            Vector3 totalForce = Vector3.zero;
            foreach (var force in ExternalForces)
            {
                totalForce += force.Value.CurrentForce;
                force.Value.CurrentForce = Vector3.Lerp(force.Value.CurrentForce, force.Value.TargetForce, force.Value.ForceLerp);
            }
            return totalForce;
        }

        public void ResetAllExternalForces()
        {
            ExternalForces.Clear();
        }
        #endregion

        #region Upgrades

        public void CheckForGroundPound(CharacterManager manager)
        {
            if (!manager.Data.PowerUpData.IsGroundPoundAvailable) return;
            if (!manager.Inputs.StartedGroundPound) return;

            manager.States.SwitchState(manager.States.GroundPoundState);
        }

        public void StartGroundPound(CharacterManager manager)
        {
            if (HasBalloonGroundPound) return;
            bool isSmallJump = GroundPoundTotalHeight < manager.Data.PowerUpData.GroundPoundNormalGroundHeightThresold;
            float groundPoundForce = isSmallJump ? manager.Data.PowerUpData.GroundPoundSmallForce : manager.Data.PowerUpData.GroundPoundNormalForce;
            
            AddExternalForce(gameObject, Vector3.up * groundPoundForce, manager.Data.PowerUpData.GroundPoundStartLerp);
        }

        public void EndGroundPound(CharacterManager manager)
        {
            AddExternalForce(gameObject, Vector3.zero, manager.Data.PowerUpData.GroundPoundEndLerp);
        }

        public void StartGroundPoundCoroutine(CharacterManager manager)
        {
            StartCoroutine(WaitBeforeGroundPoundEnd(manager));
        }

        public IEnumerator WaitBeforeGroundPoundEnd(CharacterManager manager)
        {
            GroundPoundTotalHeight -= manager.CharacterCollider.Collider.transform.position.y;
            StartGroundPound(manager);

            yield return new WaitForSeconds(manager.Data.PowerUpData.GroundPoundEndTime);

            EndGroundPound(manager);
        }

        public void UpdateGroundPoundTimer(CharacterManager manager)
        {
            GroundPoundCancelTime -= Time.deltaTime;
        }

        public void GroundPoundOnBalloon(CharacterManager manager)
        {
            AddExternalForce(gameObject, Vector3.up * manager.Data.PowerUpData.GroundPoundBalloonForce, manager.Data.PowerUpData.GroundPoundStartLerp);
            HasBalloonGroundPound = true;
        }
        #endregion

    }

}
