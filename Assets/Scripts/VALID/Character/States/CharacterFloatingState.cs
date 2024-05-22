using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterFloatingState : CharacterBaseState
    {
        public override bool IsMovable { get => true; }

        public override void EnterState(CharacterManager manager, CharacterBaseState previousState)
        {
            Debug.Log("FLOATING");
            manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.FloatingData);

            manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.FloatingData);
        }

        public override void ExitState(CharacterManager manager)
        {
            manager.Feedbacks.DeriveVFX.Stop();
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.AirManager.ReduceAir(manager.Data.AirData.FloatingAirReductionSpeed);

            manager.MovementManager.CheckForDeriveEnd(manager);
            manager.MovementManager.CheckForPropulsionStartOnAir(manager);

            manager.MovementManager.CheckForGroundPound(manager);

            manager.MovementManager.CheckForJacketToggle(manager);
            manager.MovementManager.CheckForJacketDeflated(manager);

            manager.MovementManager.CheckForDashStart(manager);
        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager);

            manager.MovementManager.UpdatePropulsionMovement(manager, false);

            manager.MovementManager.UpdateGravity(manager);

            manager.MovementManager.UpdateExternalForces();

            manager.MovementManager.CheckIfGrounded(manager, true);
        }
        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}