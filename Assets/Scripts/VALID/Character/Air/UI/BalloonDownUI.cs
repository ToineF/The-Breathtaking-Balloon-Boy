using BlownAway.Character.Movements;
using UnityEngine;

namespace BlownAway.Character.Air
{
    public class BalloonDownUI : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private CharacterMovementManager MovementManager;
        [SerializeField] private CanvasGroup UI;

        void Update()
        {
            UI.alpha = MovementManager.Manager.MovementManager.IsAboveBalloon ? 1f : 0f;
        }
    }
}