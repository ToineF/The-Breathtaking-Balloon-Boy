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
        _balloonStageManager.HammerModelisation.SetActive(true);
    }

    public override void UpdateState(BalloonStateManager balloonStateManager)
    {
        // UNSUBSCRIBE WHEN STATE CHANGE
        // _balloonStageManager.CharaController.OnGroundEnter -= PlayerEnterGround;
    }

    public override void OnActionPressed(InputAction.CallbackContext context)
    {
        _balloonStageManager.CharaController.OnGroundEnter += PlayerEnterGround;
        _balloonStageManager.Animator.SetTrigger(_balloonStageManager.AnimatorHammerTrigger);

        if (_balloonStageManager.CharaController.IsGrounded)
        {
            float gravityAccel = CharacterControllerTest.Instance.CurrentGravity * _balloonStageManager.HammerGravityAccel;
            Vector3 hit = Vector3.up * _balloonStageManager.HammerGroundForce * gravityAccel;
            float startLerp = _balloonStageManager.HammerGroundAccel;
            float endLerp = _balloonStageManager.HammerGroundDecel;
            float time = _balloonStageManager.HammerGroundJumpTime;
            _balloonStageManager.CharaController.SetForceForTime(hit, time, startLerp, endLerp);
            HitGround();
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
        _balloonStageManager.CharaController.OnGroundEnter -= PlayerEnterGround;
        if (_balloonStageManager.GetState() != _balloonStageManager.BalloonHammer) return;

        if (!_balloonStageManager.IsHammerFalling) return;

        _balloonStageManager.IsHammerFalling = false;

        float gravityAccel = CharacterControllerTest.Instance.CurrentGravity * _balloonStageManager.HammerGravityAccel;
        Vector3 hit = Vector3.up * _balloonStageManager.HammerAirJumpForce * gravityAccel;
        float startLerp = _balloonStageManager.HammerAirJumpAccel;
        float endLerp = _balloonStageManager.HammerAirJumpDecel;
        float time = _balloonStageManager.HammerAirJumpTime;
        _balloonStageManager.CharaController.SetForceForTime(hit, time, startLerp, endLerp);
        HitGround();
    }

    private void HitGround()
    {
        AirManager.Instance.AddAir(-_balloonStageManager.HammerAirPercentageUsed);

        Collider collider = _balloonStageManager.CharaController.GetComponent<Collider>();
        GameObject.Instantiate(_balloonStageManager.HammerFXGroundPrefab, collider.bounds.center - collider.bounds.extents.y * Vector3.up, _balloonStageManager.HammerFXGroundPrefab.transform.rotation);
    }

    public override void OnSecondaryActionPressed(InputAction.CallbackContext context)
    {
        _balloonStageManager.HammerSideCollider.gameObject.SetActive(!_balloonStageManager.HammerSideCollider.gameObject.activeInHierarchy);
    }
}
