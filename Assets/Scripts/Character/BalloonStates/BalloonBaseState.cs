using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public abstract class BalloonBaseState
{
    public abstract void OnActionPressed(InputAction.CallbackContext context);

    public abstract void StartState(BalloonStateManager balloonStateManager);
    public abstract void UpdateState(BalloonStateManager balloonStateManager);
}
