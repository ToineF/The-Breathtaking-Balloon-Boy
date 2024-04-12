using System;
using UnityEngine;

namespace BlownAway.Character.Animations
{
    public class CharacterAnimationManager : CharacterSubComponent
    {
        public string IdleAnim => _idleAnimName;
        public string WalkAnim => _walkAnimName;
        public string JumpAnim => _jumpAnimName;
        public string FallAnim => _fallAnimName;
        public string LandingAnim => _landingAnimName;
        public string PropulsionAnim => _propulsionAnimName;
        public string FloatAnim => _floatAnimName;
        public string DashAnim => _dashAnimName;

        [Header("References")]
        [SerializeField] private Animator _characterAnimator;
        [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderers;
        [SerializeField] private string _idleAnimName;
        [SerializeField] private string _walkAnimName;
        [SerializeField] private string _jumpAnimName;
        [SerializeField] private string _fallAnimName;
        [SerializeField] private string _landingAnimName;
        [SerializeField] private string _propulsionAnimName;
        [SerializeField] private string _floatAnimName;
        [SerializeField] private string _dashAnimName;

        [Header("Parameters")]
        [SerializeField] private bool _isOrientationInverted;
        [SerializeField] private float _lookAtLerp;
        [SerializeField, Range(0,1)] private float _jacketMorpherLerp;

        private Vector3 _lastDirection;
        private float _jacketMorpherWeight;


        private void Update()
        {
            Manager.CharacterVisual.transform.position = Manager.CharacterCollider.Rigidbody.transform.position;
            //ChangeCharacterAnimation();
            UpdateCharacterMorpher();
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

        public void PlayAnimation(string animation)
        {
            _characterAnimator.SetTrigger(animation);
        }

        //private void ChangeCharacterAnimation()
        //{
        //    Vector3 moveDirection = Manager.MovementManager.CurrentVelocity;
        //    moveDirection.y = 0;
        //    _characterAnimator.Play((moveDirection.sqrMagnitude >= 0.1f) ? WalkAnim : IdleAnim);
        //}

        private void UpdateCharacterMorpher()
        {
            foreach (SkinnedMeshRenderer morpher in _skinnedMeshRenderers)
            {
                if (morpher.sharedMesh.blendShapeCount < 1) return;
                float weight = Manager.MovementManager.IsJacketInflated ? 100 : 0;
                _jacketMorpherWeight = Mathf.Lerp(_jacketMorpherWeight, weight, _jacketMorpherLerp);
                morpher.SetBlendShapeWeight(0, _jacketMorpherWeight);
            }
            
        }

    }
}