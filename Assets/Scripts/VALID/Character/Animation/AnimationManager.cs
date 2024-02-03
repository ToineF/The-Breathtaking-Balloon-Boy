using UnityEngine;

namespace BlownAway.Character.Animations
{
    public class AnimationManager : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private Vector3 _lastDirection;

        public void RotateTowards(Vector3 target)
        {
            GameManager.Instance.CharacterManager.CharacterTransform.LookAt(target);
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

            GameManager.Instance.CharacterManager.CharacterTransform.LookAt(GameManager.Instance.CharacterManager.CharacterTransform.position - _lastDirection);

            //transform.rotation = Quaternion.identity;
            // transform.RotateAround(collider, Vector3.forward, Vector3.Angle(position, position + _lastDirection));
            //GameManager.Instance.CharacterManager.CharacterTransform.RotateAround(collider, Vector3.up, Vector3.Angle(Vector3.zero, Vector3.Scale(collider, new Vector3(0,1,0)) - _lastDirection));

        }
    }
}