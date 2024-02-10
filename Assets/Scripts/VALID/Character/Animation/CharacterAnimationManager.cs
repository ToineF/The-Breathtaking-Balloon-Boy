using UnityEngine;

namespace BlownAway.Character.Animations
{
    public class CharacterAnimationManager : MonoBehaviour
    {
        [SerializeField] private bool _isOrientationInverted;
        [SerializeField] private Vector3 _offsetFromRigidbody;
        private Vector3 _lastDirection;

        private CharacterManager _manager;

        private void Start()
        {
            _manager = GameManager.Instance.CharacterManager;
        }

        public void RotateTowards(Vector3 target)
        {
            GameManager.Instance.CharacterManager.CharacterVisual.LookAt(target);
        }

        private void Update()
        {
            _manager.CharacterVisual.transform.position = _manager.CharacterRigidbody.transform.position + _offsetFromRigidbody;
        }

        private void LateUpdate()
        {
            RotateDirection();
        }

        private void RotateDirection()
        {
            Vector3 moveDirection = GameManager.Instance.CharacterManager.MovementManager.CurrentVelocity;
            moveDirection = Vector3.Scale(moveDirection, new Vector3(1, 0, 1));
            if (moveDirection != Vector3.zero) _lastDirection = moveDirection;
            int orientation = _isOrientationInverted ? -1 : 1;

            GameManager.Instance.CharacterManager.CharacterVisual.LookAt(GameManager.Instance.CharacterManager.CharacterVisual.position - _lastDirection * orientation);

            //transform.rotation = Quaternion.identity;
            // transform.RotateAround(collider, Vector3.forward, Vector3.Angle(position, position + _lastDirection));
            //GameManager.Instance.CharacterManager.CharacterTransform.RotateAround(collider, Vector3.up, Vector3.Angle(Vector3.zero, Vector3.Scale(collider, new Vector3(0,1,0)) - _lastDirection));

        }
    }
}