using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterWalkingState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager, CharacterBaseState previousState)
        {
            Debug.Log("WALK");
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.WalkData);

            // Gravity
            manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.GroundedData);

            // VFX
            manager.Feedbacks.WalkVFX.Play();
            if (manager.Inputs.StartMoving) GameObject.Instantiate(manager.Data.FeedbacksData.WalkStartVFX, manager.Feedbacks.WalkVFX.transform);
        }

        public override void ExitState(CharacterManager manager)
        {
            //manager.MovementManager.ResetGravity(manager);

            // VFX
            manager.Feedbacks.WalkVFX.Stop();
        }
        public override void UpdateState(CharacterManager manager)
        {

            if (manager.Inputs.MoveInputDirection.magnitude <= 0.0001f)
            {
                manager.States.SwitchState(manager.States.IdleState);
                return;
            }

            manager.MovementManager.CheckIfGrounded(manager);

            manager.MovementManager.CheckForJumpStart(manager);

            manager.MovementManager.UpdateWalkFeedback(manager);

            //manager.MovementManager.CheckForDashStart(manager, true);

            //manager.MovementManager.CheckForJacketToggle(manager);

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager);

            //manager.MovementManager.CheckForStepClimb(manager);

            manager.MovementManager.UpdatePropulsionMovement(manager, false);

            //manager.MovementManager.UpdateGravity(manager, false);

            manager.MovementManager.UpdateExternalForces();

            manager.MovementManager.UpdateStickToGround(manager);
        }

        public override void LateUpdateState(CharacterManager manager)
        {

        }
    }
}