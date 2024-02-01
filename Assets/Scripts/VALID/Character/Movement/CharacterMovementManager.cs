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

        // Propulsion Data
        [field: SerializeField] public CharacterPropulsionData PropulsionData { get; private set; }


        // Fall Data
        [field: SerializeField, Tooltip("The lateral speed the character moves at while falling")] public float FallDeplacementSpeed { get; set; }
        [field: SerializeField, Tooltip("The lateral speed the character moves at while floating")] public float FloatDeplacementSpeed { get; set; }
        [Tooltip("The current global velocity of the character (movements, gravity, forces...)")] public Vector3 CurrentVelocity { get; private set; }

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


        [field:Header("Propulsion")]
        [field: SerializeField, Tooltip("The base speed the character moves at while propulsing")] public float BasePropulsionSpeed { get; set; }


        // Lateral Inputs (Idle, Walk, WASD) - Have this as a generic version for other movements
        private float _currentDeplacementSpeed;
        private Vector3 _currentDeplacementDirection;
        private Coroutine _currentDeplacementCoroutine;

        // Propulsion Inputs (Propulsion) - Have this as a generic version for other movements
        private float _currentPropulsionSpeed;
        private Vector3 _currentPropulsionDirection;
        private Coroutine _currentPropulsionCoroutine;

        private float _currentPropulsionTakeOffSpeed;
        private Coroutine _currentPropulsionTakeOffCoroutine;



        /*
        [Header("Forces")]
        [ReadOnly] public Vector3 Force;
        [ReadOnly] public Vector3 CurrentForce;*/


        private void Start()
        {
            GroundHitResults = new RaycastHit[2];
            SetGravityTo(GameManager.Instance.CharacterManager, BaseGravity, BaseMaxGravity);
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

            CurrentVelocity += _currentDeplacementDirection* _currentDeplacementSpeed * Time.deltaTime;
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
                Debug.Log(time / targetTime);
                value = Mathf.Lerp(value, targetValue, weight);
                updateAction?.Invoke(value);
                yield return null;
            }

            if (endCoroutine != null) StartCoroutine(endCoroutine);
        }

        private IEnumerator LerpWithEaseBack(float value, float targetValue, float inTime, float outTime, AnimationCurve inCurve, AnimationCurve outCurve, Action<float> updateAction, IEnumerator endCoroutine = null)
        {
            StartCoroutine(LerpWithEase(value, targetValue, inTime, inCurve, updateAction, endCoroutine));

            yield return new WaitForSeconds(inTime);

            StartCoroutine(LerpWithEase(value, value, outTime, outCurve, updateAction, endCoroutine));
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
        public void LerpPropulsionSpeed(CharacterManager manager, float targetValue, float lerpSpeed, AnimationCurve curve)
        {
            if (_currentPropulsionCoroutine != null) StopCoroutine(_currentPropulsionCoroutine);
            _currentPropulsionCoroutine = StartCoroutine(LerpWithEase(_currentPropulsionSpeed, targetValue, lerpSpeed, curve, (result) => _currentPropulsionSpeed = result));
        }

        public void LerpPropulsionTakeOffSpeed(CharacterManager manager, float startTargetValue, float startLerpSpeed, AnimationCurve startCurve, float endTargetValue, float endLerpSpeed, AnimationCurve endCurve)
        {
            if (_currentPropulsionTakeOffCoroutine != null) StopCoroutine(_currentPropulsionTakeOffCoroutine);
            //IEnumerator endCoroutine = LerpWithEase(_currentPropulsionTakeOffSpeed, endTargetValue, endLerpSpeed, endCurve, (result) => _currentPropulsionTakeOffSpeed = result);
            //IEnumerator startCoroutine = LerpWithEase(_currentPropulsionTakeOffSpeed, startTargetValue, startLerpSpeed, startCurve, (result) => _currentPropulsionTakeOffSpeed = result, endCoroutine);


            _currentPropulsionTakeOffCoroutine = StartCoroutine(LerpWithEaseBack(_currentPropulsionTakeOffSpeed, startTargetValue, startLerpSpeed, endLerpSpeed, startCurve, endCurve, (result) => _currentPropulsionTakeOffSpeed = result));
            // Start two coroutines
            //_currentPropulsionTakeOffCoroutine = StartCoroutine(startCoroutine);
            //_currentPropulsionTakeOffCoroutine = StartCoroutine(WaitForAction(2, () => ));
            // manager.MovementManager.LerpPropulsionTakeOffSpeed(manager, 0, manager.MovementManager.PropulsionData.PropulsionTakeOffDecelTime, manager.MovementManager.PropulsionData.PropulsionTakeOffDecelCurve);

        }

        public void CheckForPropulsionStartOnGround(CharacterManager manager)
        {
            if (manager.Inputs.PropulsionType.HasFlag(PropulsionDirection.Up) || manager.Inputs.PropulsionType.HasFlag(PropulsionDirection.Lateral))
            {
                 PropulsionStart(manager);
                 LerpPropulsionTakeOffSpeed(manager, PropulsionData.PropulsionTakeOffSpeed, PropulsionData.PropulsionTakeOffAccelTime, PropulsionData.PropulsionTakeOffAccelCurve, 0, PropulsionData.PropulsionTakeOffDecelTime, PropulsionData.PropulsionTakeOffDecelCurve);
            }
        }

        public void CheckForPropulsionStartOnAir(CharacterManager manager)
        {
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
            CurrentVelocity += propulsionMovement;
        }

        public void UpdateTakeOffPropulsionMovement(CharacterManager manager)
        {
            CurrentVelocity += Vector3.up * _currentPropulsionTakeOffSpeed;
        }

        public void FallfAirEmpty(CharacterManager manager)
        {
            if (!manager.AirManager.AirIsEmpty) return;

            manager.States.SwitchState(manager.States.FallingState);
        }


        private IEnumerator WaitForAction(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }

    }

}
