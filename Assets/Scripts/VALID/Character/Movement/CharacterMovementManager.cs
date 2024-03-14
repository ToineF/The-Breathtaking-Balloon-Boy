using System;
using UnityEngine;
using System.Collections;
using BlownAway.Character.Inputs;
using AntoineFoucault.Utilities;
using System.Collections.Generic;
using BlownAway.GPE;

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
        private Coroutine _currentDeplacementCoroutine;

        // Fall
        public float CurrentGravity { get; private set; }
        public float MinGravity { get; private set; }
        public float MaxGravity { get; private set; }
        public float CurrentGravityIncreaseByFrame { get; private set; }
        public float CurrentGravityIncreaseDeceleration { get; private set; }

        private Coroutine _currentFallCoroutine;

        // Propulsion Inputs (Propulsion) - Have this as a generic version for other movements
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
        private float _currentJumpSpeed;
        private float _currentJumpIncreaseByFrame;

        // Dash
        private float _dashTimer;
        private Vector3 _currentDashDirection;
        private float _currentDashes;



        // Ground Detection
        [Tooltip("The raycast hits stocked while looking for ground")] public RaycastHit[] GroundHitResults { get; private set; }
        public bool IsGrounded { get; private set; }
        public RaycastHit LastGround { get; private set; }


        // Jump buffer
        public bool CanJumpBuffer { get; private set; }
        [Tooltip("The raycast hits stocked while looking for jump buffer")] public RaycastHit[] JumpBufferHitResults { get; private set; }


        private Transform parent;

        // External Forces
        public Dictionary<GameObject, ForceData> ExternalForces { get; private set; } = new Dictionary<GameObject, ForceData>();

        protected override void StartScript(CharacterManager manager)
        {
            parent = manager.CharacterCollider.Rigidbody.transform.parent;
            GroundHitResults = new RaycastHit[2];
            JumpBufferHitResults = new RaycastHit[2];
            SetGravityTo(manager, manager.Data.FallData.BaseGravity, manager.Data.FallData.BaseMinGravity, manager.Data.FallData.BaseMaxGravity, manager.Data.FallData.BaseGravityIncreaseByFrame, manager.Data.FallData.BaseGravityIncreaseDecelerationByFrame);

            // On Ground Enter Subscriptions
            OnGroundEnter += RefreshDashes;
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
        public void MoveAtSpeed(CharacterManager manager, float walkTurnSpeed, bool includesInputs = true)
        {
            Vector3 deplacementDirection = _currentDeplacementDirection;
            Vector3 groundDirection = new Vector3(1, 0, 1);
            if (includesInputs) // Updates the Current Deplacement Value
            {
                deplacementDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)) * manager.Inputs.MoveInputDirection.z + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * manager.Inputs.MoveInputDirection.x).normalized;

                deplacementDirection = Vector3.Scale(deplacementDirection, new Vector3(1, 0, 1));

                //if (Vector3.Angle(_currentDeplacementDirection, deplacementDirection) > 170f) Debug.LogWarning("turn");
                //Debug.LogWarning(Vector3.Angle(_currentDeplacementDirection, deplacementDirection));
                //if (Vector3.Angle(_currentDeplacementDirection, deplacementDirection) > 170f) 
                //    deplacementDirection = Quaternion.AngleAxis(-70, Vector3.up) * deplacementDirection;

                //Debug.LogWarning(deplacementDirection);
            }

            deplacementDirection = GetSlopeMoveDirection(deplacementDirection);
            Debug.LogWarning(deplacementDirection);


            _currentDeplacementDirection = Vector3.Lerp(_currentDeplacementDirection, deplacementDirection, walkTurnSpeed);

            //if (IsGrounded && OnSlope())
            //     _currentDeplacementDirection = deplacementDirection; // HERE SEE SLOPES

            //Debug.LogWarning(_currentDeplacementDirection * _currentDeplacementSpeed);



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
            Vector3 colliderPosition = new Vector3(manager.CharacterCollider.Collider.bounds.center.x, manager.CharacterCollider.Collider.bounds.min.y, manager.CharacterCollider.Collider.bounds.center.z);
            CanJumpBuffer = Physics.SphereCastNonAlloc(colliderPosition, manager.Data.GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, JumpBufferHitResults, manager.Data.GroundDetectionData.JumpBufferCheckDistance, manager.Data.GroundDetectionData.GroundLayer) > 0;
            IsGrounded = Physics.SphereCastNonAlloc(colliderPosition, manager.Data.GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, GroundHitResults, manager.Data.GroundDetectionData.GroundCheckDistance, manager.Data.GroundDetectionData.GroundLayer) > 0;
            //bool a = Physics.SphereCastNonAlloc(colliderPositionn, manager.Data.GroundDetectionData.GroundDetectionSphereRadius, Vector3.down, GroundHitResults, manager.Data.GroundDetectionData.GroundCheckDistance, manager.Data.GroundDetectionData.GroundLayer) > 0;
            //Collider[] hitColliders = Physics.OverlapSphere(colliderPosition + Vector3.down * manager.Data.GroundDetectionData.GroundCheckDistance, manager.Data.GroundDetectionData.GroundDetectionSphereRadius, manager.Data.GroundDetectionData.GroundLayer);
            //IsGrounded = hitColliders.Length > 0;
            //IsGrounded = Physics.CheckSphere(colliderPosition + Vector3.down * manager.Data.GroundDetectionData.JumpBufferCheckDistance, manager.Data.GroundDetectionData.GroundDetectionSphereRadius, manager.Data.GroundDetectionData.GroundLayer, QueryTriggerInteraction.Ignore);


            //if (IsGrounded)
            LastGround = GroundHitResults[0];

            if (lastGrounded != IsGrounded)
            {
                if (IsGrounded) // On Ground Enter
                {
                    OnGroundEnter?.Invoke(manager);
                    manager.States.SwitchState(manager.States.IdleState);
                    manager.CharacterCollider.Rigidbody.transform.SetParent(LastGround.collider.transform);
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
                LerpPropulsionTakeOffSpeed(manager, manager.Data.PropulsionData.PropulsionTakeOffSpeed, manager.Data.PropulsionData.PropulsionTakeOffAccelTime, manager.Data.PropulsionData.PropulsionTakeOffAccelCurve, 0, manager.Data.PropulsionData.PropulsionTakeOffDecelTime, manager.Data.PropulsionData.PropulsionTakeOffDecelCurve);
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

        public void CheckForJacketInflated(CharacterManager manager)
        {
            if (manager.Inputs.IsJacketInflated)
            {
                manager.States.SwitchState(manager.States.FloatingState);
            }
        }

        public void CheckForJacketDeflated(CharacterManager manager)
        {
            if (!manager.Inputs.IsJacketInflated)
            {
                manager.States.SwitchState(manager.States.FallingState);
            }
        }

        public void StartJump(CharacterManager manager)
        {
            JumpTimer = manager.Data.PropulsionData.MinimumJumpTime;
            _currentJumpSpeed = manager.Data.PropulsionData.JumpForce;
            _currentJumpIncreaseByFrame = manager.Data.PropulsionData.JumpDecreaseByFrame;
        }
        public void UpdateJumpTimer(CharacterManager manager)
        {
            JumpTimer -= Time.deltaTime;
        }
        public void UpdateJumpMovement(CharacterManager manager)
        {
            _currentJumpIncreaseByFrame = Math.Max(_currentJumpIncreaseByFrame - manager.Data.PropulsionData.JumpDecreaseDeceleration, 0);
            _currentJumpSpeed = Math.Min(_currentJumpSpeed + (_currentJumpIncreaseByFrame / 100), manager.Data.PropulsionData.MaxPropulsionSpeed);

            Vector3 verticalMovement = Vector3.up * _currentJumpSpeed;

            CurrentVelocity += verticalMovement;
        }

        public void CheckForDashStart(CharacterManager manager, bool refreshDashes = false)
        {
            if (!manager.Inputs.StartedDash) return;
            if (refreshDashes) RefreshDashes(manager);
            if (_currentDashes < 1) return;

            manager.States.SwitchState(manager.States.DashState);
        }

        public void StartDash(CharacterManager manager)
        {
            _currentDashes--;

            _dashTimer = manager.Data.PowerUpData.DashDuration;

            Vector3 lateralMoveInput = manager.Inputs.LastMoveInputDirection != Vector3.zero ? manager.Inputs.LastMoveInputDirection : Vector3.forward;
            Vector3 lateralMoveDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized * lateralMoveInput.z + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * lateralMoveInput.x).normalized;
            _currentDashDirection = lateralMoveDirection;

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
            _currentDashes = manager.Data.PowerUpData.MaxDashes;
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
            if (LastGround.collider == null) return false;

            float angle = Vector3.Angle(Vector3.up, LastGround.normal);
            //Debug.LogWarning(angle);
            return angle < Manager.Data.SlopeData.MaxSlopeAngle && angle != 0;

        }

        private Vector3 GetSlopeMoveDirection(Vector3 deplacementDirection)
        {
            return Vector3.ProjectOnPlane(deplacementDirection, LastGround.normal).normalized;
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
        }
        #endregion

        #region External Force
        public void AddExternalForce(GameObject go, Vector3 force, float lerp)
        {
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
        #endregion

        #region Upgrades
        public void CheckForBalloonBounce(CharacterManager manager)
        {
            if (!manager.Data.PowerUpData.IsBalloonBounceAvailable) return;
            if (!manager.Inputs.StartedBalloonBounce) return;

            Debug.Log("Balloon Bounce");
        }

        public void CheckForGroundPound(CharacterManager manager)
        {
            if (!manager.Data.PowerUpData.IsGroundPoundAvailable) return;
            if (!manager.Inputs.StartedGroundPound) return;

            manager.States.SwitchState(manager.States.GroundPoundState);
        }

        public void CheckForGroundPoundStart(CharacterManager manager)
        {
            AddExternalForce(gameObject, Vector3.up * manager.Data.PowerUpData.GroundPoundForce, manager.Data.PowerUpData.GroundPoundStartLerp);
        }
        public void CheckForGroundPoundEnd(CharacterManager manager)
        {
            AddExternalForce(gameObject, Vector3.zero, manager.Data.PowerUpData.GroundPoundEndLerp);
            //manager.States.SwitchState(manager.States.PropulsionState);
        }

        #endregion

    }

}
