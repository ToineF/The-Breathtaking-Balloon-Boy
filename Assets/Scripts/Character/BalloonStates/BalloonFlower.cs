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
        if (_balloonStageManager.FlowerJumps <= 0) return;

        _balloonStageManager.CharaController.OnGroundEnter += PlayerEnterGround;
        if (!CharacterControllerTest.Instance.IsGrounded)
            _balloonStageManager.FlowerJumps--;

        Vector3 hit = Vector3.up * _balloonStageManager.FlowerJumpForce;
        float startLerp = _balloonStageManager.FlowerJumpAccel;
        float endLerp = _balloonStageManager.FlowerJumpDecel;
        float time = _balloonStageManager.FlowerJumpTime;
        _balloonStageManager.CharaController.SetForceForTime(hit, time, startLerp, endLerp);

        Vector3 glideForce = Vector3.up * _balloonStageManager.FlowerGlideForce;
        float glideAccel = _balloonStageManager.FlowerGlideAccel;
        _balloonStageManager.CharaController.AddForce(glideForce, glideAccel);

        Collider collider = _balloonStageManager.CharaController.GetComponent<Collider>();
        GameObject.Instantiate(_balloonStageManager.FlowerJumpFXPrefab, collider.bounds.center - collider.bounds.extents.y * Vector3.up, _balloonStageManager.FlowerJumpFXPrefab.transform.rotation);
    }

    public override void OnSecondaryActionPressed(InputAction.CallbackContext context)
    {

    }

    private void PlayerEnterGround()
    {
        _balloonStageManager.FlowerJumps = _balloonStageManager.FlowerMaxJumps;
        _balloonStageManager.CharaController.OnGroundEnter -= PlayerEnterGround;

        if (_balloonStageManager.GetState() != _balloonStageManager.BalloonFlower) return;
        _balloonStageManager.CharaController.SetForce(Vector3.zero, 1);
    }
}
