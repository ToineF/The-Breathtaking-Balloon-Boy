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

    public override void EnterState(CharacterManager manager, CharacterBaseState previousState)
    {
        Debug.Log("JUMP");
        manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.JumpAscentData);

        manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.JumpAscentData);

        manager.MovementManager.StartJump(manager);

        // Animation
        manager.AnimationManager.PlayAnimation(manager.AnimationManager.JumpAnim);
    }

    public override void ExitState(CharacterManager manager)
    {
    }

    public override void UpdateState(CharacterManager manager)
    {
        manager.MovementManager.UpdateJumpTimer(manager);

        if (manager.MovementManager.JumpTimer > 0) return;

        manager.MovementManager.UpdateJumpState(manager);

        manager.MovementManager.CheckIfJumpButtonReleased(manager);

        manager.MovementManager.CheckIfGrounded(manager, true);

        manager.MovementManager.CheckForGroundPound(manager);

        manager.MovementManager.CheckForDashStart(manager);

        if (manager.MovementManager.JumpPropulsionTimer > 0) return;

        manager.MovementManager.CheckForPropulsionStartOnAir(manager); // HERE CHECK IF BUTTON PRESSED

    }

    public override void FixedUpdateState(CharacterManager manager)
    {
        //manager.MovementManager.StopMoving(manager, manager.Data.LateralMovementData.JumpDirectionTurnSpeed);
        manager.MovementManager.MoveAtSpeed(manager);

        manager.MovementManager.UpdateJumpMovement(manager);

        manager.MovementManager.UpdateGravity(manager);

        manager.MovementManager.UpdateExternalForces();
    }
    public override void LateUpdateState(CharacterManager manager)
    {
    }
}
