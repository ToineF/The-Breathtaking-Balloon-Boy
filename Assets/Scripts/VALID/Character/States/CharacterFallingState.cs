using System;
using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterFallingState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("FALLING");
            manager.MovementManager.SetGravityTo(manager, manager.MovementManager.BaseGravity, manager.MovementManager.BaseMaxGravity);
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.MovementManager.GroundData.BaseWalkSpeed, manager.MovementManager.GroundData.BaseWalkTime, manager.MovementManager.GroundData.BaseWalkCurve);

            manager.AirManager.AddAirUntilFullIfEmpty(manager, manager.AirManager.FallingAirRefillSpeed, manager.AirManager.FallingAirRefillDelay);
        }

        public override void ExitState(CharacterManager manager)
        {
            manager.AirManager.StopAddingAir();
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraPosition();

            manager.MovementManager.CheckIfGrounded(manager);

            manager.MovementManager.CheckForPropulsionStartOnAir(manager);
        }


        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager, manager.MovementManager.FallDeplacementSpeed);

            manager.MovementManager.UpdateGravity(manager);
        }

        public override void LateUpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraAngle(manager);

        }
    }
}
