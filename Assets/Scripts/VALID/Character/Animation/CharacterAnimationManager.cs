using UnityEngine;

namespace BlownAway.Character.Animations
{
    public class CharacterAnimationManager : CharacterSubComponent
    {

        [SerializeField] private bool _isOrientationInverted;
        [SerializeField] private Vector3 _offsetFromRigidbody;
        private Vector3 _lastDirection;

        private void Update()
        {
            Manager.CharacterVisual.transform.position = Manager.CharacterCollider.Rigidbody.transform.position + _offsetFromRigidbody;
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
    }
}