using BlownAway.Character;
using BlownAway.Character.States;
using UnityEngine;

public class CharacterJumpState : CharacterBaseState
{

    public enum JumpState
    {
        ASCENT = 0,
        DESCENT = 1,
        APEX = 2,
    }

    public override void EnterState(CharacterManager manager)
    {
        Debug.Log("JUMP");
        manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.BaseData);

        manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.JumpLateralDeplacementSpeed, manager.Data.LateralMovementData.JumpDeplacementTime, manager.Data.LateralMovementData.JumpDeplacementCurve);

        manager.MovementManager.StartJump(manager);
    }

    public override void ExitState(CharacterManager manager)
    {
    }

    public override void UpdateState(CharacterManager manager)
    {
        manager.MovementManager.UpdateJumpTimer(manager);

        if (manager.MovementManager.JumpTimer > 0) return;

        manager.MovementManager.CheckIfGrounded(manager, true);

        manager.MovementManager.UpdateJumpState(manager);

        manager.MovementManager.CheckIfJumpButtonReleased(manager);

        //manager.MovementManager.CheckForPropulsionStartOnAir(manager);

        manager.MovementManager.CheckForBalloonBounce(manager);

        manager.MovementManager.CheckForGroundPound(manager);

        manager.MovementManager.CheckForDashStart(manager);

    }

    public override void FixedUpdateState(CharacterManager manager)
    {
        //manager.MovementManager.StopMoving(manager, manager.Data.LateralMovementData.JumpDirectionTurnSpeed);
        manager.MovementManager.MoveAtSpeed(manager, manager.Data.LateralMovementData.JumpDirectionTurnSpeed);

        manager.MovementManager.UpdateJumpMovement(manager);

        manager.MovementManager.UpdateGravity(manager);

        manager.MovementManager.UpdateExternalForces();
    }
    public override void LateUpdateState(CharacterManager manager)
    {
    }
}
