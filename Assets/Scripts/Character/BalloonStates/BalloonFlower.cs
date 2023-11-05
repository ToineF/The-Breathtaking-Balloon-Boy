using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BalloonFlower : BalloonBaseState
{
    private BalloonStateManager _balloonStageManager;

    public override void StartState(BalloonStateManager balloonStateManager)
    {
        _balloonStageManager = balloonStateManager;
        _balloonStageManager.FlowerModelisation.SetActive(true);
    }

    public override void UpdateState(BalloonStateManager balloonStateManager)
    {

    }

    public override void OnActionPressed(InputAction.CallbackContext context)
    {
        Vector3 hit = Vector3.up * _balloonStageManager.FlowerJumpForce;
        float startLerp = _balloonStageManager.FlowerJumpAccel;
        float endLerp = _balloonStageManager.FlowerJumpDecel;
        float time = _balloonStageManager.FlowerJumpTime;
        _balloonStageManager.CharaController.SetForceForTime(hit, time, startLerp, endLerp);
    }

    public override void OnSecondaryActionPressed(InputAction.CallbackContext context)
    {

    }
}
