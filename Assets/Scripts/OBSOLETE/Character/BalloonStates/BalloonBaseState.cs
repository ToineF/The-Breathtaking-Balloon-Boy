using UnityEngine.InputSystem;

namespace BlownAway.Player.States
{
    public abstract class BalloonBaseState
    {
        public abstract void OnActionPressed(InputAction.CallbackContext context);
        public abstract void OnSecondaryActionPressed(InputAction.CallbackContext context);

        public abstract void StartState(BalloonStateManager balloonStateManager);
        public abstract void UpdateState(BalloonStateManager balloonStateManager);
    }
}
