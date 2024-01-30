using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterFloatingState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("FLOATING");
            manager.MovementManager.SetGravityTo(manager, manager.MovementManager.FloatingGravity, manager.MovementManager.FloatingMaxGravity);
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.MovementManager.GroundData.BaseWalkSpeed, manager.MovementManager.GroundData.BaseWalkTime, manager.MovementManager.GroundData.BaseWalkCurve);

        }

        public override void ExitState(CharacterManager manager)
        {
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.AirManager.ReduceAir(manager.AirManager.FloatingAirReductionSpeed);

            manager.CameraManager.UpdateCameraPosition();

            manager.MovementManager.CheckIfGrounded(manager);

            manager.MovementManager.CheckForPropulsionStartOnAir(manager);

            manager.MovementManager.CheckForFloatCancel(manager);

            manager.MovementManager.CheckIfAirEmpty(manager);
        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager, manager.MovementManager.FloatDeplacementSpeed);

            manager.MovementManager.UpdateGravity(manager);
        }
        public override void LateUpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraAngle(manager);
        }
    }
}