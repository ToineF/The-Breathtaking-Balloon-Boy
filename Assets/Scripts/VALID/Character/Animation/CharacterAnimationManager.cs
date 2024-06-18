using System;
using UnityEngine;

namespace BlownAway.Character.Animations
{
    public class CharacterAnimationManager : CharacterSubComponent
    {

        [Header("References")]
        [SerializeField] private Animator _characterAnimator;
        [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderers;

        [field:Header("Animator Params")]
        [field:SerializeField] public string X { get; private set; }
        [field:SerializeField] public string Y { get; private set; }
        [field:SerializeField] public string Z { get; private set; }
        [field:SerializeField] public string PropulsionAmount { get; private set; }
        [field:SerializeField] public string DeriveAmount { get; private set; }
        [field:SerializeField] public string IsDashing { get; private set; }
        [field:SerializeField] public string IsGroundPounding { get; private set; }
        [field:SerializeField] public string IsJacketInflated { get; private set; }
        [field:SerializeField] public string IsGrounded { get; private set; }
        [field:SerializeField] public string IsPropulsing { get; private set; }
        [field:SerializeField] public string IsJumping { get; private set; }
        [field:SerializeField] public string IsJumpDescending { get; private set; }

        [Header("Parameters")]
        [SerializeField] private bool _isOrientationInverted;
        [SerializeField] private float _lookAtLerp;
        [SerializeField, Range(0,1)] private float _jacketMorpherLerp;

        [Header("Animation Randomness")]
        [SerializeField] private string _startVariation;
        [SerializeField] private float _variationWaitTimeMin;
        [SerializeField] private float _variationWaitTimeMax;

        private Vector3 _lastDirection;
        private float _jacketMorpherWeight;

        private float _variationAnimationTimer;
        private bool _hasResetVariationTimer;


        private void Update()
        {
            Manager.CharacterVisual.transform.position = Manager.CharacterCollider.Rigidbody.transform.position;
            //ChangeCharacterAnimation();
            UpdateCharacterMorpher();
            UpdateAnimationParams(Manager);
            UpdateIdleVariationRandomness();
        }

        private void LateUpdate()
        {
            RotateDirection();
        }

        private void RotateDirection()
        {
            Vector3 moveDirection = Manager.MovementManager.CurrentVelocity;
            moveDirection = Vector3.Scale(moveDirection, new Vector3(1, 0, 1));
            if (moveDirection.sqrMagnitude >= 0.001f) _lastDirection = moveDirection;
            int orientation = _isOrientationInverted ? -1 : 1;

            Vector3 point = Manager.CharacterVisual.position - _lastDirection * orientation;
            //Manager.CharacterVisual.LookAt(point);
            Vector3 direction = point - Manager.CharacterVisual.transform.position;
            if (direction.magnitude < 0.001f) return;
            Quaternion toRotation = Quaternion.LookRotation(direction, Manager.CharacterVisual.transform.up);
            Manager.CharacterVisual.transform.rotation = Quaternion.Lerp(Manager.CharacterVisual.transform.rotation, toRotation, _lookAtLerp * Time.deltaTime);

            //transform.rotation = Quaternion.identity;
            // transform.RotateAround(collider, Vector3.forward, Vector3.Angle(position, position + _lastDirection));
            //Manager.CharacterTransform.RotateAround(collider, Vector3.up, Vector3.Angle(Vector3.zero, Vector3.Scale(collider, new Vector3(0,1,0)) - _lastDirection));

        }

        private void UpdateAnimationParams(CharacterManager manager)
        {
            _characterAnimator.SetFloat(X, manager.MovementManager.CurrentVelocity.x);
            _characterAnimator.SetFloat(Y, manager.MovementManager.CurrentVelocity.y);
            _characterAnimator.SetFloat(Z, manager.MovementManager.CurrentVelocity.z);
            _characterAnimator.SetFloat(PropulsionAmount, manager.AirManager.CurrentAir);
            _characterAnimator.SetFloat(DeriveAmount, manager.MovementManager.NormalizedDeriveAirAmount);
            _characterAnimator.SetBool(IsDashing, manager.States.IsInState(manager.States.DashState));
            _characterAnimator.SetBool(IsGroundPounding, manager.States.IsInState(manager.States.GroundPoundState));
            _characterAnimator.SetBool(IsGrounded, manager.MovementManager.IsMaxGrounded);
            _characterAnimator.SetBool(IsJacketInflated, manager.MovementManager.IsJacketInflated);
            _characterAnimator.SetBool(IsPropulsing, manager.States.IsInState(manager.States.PropulsionState));
            _characterAnimator.SetBool(IsJumping, manager.States.IsInState(manager.States.JumpState));
            _characterAnimator.SetBool(IsJumpDescending, manager.States.IsInState(manager.States.JumpState) && manager.MovementManager.CurrentJumpState == States.CharacterJumpState.JumpState.DESCENT);
        }

        //private void ChangeCharacterAnimation()
        //{
        //    Vector3 moveDirection = Manager.MovementManager.CurrentVelocity;
        //    moveDirection.y = 0;
        //    _characterAnimator.Play((moveDirection.sqrMagnitude >= 0.1f) ? WalkAnim : IdleAnim);
        //}

        private void UpdateCharacterMorpher()
        {
            if (Time.timeScale <= 0) return;
            foreach (SkinnedMeshRenderer morpher in _skinnedMeshRenderers)
            {
                if (morpher.sharedMesh.blendShapeCount < 1) return;
                float weight = Manager.MovementManager.IsJacketInflated ? 100 : 0;
                _jacketMorpherWeight = Mathf.Lerp(_jacketMorpherWeight, weight, _jacketMorpherLerp);
                if (Manager.States.IsInState(Manager.States.FloatingState) && Manager.MovementManager.NormalizedDeriveAirAmount < 1)
                {
                    _jacketMorpherWeight = Manager.MovementManager.NormalizedDeriveAirAmount * 100f;
                }
                morpher.SetBlendShapeWeight(0, _jacketMorpherWeight);
            }
            
        }

        #region Variation
        private void UpdateIdleVariationRandomness() // Animations Randomness
        {
            if (!Manager.States.IsInState(Manager.States.IdleState) && !_hasResetVariationTimer)
            {
                _hasResetVariationTimer = true;
                SetNewVariationTimer();
                return;
            }

            _hasResetVariationTimer = false;
            _variationAnimationTimer -= Time.deltaTime;

            if (_variationAnimationTimer < 0)
            {
                _characterAnimator.SetTrigger(_startVariation);
                SetNewVariationTimer();
            }
        }

        private void SetNewVariationTimer()
        {
            _variationAnimationTimer = UnityEngine.Random.Range(_variationWaitTimeMin, _variationWaitTimeMax);
        }
        #endregion

    }
}