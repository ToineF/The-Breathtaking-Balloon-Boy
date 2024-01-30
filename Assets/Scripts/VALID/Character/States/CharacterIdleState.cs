using UnityEngine;


namespace BlownAway.Character.States
{
    public class CharacterIdleState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("IDLE");

            // Movements
            manager.Inputs.ResetLastMoveInputDirection();
            manager.MovementManager.LerpDeplacementSpeed(manager, 0, manager.MovementManager.GroundData.BaseIdleTime, manager.MovementManager.GroundData.BaseIdleCurve);

            // Air
            manager.AirManager.RefreshAir();
        }

        public override void ExitState(CharacterManager manager)
        {
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraPosition();
            if (manager.Inputs.MoveInputDirection.magnitude > 0.0001f)
            {
                manager.States.SwitchState(manager.States.WalkingState);
                return;
            }

            manager.MovementManager.CheckForPropulsionStartOnGround(manager);

            manager.MovementManager.CheckIfGrounded(manager);

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager, 0, false);
        }
        public override void LateUpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraAngle(manager);
        }
    }

}