using System;
using UnityEngine;

namespace BlownAway.Character.Animations
{
    public class CharacterAnimationManager : CharacterSubComponent
    {
        [Header("References")]
        [SerializeField] private Animator _characterAnimator;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private string _idleAnimName;
        [SerializeField] private string _walkAnimName;

        [Header("Parameters")]
        [SerializeField] private bool _isOrientationInverted;
        [SerializeField] private Vector3 _offsetFromRigidbody;
        [SerializeField, Range(0,1)] private float _jacketMorpherLerp;

        private Vector3 _lastDirection;
        private float _jacketMorpherWeight;

        private void Update()
        {
            Manager.CharacterVisual.transform.position = Manager.CharacterCollider.Rigidbody.transform.position + _offsetFromRigidbody;
            ChangeCharacterAnimation();
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

            Manager.CharacterVisual.LookAt(Manager.CharacterVisual.position - _lastDirection * orientation);

            //transform.rotation = Quaternion.identity;
            // transform.RotateAround(collider, Vector3.forward, Vector3.Angle(position, position + _lastDirection));
            //Manager.CharacterTransform.RotateAround(collider, Vector3.up, Vector3.Angle(Vector3.zero, Vector3.Scale(collider, new Vector3(0,1,0)) - _lastDirection));

        }

        private void ChangeCharacterAnimation()
        {
            Vector3 moveDirection = Manager.MovementManager.CurrentVelocity;
            moveDirection.y = 0;
            _characterAnimator.Play((moveDirection.sqrMagnitude >= 0.1f) ? _walkAnimName : _idleAnimName);
        }

        private void UpdateCharacterMorpher()
        {
            if (_skinnedMeshRenderer.sharedMesh.blendShapeCount < 1) return;
            float weight = Manager.MovementManager.IsJacketInflated ? 100 : 0;
            _jacketMorpherWeight = Mathf.Lerp(_jacketMorpherWeight, weight, _jacketMorpherLerp);
            _skinnedMeshRenderer.SetBlendShapeWeight(0, _jacketMorpherWeight);
        }

    }
}