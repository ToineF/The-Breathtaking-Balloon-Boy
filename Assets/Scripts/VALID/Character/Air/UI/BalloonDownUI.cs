using BlownAway.Character.Movements;
using UnityEngine;

namespace BlownAway.Character.Air
{
    public class BalloonDownUI : MonoBehaviour
    {
        public float Alpha
        {
            set
            {
                if (_alpha != value) Appear(value);
                _alpha = value;
                UI.alpha = value;
            }
        }

        [Header("Reference")]
        [SerializeField] private CharacterMovementManager _movementManager;
        [SerializeField] private CanvasGroup UI;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _appearName;
        [SerializeField] private string _disappearName;

        private float _alpha;


        private void Appear(float alpha)
        {
            Debug.Log(alpha < 0.5f ? _disappearName : _appearName);
            _animator.SetTrigger(alpha < 0.5f ? _disappearName : _appearName);
        }

        void Update()
        {
            Debug.Log(_movementManager.Manager.MovementManager.IsAboveBalloon ? 1f : 0f);
            Alpha = _movementManager.Manager.MovementManager.IsAboveBalloon ? 1f : 0f;
        }
    }
}