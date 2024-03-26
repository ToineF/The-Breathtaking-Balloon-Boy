using System;
using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterGroundPoundState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager, CharacterBaseState previousState)
        {
            Debug.Log("Ground Pound");

            // Movements
            manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.GroundPoundData);

            manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.GroundPoundData);

            manager.MovementManager.GroundPoundTotalHeight = manager.CharacterCollider.Collider.transform.position.y;

        }

        public override void ExitState(CharacterManager manager)
        {
            manager.MovementManager.StartGroundPoundCoroutine(manager);
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.MovementManager.CheckIfGrounded(manager);
        }


        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager);

            manager.MovementManager.UpdateGravity(manager);

            manager.MovementManager.UpdateExternalForces();
        }

        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
