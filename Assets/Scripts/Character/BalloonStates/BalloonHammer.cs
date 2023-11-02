using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BalloonHammer : BalloonBaseState
{
    private BalloonStateManager _balloonStageManager;

    public override void StartState(BalloonStateManager balloonStateManager)
    {
        _balloonStageManager = balloonStateManager;
        _balloonStageManager.CharaController.OnGroundEnter += PlayerEnterGround;
    }

    public override void UpdateState(BalloonStateManager balloonStateManager)
    {
        // UNSUBSCRIBE WHEN STATE CHANGE
        // _balloonStageManager.CharaController.OnGroundEnter -= PlayerEnterGround;
    }

    public override void OnActionPressed(InputAction.CallbackContext context)
    {
        _balloonStageManager.Animator.SetTrigger(_balloonStageManager.AnimatorHammerTrigger);

        if (_balloonStageManager.CharaController.IsGrounded)
        {
            Vector3 hit = Vector3.up * _balloonStageManager.HammerGroundForce;
            float startLerp = _balloonStageManager.HammerGroundAccel;
            float endLerp = _balloonStageManager.HammerGroundDecel;
            float time = _balloonStageManager.HammerGroundJumpTime;
            _balloonStageManager.CharaController.SetForceForTime(hit, time, startLerp, endLerp);
            CreateHammerFX();
        }
        else
        {
            if (!_balloonStageManager.IsHammerFalling)
            {
                _balloonStageManager.IsHammerFalling = true;
                Vector3 hit = Vector3.up * _balloonStageManager.HammerAirFallForce;
                float startLerp = _balloonStageManager.HammerAirFallAccel;
                _balloonStageManager.CharaController.SetForce(hit, startLerp);
            }
        }

    }

    private void PlayerEnterGround()
    {
        if (!_balloonStageManager.IsHammerFalling) return;

        _balloonStageManager.IsHammerFalling = false;

        Vector3 hit = Vector3.up * _balloonStageManager.HammerAirJumpForce;
        float startLerp = _balloonStageManager.HammerAirJumpAccel;
        float endLerp = _balloonStageManager.HammerAirJumpDecel;
        float time = _balloonStageManager.HammerAirJumpTime;
        _balloonStageManager.CharaController.SetForceForTime(hit, time, startLerp, endLerp);
        CreateHammerFX();
    }

    private void CreateHammerFX()
    {
        Collider collider = _balloonStageManager.CharaController.GetComponent<Collider>();
        GameObject.Instantiate(_balloonStageManager.HammerFXGroundPrefab, collider.bounds.center - collider.bounds.extents.y * Vector3.up, _balloonStageManager.HammerFXGroundPrefab.transform.rotation);
    }
}
