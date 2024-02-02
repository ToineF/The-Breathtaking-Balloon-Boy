using UnityEngine;
using UnityEngine.EventSystems;

namespace BlownAway.Character.Animations
{
    public class AnimationManager : MonoBehaviour
    {
        public void RotateTowards(Vector3 target)
        {
            GameManager.Instance.CharacterManager.CharacterTransform.LookAt(target);
        }

        private void Update()
        {
            RotateDirection();
        }

        private static void RotateDirection()
        {
            Vector3 moveDirection = GameManager.Instance.CharacterManager.MovementManager.CurrentVelocity;
            moveDirection = Vector3.Scale(moveDirection, new Vector3(1, 0, 1));
            GameManager.Instance.CharacterManager.CharacterTransform.LookAt(GameManager.Instance.CharacterManager.CharacterTransform.position - moveDirection);
        }
    }
}