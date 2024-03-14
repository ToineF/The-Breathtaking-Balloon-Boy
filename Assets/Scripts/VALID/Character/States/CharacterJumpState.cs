using BlownAway.Character;
using BlownAway.Character.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CharacterJumpState : CharacterBaseState
{
    public override void EnterState(CharacterManager manager)
    {
        Debug.Log("JUMP");
        manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.JumpGravity, manager.Data.FallData.JumpMinGravity, manager.Data.FallData.JumpMaxGravity, manager.Data.FallData.JumpGravityIncreaseByFrame, manager.Data.FallData.JumpGravityIncreaseDecelerationByFrame, manager.Data.FallData.JumpGravityTime, manager.Data.FallData.JumpGravityAccel);

        manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.JumpLateralDeplacementSpeed, manager.Data.LateralMovementData.JumpDeplacementTime, manager.Data.LateralMovementData.JumpDeplacementCurve);

        manager.MovementManager.StartJump(manager);
    }

    public override void ExitState(CharacterManager manager)
    {
    }

    public override void UpdateState(CharacterManager manager)
    {
        manager.MovementManager.UpdateJumpTimer(manager);

        manager.MovementManager.CheckIfGrounded(manager, true);

        if (manager.MovementManager.JumpTimer > 0) return;

        manager.MovementManager.CheckForPropulsionStartOnAir(manager);

        manager.MovementManager.CheckForBalloonBounce(manager);

        manager.MovementManager.CheckForGroundPound(manager);

        manager.MovementManager.CheckForDashStart(manager);

    }

    public override void FixedUpdateState(CharacterManager manager)
    {
            manager.MovementManager.MoveAtSpeed(manager, manager.Data.LateralMovementData.JumpDirectionTurnSpeed);

        manager.MovementManager.UpdateJumpMovement(manager);

        manager.MovementManager.UpdateGravity(manager);

        manager.MovementManager.UpdateExternalForces();
    }
    public override void LateUpdateState(CharacterManager manager)
    {
    }
}
