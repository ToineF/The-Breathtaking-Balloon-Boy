using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterPropulsionState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager, CharacterBaseState previousState)
        {
            Debug.Log("PROPULSION");
            manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.PropulsionData);

            manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.PropulsionData);

            manager.MovementManager.LerpPropulsionSpeed(manager, 1, manager.Data.PropulsionData.BasePropulsionAccelTime, manager.Data.PropulsionData.BasePropulsionAccelCurve);

            manager.MovementManager.StartPropulsionTimer(manager);
            manager.MovementManager.ToggleJacketInflate(manager, true);

            manager.MovementManager.EndGroundPound(manager);

            // Animation
            manager.AnimationManager.PlayAnimation(manager.AnimationManager.JumpAnim);
        }

        public override void ExitState(CharacterManager manager)
        {
            manager.MovementManager.LerpPropulsionSpeed(manager, 0, manager.Data.PropulsionData.BasePropulsionDecelTime, manager.Data.PropulsionData.BasePropulsionDecelCurve);
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.AirManager.ReduceAir(manager.Data.AirData.PropulsionAirReductionSpeed);

            manager.MovementManager.FallIfAirEmpty(manager);

            manager.MovementManager.UpdatePropulsionTimer(manager);

            if (manager.MovementManager.PropulsionTimer > 0) return;

            manager.MovementManager.CheckForPropulsionEnd(manager);

            manager.MovementManager.CheckIfGrounded(manager, true);

            manager.MovementManager.CheckForGroundPound(manager);

            manager.MovementManager.CheckForDashStart(manager);

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            if (!manager.Inputs.PropulsionType.HasFlag(Inputs.PropulsionDirection.Lateral))
                manager.MovementManager.MoveAtSpeed(manager);

            manager.MovementManager.UpdatePropulsionMovement(manager);

            manager.MovementManager.UpdateGravity(manager);

            manager.MovementManager.UpdateExternalForces();
        }
        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
