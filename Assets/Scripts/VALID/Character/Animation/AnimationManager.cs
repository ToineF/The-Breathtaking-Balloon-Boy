using UnityEngine;

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
            // here process animations
            GameManager.Instance.CharacterManager.CharacterTransform.LookAt(GameManager.Instance.CharacterManager.CharacterTransform.position - GameManager.Instance.CharacterManager.MovementManager.CurrentVelocity);
        }
    }
}