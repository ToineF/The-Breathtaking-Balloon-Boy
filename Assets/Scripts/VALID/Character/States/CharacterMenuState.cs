using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterMenuState : CharacterBaseState
    {
        public override bool IsMovable { get => false; }

        public override void EnterState(CharacterManager manager, CharacterBaseState previousState)
        {
            Debug.Log("MENU");
            manager.CameraManager.IsMovable = false;
            manager.UIManager.ShowUI(false);
            manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.BaseData);
        }

        public override void ExitState(CharacterManager manager)
        {
            manager.CameraManager.IsMovable = true;
            manager.CameraManager.ReactivateCamera();
            manager.UIManager.ShowUI(true);
        }

        public override void UpdateState(CharacterManager manager)
        {

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            if (manager.MovementManager.IsMinGrounded)
                manager.MovementManager.UpdateStickToGround(manager);
            else
                manager.MovementManager.UpdateGravity(manager);

            manager.MovementManager.CheckIfGrounded(manager, false, false);
        }

        public override void LateUpdateState(CharacterManager manager)
        {

        }
    }
}